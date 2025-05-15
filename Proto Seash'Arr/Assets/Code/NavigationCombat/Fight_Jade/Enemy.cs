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

    [SerializeField] private int HPMax;
    [SerializeField] private int ATT;
    [SerializeField] private int CanonBoatATT;
    [SerializeField] private int AllATT;
    [SerializeField] private int HealPower;
    [SerializeField] private int BoostPower;
    

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
    public void Attack()
    {

        player.HP -= ATT;

        Debug.Log("un joueur a été attaqué");
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

    public void Boost()
    {
        isBoosted = true;
        ATT += BoostPower;

        Debug.Log("un monstre a été boosté");
    }

    public void Heal()
    {
        HP += HealPower;
        Debug.Log("un monstre a été soigné");
    }
}
