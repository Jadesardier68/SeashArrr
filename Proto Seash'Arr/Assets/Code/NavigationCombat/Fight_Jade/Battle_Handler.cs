using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Battle_Handler : MonoBehaviour
{
    public PlayVFX playVFX;

    [HideInInspector] public List<GameObject> Players = new List<GameObject>();
    [HideInInspector] public List<GameObject> Ennemies = new List<GameObject>();
    [HideInInspector] public List<GameObject> turnOrder = new List<GameObject>();

    public List<Transform> playerPlaceholders;
    public GameObject[] BanqueEnnemies;
    public GameObject turnPopupTextObject;
    public TextMeshProUGUI turnPopupText;

    public Camera fightCamera;

    public StatsManager statsManager;
    public UIManager UIManager;
    public AudioManager audioManager;

    public GameObject currentUnit;
    private int currentTurnIndex = 0;

    private bool fightStarted = false;
    public bool isBattleOver = false;
    public bool isTurnOver = false;

    void Start()
    {
        Players.Clear();
        Ennemies.Clear();
    }

    void Update()
    {
        if (statsManager.Fight && !fightStarted)
        {
            fightStarted = true;
            OnFightBegin();
        }
    }

    public void OnFightBegin()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        GenerateEnemies();
        BuildTurnOrder();
        StartCoroutine(BattleLoop());
        UIManager.InitializeHealthSliders();

        for (int i = 0; i < Players.Count && i < playerPlaceholders.Count; i++)
        {
            Transform modele3D = null;

            foreach (Transform child in Players[i].GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("Modele3D"))
                {
                    modele3D = child;
                    break;
                }
            }

            if (modele3D != null)
            {
                Rigidbody rbChild = modele3D.GetComponent<Rigidbody>();
                if (rbChild != null)
                {
                    rbChild.isKinematic = false;
                    rbChild.useGravity = true;
                    rbChild.velocity = Vector3.zero;
                    rbChild.angularVelocity = Vector3.zero;

                    rbChild.MovePosition(playerPlaceholders[i].position); // Physique-safe placement
                }
                else
                {
                    modele3D.position = playerPlaceholders[i].position; // Fallback
                }

                Debug.Log($"Positionné : {modele3D.name} de {Players[i].name}");
            }
            else
            {
                Debug.LogWarning($"Aucun modèle 3D trouvé pour {Players[i].name}");
            }
        }
    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();
        turnOrder.AddRange(Players);
        for (int i = 0; i < Players.Count; i++)
        {
            Player playerComp = Players[i].GetComponent<Player>();
            if (playerComp != null)
            {
                playerComp.playerIndex = i;
            }
        }
        turnOrder.AddRange(Ennemies);
        ShuffleList(turnOrder);
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private void GenerateEnemies()
    {
        int enemyCount = statsManager.TimerTotal < 300 ? 1 : (statsManager.TimerTotal < 600 ? 2 : 3);
        HashSet<int> selectedIndices = new HashSet<int>();

        while (Ennemies.Count < enemyCount && Ennemies.Count < BanqueEnnemies.Length)
        {
            int index = Random.Range(0, BanqueEnnemies.Length);
            if (!selectedIndices.Contains(index))
            {
                GameObject enemy = Instantiate(BanqueEnnemies[index]);
                enemy.tag = "Enemy";
                Ennemies.Add(enemy);
                selectedIndices.Add(index);
            }
        }
    }

    private IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(2f);

        while (!isBattleOver)
        {
            if (Ennemies.Count == 0 || statsManager.boatHealth <= 0)
            {
                isBattleOver = true;
                fightStarted = false;
                CleanupBattle();
                yield break;
            }

            currentUnit = turnOrder[currentTurnIndex];

            if (currentUnit == null)
            {
                turnOrder.RemoveAt(currentTurnIndex);
                currentTurnIndex %= turnOrder.Count;
                continue;
            }

            yield return StartCoroutine(ShowTurnText(currentUnit));
            isTurnOver = false;

            if (currentUnit.CompareTag("Player"))
            {
                Player player = currentUnit.GetComponent<Player>();
                if (player && player.HP > 0)
                {
                    yield return StartCoroutine(UIManager.StartPlayerTurn(player, OnPlayerChoice));
                }
                else
                {
                    isTurnOver = true;
                }
            }
            else if (currentUnit.CompareTag("Enemy"))
            {
                Enemy enemy = currentUnit.GetComponent<Enemy>();
                if (enemy)
                    yield return StartCoroutine(EnemyTurn(enemy));
                else
                    isTurnOver = true;
            }

            while (!isTurnOver)
                yield return null;

            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnPlayerChoice(int actionIndex, int targetIndex)
    {
        Player player = currentUnit.GetComponent<Player>();
        Anim_Nav animNav = GetAnimNavFrom(currentUnit);

        switch (actionIndex)
        {
            case 0: // Attaque
                if (IsValidTarget(targetIndex, Ennemies))
                {
                    animNav.Attaquer();
                    Enemy enemy = Ennemies[targetIndex].GetComponent<Enemy>();
                    enemy.SetHP(enemy.GetHP() - player.ATT);
                }
                break;

            case 1: // Heal
                if (IsValidTarget(targetIndex, Players) && statsManager.nbrRagout > 0)
                {
                    animNav.Soigner();
                    statsManager.nbrRagout--;
                    statsManager.UpdateText();

                    Player target = Players[targetIndex].GetComponent<Player>();
                    target.SetHP(target.GetHP() + target.HealPower);

                    // 🔧 AJOUTE CECI POUR METTRE À JOUR LE SLIDER
                    UIManager.UpdateHealthSlider(target.playerIndex, target.HP);
                }
                break;

            case 2: // Canon
                animNav.Reparer();
                if (playVFX != null)
                {
                    playVFX.PlaySmokeCanon();
                    playVFX.PlayMecheCanon();
                }
                for (int i = Ennemies.Count - 1; i >= 0; i--)
                {
                    if (Ennemies[i] != null)
                    {
                        Enemy enemy = Ennemies[i].GetComponent<Enemy>();
                        enemy.SetHP(enemy.GetHP() - player.CanonPower);
                    }
                }
                break;

            case 3: // Réparer bateau
                if (statsManager.nbrWood >= 20)
                {
                    animNav.Reparer();
                    statsManager.boatHealth += player.FixPower;
                    statsManager.nbrWood -= 20;
                    statsManager.UpdateText();
                }
                break;
        }

        isTurnOver = true;
    }

    private IEnumerator ShowTurnText(GameObject unit)
    {
        string unitName = unit.CompareTag("Player") ? unit.GetComponent<Player>().portraitSpriteName :
                          unit.CompareTag("Enemy") ? unit.GetComponent<Enemy>().portraitSpriteName : "???";

        turnPopupText.text = $"{unitName} commence à jouer";
        turnPopupTextObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        turnPopupTextObject.SetActive(false);
    }

    private IEnumerator EnemyTurn(Enemy enemy)
    {
        yield return new WaitForSeconds(1f);

        switch (enemy.type)
        {
            case Enemy.Type.Destroyer:
                yield return enemy.CanonBoatAttack();
                break;

            case Enemy.Type.Healer:
                int r = Random.Range(0, 2);
                if (r == 0) enemy.Heal();
                else enemy.Boost();
                break;

            case Enemy.Type.AOE:
                yield return enemy.AllAttack();
                break;
        }

        isTurnOver = true;
    }

    private Anim_Nav GetAnimNavFrom(GameObject unit)
    {
        foreach (Transform child in unit.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Modele3D"))
            {
                return child.GetComponent<Anim_Nav>();
            }
        }

        Debug.LogWarning($"Aucun Anim_Nav trouvé dans {unit.name}");
        return null;
    }

    private bool IsValidTarget(int index, List<GameObject> list)
    {
        return index >= 0 && index < list.Count && list[index] != null;
    }

    private void CleanupBattle()
    {
        StopAllCoroutines();

        foreach (var enemy in Ennemies)
        {
            if (enemy != null) Destroy(enemy);
        }

        Ennemies.Clear();
        Players.Clear();
        turnOrder.Clear();

        currentUnit = null;
        currentTurnIndex = 0;
        isTurnOver = false;
        isBattleOver = false;
        fightStarted = false;

        statsManager.Fight = false;
        statsManager.Navigation = true;
        statsManager.CanonVieFight.SetActive(false);
        statsManager.sliderNavigation.SetActive(true);
        statsManager.canonHealthGO.SetActive(true);
        audioManager.PlayMusic(AudioManager.Music.NavTheme);

        if (fightCamera != null)
            fightCamera.gameObject.SetActive(false);
    }
}