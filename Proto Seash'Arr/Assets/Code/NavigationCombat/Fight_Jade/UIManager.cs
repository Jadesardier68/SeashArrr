using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panneaux UI")]
    public GameObject actionPanel;
    public GameObject targetPanel;

    [Header("Boutons de cible")]
    public Button[] targetButtons;

    [Header("Input Actions")]
    public InputAction attackInput;
    public InputAction healInput;
    public InputAction canonInput;
    public InputAction fixBoatInput;

    [Header("Références")]
    public StatsManager statsManager;
    public Battle_Handler battleHandler;

    private Player currentPlayer;

    private int selectedAction = -1;
    private int selectedTarget = -1;
    private bool actionSelected = false;
    private bool targetSelected = false;

    [Header("Sliders de Vie")]
    public Slider[] healthSliders;

    private void Awake()
    {
        // Initialisation dynamique si nécessaire
        if (statsManager == null)
            statsManager = FindObjectOfType<StatsManager>();

        if (battleHandler == null)
            battleHandler = FindObjectOfType<Battle_Handler>();
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    private void EnableInputs()
    {
        attackInput.Enable();
        healInput.Enable();
        canonInput.Enable();
        fixBoatInput.Enable();

        attackInput.performed += ctx => SelectAction(0); // Attaque
        healInput.performed += ctx => SelectAction(1);   // Soin
        canonInput.performed += ctx => SelectAction(2);  // Canon
        fixBoatInput.performed += ctx => SelectAction(3); // Réparer
    }

    private void DisableInputs()
    {
        attackInput.Disable();
        healInput.Disable();
        canonInput.Disable();
        fixBoatInput.Disable();
    }

    public void InitializeHealthSliders()
    {
        for (int i = 0; i < healthSliders.Length; i++)
        {
            if (i < battleHandler.Players.Count && healthSliders[i] != null)
            {
                Player player = battleHandler.Players[i].GetComponent<Player>();
                healthSliders[i].gameObject.SetActive(true);
                healthSliders[i].maxValue = player.HPMax;
                healthSliders[i].value = player.HP;
            }
            else if (healthSliders[i] != null)
            {
                healthSliders[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealthSlider(int playerIndex, int currentHP)
    {
        if (playerIndex >= 0 && playerIndex < healthSliders.Length && healthSliders[playerIndex] != null)
        {
            healthSliders[playerIndex].value = currentHP;
        }
    }

    private bool CanPlayerAct()
    {
        return battleHandler != null &&
               battleHandler.currentUnit != null &&
               battleHandler.currentUnit.CompareTag("Player") &&
               !battleHandler.isTurnOver;
    }

    public IEnumerator StartPlayerTurn(Player player, Action<int, int> onTurnComplete)
    {
        currentPlayer = player;
        selectedAction = -1;
        selectedTarget = -1;
        actionSelected = false;
        targetSelected = false;

        actionPanel.SetActive(true);
        targetPanel.SetActive(false);

        yield return new WaitUntil(() => actionSelected);
        actionPanel.SetActive(false);

        // Actions nécessitant un ciblage
        if (selectedAction == 0 || selectedAction == 1) // Attaque ou Soin
        {
            yield return StartCoroutine(HandleTargetSelection());
        }

        onTurnComplete?.Invoke(selectedAction, selectedTarget);
    }

    private void SelectAction(int action)
    {
        if (!CanPlayerAct()) return;

        selectedAction = action;
        actionSelected = true;

        Debug.Log($"Action sélectionnée : {action}");
    }

    private IEnumerator HandleTargetSelection()
    {
        List<GameObject> targets = selectedAction == 1 ? battleHandler.Players : battleHandler.Ennemies;

        targetSelected = false;
        selectedTarget = -1;

        string[] fixedEnemyNames = { "Blobfish", "Méduse", "Calamar" };

        for (int i = 0; i < targetButtons.Length; i++)
        {
            Button btn = targetButtons[i];

            if (selectedAction == 1) // Heal (sur les joueurs)
            {
                if (i < battleHandler.Players.Count && btn != null && battleHandler.Players[i] != null)
                {
                    GameObject target = battleHandler.Players[i];
                    TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                    if (label != null)
                        label.text = GetPortraitName(target);

                    btn.gameObject.SetActive(true);
                    int index = i;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnTargetSelected(index));
                }
                else
                {
                    if (btn != null) btn.gameObject.SetActive(false);
                }
            }
            else if (selectedAction == 0) // Attack (sur ennemis)
            {
                if (i < fixedEnemyNames.Length && btn != null)
                {
                    string requiredName = fixedEnemyNames[i];
                    GameObject enemy = battleHandler.Ennemies.Find(e =>
                    {
                        Enemy comp = e.GetComponent<Enemy>();
                        return comp != null && comp.portraitSpriteName == requiredName;
                    });

                    if (enemy != null)
                    {
                        TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                        if (label != null)
                            label.text = requiredName;

                        btn.gameObject.SetActive(true);
                        int index = battleHandler.Ennemies.IndexOf(enemy);
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => OnTargetSelected(index));
                    }
                    else
                    {
                        btn.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (btn != null) btn.gameObject.SetActive(false);
                }
            }
            else
            {
                if (btn != null) btn.gameObject.SetActive(false);
            }
        }

        targetPanel.SetActive(true);
        yield return new WaitUntil(() => targetSelected);
        targetPanel.SetActive(false);
    }

    private void OnTargetSelected(int index)
    {
        selectedTarget = index;
        targetSelected = true;
        Debug.Log($"Cible sélectionnée : {index}");
    }

    /// <summary>
    /// Retourne le nom d'affichage du portrait selon le type d'unité.
    /// </summary>
    private string GetPortraitName(GameObject unit)
    {
        if (unit.CompareTag("Player"))
        {
            Player p = unit.GetComponent<Player>();
            return p != null ? p.portraitSpriteName : "???";
        }
        else if (unit.CompareTag("Enemy"))
        {
            Enemy e = unit.GetComponent<Enemy>();
            return e != null ? e.portraitSpriteName : "???";
        }

        return "???";
    }


}

