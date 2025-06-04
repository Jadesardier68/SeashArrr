using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    public enum Role
    {
        Captain,
        //Cook,
       // Carpenter,
       // Engineer,
        Doctor,
        Fighter,
        //Cannonier
    }

    private static int roleIndex = 0;

    [SerializeField] public int HPMax;
    [SerializeField] public int ATT;
    [SerializeField] public int CanonPower;
    [SerializeField] public int HealPower;
    [SerializeField] public float BoostPower;
    [SerializeField] public int FixPower;
    [SerializeField] public UIManager UIManager;
    [SerializeField] public StatsManager statsManager;
    public Battle_Handler battleHandler;
    public int playerIndex; // Index du joueur dans battleHandler.Players

    public bool isBoosted;
    public Role role;
    public GameObject CaptainPrefab;
    public GameObject DoctorPrefab;
    public string portraitSpriteName;

    public int HP;

    public InputAction AttackInput;
    public InputAction HealInput;
   // public InputAction BoostInput;
    public InputAction CanonInput;
    //public InputAction FixInput;
   // public InputAction ItemInput;
   // public InputAction CanonFixInput;
    public InputAction BoatFixInput;
    //public InputAction Annuler;

    private int currentTargetIndex;

    public class PlayerInfo : MonoBehaviour
    {
        public int playerId;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the good skin on the prefab
        switch (role)
        {
            case Role.Captain:
                break;
            //case Role.Cook:
              //  break;
            //case Role.Carpenter:
              //  break;
            //case Role.Engineer:
              //  break;
            case Role.Doctor:
                break;
            case Role.Fighter:
                break;
            //case Role.Cannonier:
              //  break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        HP = HPMax;
        AssignRole();
        ChangePrefab();

        GameObject playerObject = GameObject.FindGameObjectWithTag("StatsManager");
        GameObject battleManager = GameObject.FindGameObjectWithTag("BattleManager");
        GameObject UIManagerObject = GameObject.FindGameObjectWithTag("BattleManager");

        statsManager = playerObject.GetComponent<StatsManager>();
        UIManager = UIManagerObject.GetComponent<UIManager>();
        battleHandler = battleManager.GetComponent<Battle_Handler>();

    }

    private void AssignRole()
    {
        // Définir le rôle basé sur l'index global
        role = (Role)roleIndex;

        // Incrémenter l'index global et revenir au début si nécessaire
        roleIndex = (roleIndex + 1) % Enum.GetValues(typeof(Role)).Length;
    }

        
    public void ChangePrefab()
    {
        if (roleIndex == 1) //Captain
            {
            CaptainPrefab.SetActive(true);
            ATT = 25;
            CanonPower = 10;
            HealPower = 50;
            BoostPower = 0.5f;
            FixPower = 100;
            portraitSpriteName = "Capitaine";
            }
        if (roleIndex == 2) // Doctor
        {
            DoctorPrefab.SetActive(true);
            ATT = 25;
            CanonPower = 10;
            HealPower = 100;
            BoostPower = 0f;
            FixPower = 100;
            portraitSpriteName = "Médecin";
        }
    }


    public int GetHP()
    {
        return HP;
    }

    public void SetHP(int newHP)
    {
        HP = Mathf.Clamp(newHP, 0, HPMax);
        UIManager.UpdateHealthSlider(playerIndex, HP);
    }

    public int GetHPMax()
    {
        return HPMax;
    }

    public int GetRoleIndex()
    {
        return roleIndex;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Max(HP, 0);

        // Met à jour le slider de vie via UIManager
        if (UIManager != null)
        {
            UIManager.UpdateHealthSlider(playerIndex, HP);
        }

        Debug.Log($"[Player {playerIndex}] Dégâts subis : {damage} | PV restants : {HP}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
