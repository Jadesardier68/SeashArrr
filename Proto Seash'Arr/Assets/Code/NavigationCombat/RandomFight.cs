using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class RandomFight : MonoBehaviour
{
    // Temps de jeu
    [Header("Temps globaux")]
    public float TempsNavigation = 0;
    public float TempsFight = 0;
    public int TempsMinNavigation = 50;
    public int TempsMaxNavigation = 75;
    public float TimerFightCooldown = 5;

    [Space(20)]

    // Random et switch entre cam�ras
    [Header("Temps avant le switch de camera")]
    public int LancementFight;
    public int LancementNavig = 10;
    [Space(20)]

    //Etat de jeu
    [Header("Etat des phases")]
    public bool Navigation = true;
    public static bool Fight = false;
    [Space(20)]

    // Objets � toggle
    [Header("Objets Toggle")]
    public GameObject CameraFight;
    public GameObject CameraNavigation;
    public GameObject UI;
    [Space(20)]

    // Le random
    System.Random rnd = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        LancementFight = rnd.Next(TempsMinNavigation, TempsMaxNavigation); //Randomise automatiquement le premier lancement de combat
    }

    // Update is called once per frame
    void Update()
    {
        if (Navigation == true) 
        {
            TempsNavigation += Time.deltaTime;
            if (Mathf.Abs(TempsNavigation - LancementFight) < 0.1f)
            {
                Navigation = false;
                Fight = true;
                CameraNavigation.SetActive(!CameraNavigation.activeSelf);
                CameraFight.SetActive(!CameraFight.activeSelf);
                UI.SetActive(!UI.activeSelf);

                // R�initialiser TempsNavigation pour arr�ter le timer
                TempsNavigation = 0;

                // Red�finir Lancement pour le prochain combat al�atoire
                LancementFight = rnd.Next(TempsMinNavigation, TempsMaxNavigation);

            }
        }

        if (Fight == true)
        {
            //SceneManager.LoadScene("FightTest");

            TempsFight += Time.deltaTime;
            if (Mathf.Abs(TempsFight - LancementNavig) < 0.1f)
            {
                Fight = false;
                Navigation = true;
                CameraNavigation.SetActive(!CameraNavigation.activeSelf);
               CameraFight.SetActive(!CameraFight.activeSelf);
               UI.SetActive(!UI.activeSelf);

                // R�initialiser TempsFight pour arr�ter le timer
                TempsFight = 0;
            }
        }
    }
}

