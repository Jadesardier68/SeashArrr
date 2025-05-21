using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    public enum Type
    {
        Fighter,
        Destroyer,
        Healer,
        AOE,
        Boss
    }

     public int HPMax;
     public int ATT;
     public int CanonBoatATT;
    public int AllATT;
    public int HealPower;
    public int BoostPower;
    

    public bool isBoosted;
    public Type type;


    private Player player;
    public List<Player> allPlayers;
    public StatsManager statsManager;
    

    public int HP;

    // Start is called before the first frame update
    void Start()
    {
        // Initialisation de la liste des joueurs présents dans la scène
        allPlayers = new List<Player>(FindObjectsOfType<Player>());
        statsManager = FindObjectOfType<StatsManager>();

            // HP de départ
        HP = HPMax;

            // (Ton switch type ici si tu veux faire un comportement selon le type)
  
        switch (type)
        {
            case Type.Fighter:
                break;
            case Type.Destroyer:
                break;
            case Type.Healer:
                break;
            case Type.AOE:
                break;
            case Type.Boss:
                break;
        }


    }

    public int GetHP()
    {
        return HP;
    }

    public void SetHP(int value)
    {
        HP = Mathf.Clamp(value, 0, HPMax); // pour éviter des HP négatifs ou > max
        if (HP <= 0)
        {
            Die(); // si tu veux gérer la mort ici
        }
    }

    private void Die()
    {
        Debug.Log($"{name} est mort.");
        Destroy(gameObject);
    }


    public void Attack()
    {
        List<Player> alivePlayers = allPlayers.FindAll(p => p.HP > 0);
        if (alivePlayers.Count == 0) return;

        Player target = alivePlayers[Random.Range(0, alivePlayers.Count)];
        target.HP -= ATT;

        Debug.Log("Le joueur " + target.name + " a été attaqué pour " + ATT);
    }
    public void CanonBoatAttack()
    {
        var focus = Random.Range(1, 4);

        if (focus <= 3)
        {
            statsManager.boatHealth-=CanonBoatATT;
            statsManager.UpdateText();
        }
        else
        {
            statsManager.canonHealth -= CanonBoatATT;
            statsManager.UpdateText();
        }

        Debug.Log("le canon ou bateau a été attaqué");
    }

    public void AllAttack()
    {
        int tempATT;
        var chance = Random.Range(1, 100);

        if (chance <= 15)
            tempATT = 5;
        else if (chance <= 50)
            tempATT = 10;
        else if (chance <= 85)
            tempATT = 15;
        else
            tempATT = 20;

        foreach (var p in allPlayers)
        {
            p.HP -= tempATT; 
        }

        Debug.Log("un monstre a attaqué tous les joueurs");
    }

    public void Heal()
    {
        HP = Mathf.Min(HP + HealPower, HPMax);
        Debug.Log(name + " s'est soigné pour " + HealPower + " HP. Total: " + HP);
    }

    public void Boost()
    {
        if (!isBoosted)
        {
            isBoosted = true;
            ATT += BoostPower;
            Debug.Log(name + " a été boosté. Nouvelle ATT: " + ATT);
        }
    }
}
