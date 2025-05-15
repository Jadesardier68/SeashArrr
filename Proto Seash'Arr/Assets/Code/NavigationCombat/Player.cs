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
    

    public bool isBoosted;
    public Role role;
    public GameObject CaptainPrefab;
    public GameObject DoctorPrefab;

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
        // D�finir le r�le bas� sur l'index global
        role = (Role)roleIndex;

        // Incr�menter l'index global et revenir au d�but si n�cessaire
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
            }
        if (roleIndex == 2) // Doctor
        {
            DoctorPrefab.SetActive(true);
            ATT = 25;
            CanonPower = 10;
            HealPower = 100;
            BoostPower = 0f;
            FixPower = 100;
        }
    }


    public int GetHP()
    {
        return HP;
    }

    public void SetHP(int newHP)
    {
        HP = newHP;
    }

    public int GetHPMax()
    {
        return HPMax;
    }

    public int GetRoleIndex()
    {
        return roleIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
