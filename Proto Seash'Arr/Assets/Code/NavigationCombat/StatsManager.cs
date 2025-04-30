﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StatsManager : MonoBehaviour
{
    [Header("Temps globaux")]
    public float TempsNavigation = 0;
    public float TempsCombat = 0;
    public float TempsFight = 0;
    public int TempsMinBeforeFight = 50;
    public int TempsMaxBeforeFight = 75;
    public int TempsBeforeIsland = 90;
    public float TimerFightCooldown = 5;
    public float TimerRessourcesCooldown = 5;

    [Header("Temps avant le switch de camera")]
    public int LancementFight;
    public int LancementNavig = 10;

    [Header("Etat des phases")]
    public bool Carte = true;
    public bool Navigation = false;
    public bool Fight = false;
    public bool Ressources = false;

    [Header("Objets Toggle")]
    public GameObject CameraFight;
    public GameObject CameraNavigation;
    public GameObject UIPopUpEnnemies;
    public GameObject UIPopUpRessources;
    public GameObject carte;
    public GameObject UI;
    public Slider slider;

    [Header("Statistiques")]
    public int boatHealth;
    public int canonHealth;
    public int nbrWood;
    public int nbrIron;
    public int nbrFood;
    public int nbrRhum;
    public int nbrRagout;
    public int boatMaxHealth = 300;
    public int canonMaxHealth = 100;

    [Header("Texts")]
    public TMP_Text boatHealthText;
    public TMP_Text canonHealthText;
    public TMP_Text nbrWoodText;
    public TMP_Text nbrIronText;
    public TMP_Text nbrFoodText;
    public TMP_Text nbrRhumText;
    public TMP_Text nbrRagoutText;

    [Header("Iles")]
    public Button Boat;
    public Button Calmar;
    public Button Espidoche;
    public Button Scylla;
    public Button Sil;
    public Button Ahuizotl;

    private List<AtelierManager> ateliers = new List<AtelierManager>();
    public UseAtelier useAtelier;
    private float refreshRate = 2f;
    private System.Random rnd = new System.Random();
    private string currentIsland = null; // Mémorise la dernière île choisie

    private Dictionary<(string, string), int> islandTravelTimes = new Dictionary<(string, string), int>()
{
    { ("Calmar", "Espidoche"), 120 },
    { ("Calmar", "Scylla"), 180 },
    { ("Calmar", "Sil"), 180 },
    { ("Calmar", "Ahuizotl"), 120 },

    { ("Espidoche", "Calmar"), 120 },
    { ("Espidoche", "Scylla"), 120 },
    { ("Espidoche", "Sil"), 240 },
    { ("Espidoche", "Ahuizotl"), 180 },

    { ("Scylla", "Calmar"), 180 },
    { ("Scylla", "Espidoche"), 120 },
    { ("Scylla", "Sil"), 120 },
    { ("Scylla", "Ahuizotl"), 180 },

    { ("Sil", "Calmar"), 180 },
    { ("Sil", "Espidoche"), 240 },
    { ("Sil", "Scylla"), 120 },
    { ("Sil", "Ahuizotl"), 120 },

    { ("Ahuizotl", "Calmar"), 120 },
    { ("Ahuizotl", "Espidoche"), 180 },
    { ("Ahuizotl", "Scylla"), 180 },
    { ("Ahuizotl", "Sil"), 120 },
};

    private Dictionary<string, (int wood, int iron, int food)> islandResources = new Dictionary<string, (int, int, int)>()
{
    { "Calmar", (60, 20, 80) },
    { "Espidoche", (130, 10, 70) },
    { "Scylla", (30, 50, 90) },
    { "Sil", (90, 30, 60) },
    { "Ahuizotl", (70, 20, 120) }
};

    void Start()
    {
        carte.SetActive(true);
        UI.SetActive(false);
        LancementFight = rnd.Next(TempsMinBeforeFight, TempsMaxBeforeFight);
        slider.maxValue = TempsBeforeIsland;
        slider.value = 0;
        boatHealth = boatMaxHealth;
        canonHealth = canonMaxHealth;
        UpdateText();
        StartCoroutine(RefreshAtelierList());
    }

    private IEnumerator RefreshAtelierList()
    {
        while (true)
        {
            ateliers = new List<AtelierManager>(FindObjectsOfType<AtelierManager>());
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void UpdateText()
    {
        boatHealthText.text = boatHealth + " / " + boatMaxHealth;
        canonHealthText.text = canonHealth + " / " + canonMaxHealth;
        nbrWoodText.text = nbrWood + "";
        nbrIronText.text = nbrIron + "";
        nbrFoodText.text = nbrFood + "";
        nbrRhumText.text = nbrRhum + "";
        nbrRagoutText.text = nbrRagout + "";
    }

    void Update()
    {
        if (Carte == true) return;

        if (Navigation)
        {
            TempsNavigation += Time.deltaTime;
            TempsCombat += Time.deltaTime;
            slider.value = TempsNavigation;

            if (TempsCombat >= LancementFight - TimerFightCooldown)
                UIPopUpEnnemies.SetActive(true);

            if (Mathf.Abs(TempsCombat - LancementFight) < 0.1f)
            {
                Navigation = false;
                Fight = true;
                CameraNavigation.SetActive(false);
                CameraFight.SetActive(true);
                slider.value = LancementFight;
                TempsNavigation = LancementFight;
                UIPopUpEnnemies.SetActive(false);
                LancementFight = rnd.Next(TempsMinBeforeFight, TempsMaxBeforeFight);
                TempsCombat = 0;
            }

            if (TempsNavigation >= TempsBeforeIsland - TimerRessourcesCooldown)
                UIPopUpRessources.SetActive(true);

            if (Mathf.Abs(TempsNavigation - TempsBeforeIsland) < 0.1f || TempsNavigation > TempsBeforeIsland)
            {
                int ancreActives = 0;
                foreach (AtelierManager atelier in ateliers)
                {
                    if (atelier.PanelAncre != null && atelier.PanelAncre.activeInHierarchy)
                        ancreActives++;
                }

                if (ancreActives == 2)
                {
                    useAtelier.AnnulerAction();
                    Navigation = false;
                    Ressources = true;
                    UIPopUpRessources.SetActive(false);
                }
            }
        }

        if (Fight)
        {
            TempsFight += Time.deltaTime;
            if (Mathf.Abs(TempsFight - LancementNavig) < 0.1f)
            {
                Fight = false;
                Navigation = true;
                CameraFight.SetActive(false);
                CameraNavigation.SetActive(true);
                TempsFight = 0;
            }
        }

        if (Ressources)
        {
            TempsNavigation = 0;
            TempsCombat += Time.deltaTime;

            // Obtention des ressources en fonction de l'île
            if (islandResources.ContainsKey(currentIsland))
            {
                var resources = islandResources[currentIsland];
                nbrWood += resources.wood;
                nbrIron += resources.iron;
                nbrFood += resources.food;

                UpdateText(); // Met à jour l'UI avec les nouvelles ressources
            }

            Ressources = false;

            // Retour à la carte pour rechoisir une île
            Carte = true;
            Navigation = false;
            carte.SetActive(true);
            UI.SetActive(false); // UI de navigation si désactivée pendant le choix

            // Réinitialiser l'input système
            ResetInputSystem();
        }
    }

    // === Méthodes pour chaque île ===
    public void ChoisirCalmar() => InitierVoyage("Calmar", Calmar);
    public void ChoisirEspidoche() => InitierVoyage("Espidoche", Espidoche);
    public void ChoisirScylla() => InitierVoyage("Scylla", Scylla);
    public void ChoisirSil() => InitierVoyage("Sil", Sil);
    public void ChoisirAhuizotl() => InitierVoyage("Ahuizotl", Ahuizotl);

     

    private void InitierVoyage(string nomIle, Button boutonIle)
    {
        if (!string.IsNullOrEmpty(currentIsland))
        {
            if (islandTravelTimes.TryGetValue((currentIsland, nomIle), out int travelTime))
            {
                TempsBeforeIsland = travelTime;
            }
            else
            {
                Debug.LogWarning("Pas de durée définie pour ce trajet : " + currentIsland + " → " + nomIle);
                TempsBeforeIsland = 90; // par défaut
            }
        }
        else
            {
                switch (nomIle)
                {
                    case "Calmar": TempsBeforeIsland = 60; break;
                    case "Espidoche": TempsBeforeIsland = 120; break;
                    case "Scylla": TempsBeforeIsland = 120; break;
                    case "Sil": TempsBeforeIsland = 120; break;
                    case "Ahuizotl": TempsBeforeIsland = 60; break;
                    default:
                        Debug.LogWarning("Ile inconnue : " + nomIle);
                        return;
                }
            }

        currentIsland = nomIle;

        // Positionne le bouton Boat sans animation
        RectTransform boatRect = Boat.GetComponent<RectTransform>();
        RectTransform ileRect = boutonIle.GetComponent<RectTransform>();
        boatRect.anchoredPosition = ileRect.anchoredPosition;

        carte.SetActive(false);
        UI.SetActive(true);
        slider.maxValue = TempsBeforeIsland;
        slider.value = 0;
        Carte = false;
        Navigation = true;
    }

    void ResetInputSystem()
    {
        // Réactive les événements de souris et de manette
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Boat.gameObject); // ou n'importe quel autre bouton actif
    }
}
