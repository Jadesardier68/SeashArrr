using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject ordrePanel;
    public GameObject actionPanel;
    public GameObject targetPanel;

    [Header("Target Buttons")]
    public Button[] targetButtons;

    [Header("Input System Actions")]
    public InputAction attackInput;
    public InputAction healInput;
    public InputAction canonInput;
    public InputAction fixBoatInput;

    [Header("References")]
    public StatsManager statsManager;
    public Battle_Handler battleHandler;

    private Player currentPlayer;

    private int selectedAction = -1;
    private int selectedTarget = -1;
    private bool actionSelected = false;
    private bool targetSelected = false;

    private void Awake()
    {
        // Assigner les références dynamiquement si besoin
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

        attackInput.performed += ctx => SelectAction(0); // Attack
        healInput.performed += ctx => SelectAction(1);   // Heal
        canonInput.performed += ctx => SelectAction(2);  // Canon
        fixBoatInput.performed += ctx => SelectAction(3); // Fix
    }

    private void DisableInputs()
    {
        attackInput.Disable();
        healInput.Disable();
        canonInput.Disable();
        fixBoatInput.Disable();
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
        Debug.Log("tkt je me lance");
        currentPlayer = player;
        selectedAction = -1;
        selectedTarget = -1;
        actionSelected = false;
        targetSelected = false;

        actionPanel.SetActive(true);
        Debug.Log("j'ai affiché le panel");
        targetPanel.SetActive(false);

        yield return new WaitUntil(() => actionSelected);

        actionPanel.SetActive(false);

        // Si l’action nécessite un ciblage
        if (selectedAction == 0 || selectedAction == 1) // Attack or Heal
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
        Debug.Log("allez go à moi");
        List<GameObject> targets = selectedAction == 1 ? battleHandler.Players : battleHandler.Ennemies;

        targetSelected = false;
        selectedTarget = -1;

        // Afficher les bons boutons
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (i < targets.Count)
            {
                int index = i;
                GameObject target = targets[i];
                Button btn = targetButtons[i];

                if (btn == null)
                {
                    Debug.LogError($"Le bouton à l’index {i} est null.");
                    continue;
                }

                if (target == null)
                {
                    Debug.LogError($"La cible à l’index {i} est null.");
                    continue;
                }

                TMP_Text textComponent = btn.GetComponentInChildren<TMP_Text>();
                if (textComponent == null)
                {
                    Debug.LogError($"Le bouton à l’index {i} n’a pas de composant Text.");
                    continue;
                }

                btn.gameObject.SetActive(true);
                textComponent.text = target.name;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnTargetSelected(index));
            }
            else
            {
                if (targetButtons[i] != null)
                    targetButtons[i].gameObject.SetActive(false);
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
}