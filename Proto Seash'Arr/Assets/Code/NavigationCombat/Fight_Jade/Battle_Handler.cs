using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Battle_Handler : MonoBehaviour
{
    [HideInInspector] public List<GameObject> Players = new List<GameObject>();
    [HideInInspector] public List<GameObject> Ennemies = new List<GameObject>();
    [HideInInspector] public List<GameObject> turnOrder = new List<GameObject>();
    public List<Transform> playerPlaceholders;
    [HideInInspector] public GameObject currentUnit;
    public GameObject[] BanqueEnnemies;
    private int currentTurnIndex = 0;
    public bool isBattleOver = false;
    public bool isTurnOver = false;
    public StatsManager statsManager;
    private bool fightStarted = false;

    public GameObject turnPopupTextObject;
    public TextMeshProUGUI turnPopupText;

    /*public GameObject ordrePanel4;
    public GameObject ordrePanel5;
    public GameObject ordrePanel6;*/
    public UIManager UIManager;

    void Start()
    {
        Players.Clear();
        Ennemies.Clear();
    }



    private void Update()
    {
        if (statsManager.Fight && !fightStarted)
        {
            fightStarted = true;
            OnFightBegin();
        }
    }

    public void OnFightBegin()
    {
        Players.Clear();
        Ennemies.Clear();

        Players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        GenerateEnemies();
        BuildTurnOrder();
        //UpdateOrderPanel(); // ⬅️ Appel ici

        StartCoroutine(BattleLoop());

        for (int i = 0; i < Players.Count && i < playerPlaceholders.Count; i++)
        {
            Transform modele3D = Players[i].transform.GetComponentInChildren<Transform>(true);
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
                // Position absolue du Modele3D sur le placeholder
                modele3D.position = playerPlaceholders[i].position;

                // Bloquer le Rigidbody du Modele3D
                Rigidbody rbChild = modele3D.GetComponent<Rigidbody>();
                if (rbChild != null)
                {
                    rbChild.isKinematic = true;
                }
                else
                {
                    Debug.LogWarning($"Pas de Rigidbody sur le Modele3D : {modele3D.name}");
                }

                // Bloquer le Rigidbody du Player (parent)
                Rigidbody rbParent = Players[i].GetComponent<Rigidbody>();
                if (rbParent != null)
                {
                    rbParent.isKinematic = true;
                }
                else
                {
                    Debug.LogWarning($"Pas de Rigidbody sur le Player : {Players[i].name}");
                }

                Debug.Log($"Modele3D '{modele3D.name}' et son parent '{Players[i].name}' placés et figés.");
            }
            else
            {
                Debug.LogWarning($"Modele3D introuvable dans {Players[i].name}");
            }
        }

    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();
        foreach (var player in Players)
        {
            if (player.GetComponent<Player>() != null)
                turnOrder.Add(player);
            else
                Debug.LogWarning("Player sans component Player détecté : " + player.name);
        }

        foreach (var enemy in Ennemies)
        {
            if (enemy.GetComponent<Enemy>() != null)
                turnOrder.Add(enemy);
            else
                Debug.LogWarning("Enemy sans component Enemy détecté : " + enemy.name);
        }

        ShuffleList(turnOrder);
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void GenerateEnemies()
    {
        int enemyCount;

        if (statsManager.TimerTotal < 300)
            enemyCount = 1;
        else if (statsManager.TimerTotal < 600)
            enemyCount = 2;
        else
            enemyCount = 3;

        HashSet<int> selectedIndices = new HashSet<int>();

        while (Ennemies.Count < enemyCount && Ennemies.Count < BanqueEnnemies.Length)
        {
            int index = Random.Range(0, BanqueEnnemies.Length);
            if (!selectedIndices.Contains(index))
            {
                GameObject enemyInstance = Instantiate(BanqueEnnemies[index]);
                enemyInstance.tag = "Enemy";
                Ennemies.Add(enemyInstance);
                selectedIndices.Add(index);
            }
        }
    }

    private IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(2f);

        while (!isBattleOver)
        {
            if (Ennemies.Count == 0)
            {
                Debug.Log("Battle Over!");
                isBattleOver = true;
                fightStarted = false;
                yield break;

            }

            else if (statsManager.boatHealth <= 0)
            {
                Debug.Log("Battle Over!");
                isBattleOver = true;
                fightStarted = false;
                yield break;
            }


            currentUnit = turnOrder[currentTurnIndex];
            yield return StartCoroutine(ShowTurnText(currentUnit));
            Debug.Log(turnOrder[currentTurnIndex] + "joue");
            isTurnOver = false;

            if (currentUnit.CompareTag("Player"))
            {
                Player player = currentUnit.GetComponent<Player>();
                isTurnOver = false;
                
                if (player != null && player.HP > 0)
                {
                    yield return StartCoroutine(UIManager.StartPlayerTurn(player, OnPlayerChoice));
                }

                else
                {
                    Debug.Log("wesh je veux pas fonctionner");
                }
            }
            else if (currentUnit.CompareTag("Enemy"))
            {
                Enemy enemy = currentUnit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    yield return StartCoroutine(EnemyTurn(enemy));
                }
                else
                {
                    isTurnOver = true;
                }
            }

            // Wait until action is complete
            while (!isTurnOver)
                yield return null;

            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;

            yield return new WaitForSeconds(1f);
        }
    }

    public void OnPlayerChoice(int actionIndex, int targetIndex)
    {
        Debug.Log($"Player chose action {actionIndex} on target {targetIndex}");

        Player player = currentUnit.GetComponent<Player>();

        // Appliquer l'action ici selon le choix :
        switch (actionIndex)
        {
            case 0: // Attaque
                if (targetIndex >= 0 && targetIndex < Ennemies.Count)
                {
                    Enemy targetEnemy = Ennemies[targetIndex].GetComponent<Enemy>();
                    if (targetEnemy != null)
                    {
                        targetEnemy.SetHP(targetEnemy.GetHP() - player.ATT);
                        Debug.Log("allez go je l'ai tapé");
                    }
                }
                break;

            case 1: // Heal
                if (targetIndex >= 0 && targetIndex < Players.Count && statsManager.nbrRagout > 0)
                {
                    Player targetPlayer = Players[targetIndex].GetComponent<Player>();
                    if (targetPlayer != null)
                    {
                        statsManager.nbrRagout -= 1;
                        statsManager.UpdateText();
                        targetPlayer.SetHP(targetPlayer.GetHP() + targetPlayer.HealPower);
                        Debug.Log("j'ai soigné untel");
                    }
                }

                else
                {
                    isTurnOver = true;
                }
                break;

            case 2: // Canon
                foreach (var e in Ennemies)
                {
                    Enemy enemy = e.GetComponent<Enemy>();
                    enemy.SetHP(enemy.GetHP() - player.CanonPower);
                    Debug.Log("wesh le canon est trop cool");
                }
                break;

            case 3: // BoatFix
                if(statsManager.nbrWood > 20)
                {
                    statsManager.boatHealth += player.FixPower;
                    statsManager.nbrWood -= 20;
                    statsManager.UpdateText(); // facultatif : pour afficher les PV mis à jour
                   
                }
                else
                {
                    isTurnOver = true;
                }
                break;
        }

        isTurnOver = true;
    }

    private IEnumerator ShowTurnText(GameObject unit)
    {
        string unitName = "";

        if (unit.CompareTag("Player"))
        {
            Player p = unit.GetComponent<Player>();
            unitName = p.portraitSpriteName;
        }
        else if (unit.CompareTag("Enemy"))
        {
            Enemy e = unit.GetComponent<Enemy>();
            unitName = e.portraitSpriteName;
        }

        turnPopupText.text = $"{unitName} commence à jouer";
        turnPopupTextObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        turnPopupTextObject.SetActive(false);
    }

    private IEnumerator EnemyTurn(Enemy enemy)
    {
        yield return new WaitForSeconds(1f); // temps de réflexion

        switch (enemy.type)
        {
           /* case Enemy.Type.Fighter:
                yield return enemy.Attack(); // ATTENTION : doit retourner IEnumerator
                break;*/

            case Enemy.Type.Destroyer:
                yield return enemy.CanonBoatAttack();
                break;

            case Enemy.Type.Healer:
                int choice = Random.Range(0, 1);
                if (choice == 0) enemy.Heal();
                else if (choice == 1) enemy.Boost();
                // Si tu veux, tu peux activer ça aussi :
                // else yield return enemy.Attack();
                break;
          
            case Enemy.Type.AOE:
                yield return enemy.AllAttack(); // attendre fin anim
                break;

           /* case Enemy.Type.Boss:
                choice = Random.Range(0, 3);
                if (choice == 0) yield return enemy.AllAttack();
                else if (choice == 1) yield return enemy.CanonBoatAttack();
                else yield return enemy.Attack();
                break;*/
        }

        isTurnOver = true;
    }

    /*private void UpdateOrderPanel()
    {
        ordrePanel4.SetActive(false);
        ordrePanel5.SetActive(false);
        ordrePanel6.SetActive(false);

        GameObject activePanel = null;
        int count = turnOrder.Count;

        if (count == 4)
            activePanel = ordrePanel4;
        else if (count == 5)
            activePanel = ordrePanel5;
        else if (count == 6)
            activePanel = ordrePanel6;
        else
        {
            Debug.LogWarning($"Aucun panel défini pour {count} unités dans le tour.");
            return;
        }

        activePanel.SetActive(true);

        for (int i = 0; i < activePanel.transform.childCount; i++)
        {
            Transform child = activePanel.transform.GetChild(i);
            Image portraitImage = child.GetComponent<Image>();

            if (i < turnOrder.Count)
            {
                GameObject unit = turnOrder[i];
                string spriteName = "";

                if (unit.CompareTag("Player"))
                {
                    Player p = unit.GetComponent<Player>();
                    spriteName = p.portraitSpriteName;
                }
                else if (unit.CompareTag("Enemy"))
                {
                    Enemy e = unit.GetComponent<Enemy>();
                    spriteName = e.portraitSpriteName;
                }

                Sprite portrait = Resources.Load<Sprite>($"Sprites/Portraits/{spriteName}");

                if (portrait != null)
                {
                    portraitImage.sprite = portrait;
                    portraitImage.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"Portrait non trouvé pour : {spriteName}");
                    portraitImage.enabled = false;
                }
            }
            else
            {
                portraitImage.enabled = false;
            }
        }
    }*/


}