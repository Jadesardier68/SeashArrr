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
    public string portraitSpriteName;


    private Player player;
    public List<Player> allPlayers;
    public StatsManager statsManager;
    Animator animator;
    

    public int HP;

    // Start is called before the first frame update

    void Awake()
    {
        animator = GetComponent<Animator>();
        // ou : animator = GetComponentInChildren<Animator>();
    }
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

        Battle_Handler handler = FindObjectOfType<Battle_Handler>();
        if (handler != null)
        {
            handler.Ennemies.Remove(gameObject);
            handler.turnOrder.Remove(gameObject);
        }

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
    public IEnumerator CanonBoatAttack()
    {
        var focus = Random.Range(1, 4);

        if (animator == null)
        {
            Debug.LogError("Animator est null !");
            yield break;
        }

        if (focus <= 3)
        {
            animator.SetBool("attack", true);
            yield return new WaitForSeconds(0.5f); // temps avant impact
            statsManager.boatHealth-=CanonBoatATT;
            statsManager.UpdateText();
        }
        else
        {
            animator.SetBool("attack", true);
            yield return new WaitForSeconds(0.5f); // temps avant impact
            statsManager.canonHealth -= CanonBoatATT;
            statsManager.UpdateText();
        }

        yield return new WaitForSeconds(0.5f); // attendre fin animation

        Debug.Log("le canon ou bateau a été attaqué");

        animator.SetBool("attack", false);
    }


    public IEnumerator AllAttack()
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

        if (animator == null)
        {
            Debug.LogError("Animator est null !");
            yield break;
        }

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.5f); // temps avant impact

        if (allPlayers == null)
        {
            Debug.LogError("allPlayers est null !");
            yield break;
        }

        foreach (var p in allPlayers)
        {
            if (p == null)
            {
                Debug.LogError("Un joueur dans allPlayers est null !");
                continue;
            }

            p.HP -= tempATT;
        }

        Debug.Log("un monstre a attaqué tous les joueurs");

        yield return new WaitForSeconds(2.5f); // attendre fin animation

        animator.SetBool("Attack", false);
    }

    public IEnumerator Heal()
    {
        if (animator == null)
        {
            Debug.LogError("Animator est null !");
            yield break;
        }

        animator.SetBool("Attack", true);

        HP = Mathf.Min(HP + HealPower, HPMax);
        Debug.Log(name + " s'est soigné pour " + HealPower + " HP. Total: " + HP);

        yield return new WaitForSeconds(0.5f); // attendre fin animation

        animator.SetBool("Attack", false);
    }

    public IEnumerator Boost()
    {
        if (animator == null)
        {
            Debug.LogError("Animator est null !");
            yield break;
        }

        animator.SetBool("Attack", true);

        if (!isBoosted)
        {
            isBoosted = true;
            ATT += BoostPower;
            Debug.Log(name + " a été boosté. Nouvelle ATT: " + ATT);
        }

        yield return new WaitForSeconds(0.5f); // attendre fin animation

        animator.SetBool("Attack", false);
    }
}
