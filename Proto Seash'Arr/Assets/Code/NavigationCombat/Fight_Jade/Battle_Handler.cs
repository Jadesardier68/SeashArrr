using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Battle_Handler : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>();
    public List<GameObject> Ennemies = new List<GameObject>();
    public GameObject[] BanqueEnnemies;
    public List<GameObject> turnOrder = new List<GameObject>();
    private int currentTurnIndex = 0;
    public bool isBattleOver = false;
    public bool isTurnOver = false;
    public StatsManager statsManager;
    private bool fightStarted = false;

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
        StartCoroutine(BattleLoop());
    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();
        turnOrder.AddRange(Players);
        turnOrder.AddRange(Ennemies);
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
            enemyCount = 3;
        else if (statsManager.TimerTotal < 600) 
            enemyCount = 4;
        else
            enemyCount = 5;

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
            GameObject currentUnit = turnOrder[currentTurnIndex];
            Debug.Log(turnOrder[currentTurnIndex] + "joue");
            isTurnOver = false;

            if (currentUnit.CompareTag("Player"))
            {
                Player player = currentUnit.GetComponent<Player>();
                if (player != null && player.GetHP() > 0)
                {
                    yield return StartCoroutine(player.Action());
                }
                else
                {
                    isTurnOver = true;
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

    private IEnumerator EnemyTurn(Enemy enemy)
    {
        yield return new WaitForSeconds(5f);

        switch (enemy.type)
        {
            case Enemy.Type.Fighter:
                enemy.Attack();
                break;

            case Enemy.Type.Destroyer:
                enemy.CanonBoatAttack();
                break;

            case Enemy.Type.Healer:
                int choice = Random.Range(0, 3);
                if (choice == 0) enemy.Heal();
                else if (choice == 1) enemy.Boost();
               // else enemy.Attack();

                break;

            case Enemy.Type.AOE:
                enemy.AllAttack();
                break;

            case Enemy.Type.Boss:
                choice = Random.Range(0, 3);
                if (choice == 0) enemy.AllAttack();
                else if (choice == 1) enemy.CanonBoatAttack();
                else enemy.Attack();
                break;
        }

        isTurnOver = true;
    }
}



