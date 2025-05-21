using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject ordrePanel;
    public GameObject targetPanel;
    public GameObject choicePanel;

    [Header("Buttons")]
    public Button[] targetButtons;

    [Header("Inputs")]
    public InputAction AttackInput;
    public InputAction HealInput;
    public InputAction CanonInput;
    public InputAction BoatFixInput;

    [Header("References")]
    public StatsManager statsManager;
    public Battle_Handler battleHandler;

    [Header("State")]
    private Player currentPlayer;
    public int selectedAction = -1;
    public int selectedTarget = -1;
    public bool actionChosen = false;
    public bool targetChosen = false;
    public int currentTargetIndex;

    public List<GameObject> Enemies => battleHandler != null ? battleHandler.Ennemies : new List<GameObject>();
    public List<GameObject> Players => battleHandler != null ? battleHandler.Players : new List<GameObject>();

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("StatsManager");
        GameObject battleManager = GameObject.FindGameObjectWithTag("BattleManager");

        if (playerObject != null)
            statsManager = playerObject.GetComponent<StatsManager>();

        if (battleManager != null)
            battleHandler = battleManager.GetComponent<Battle_Handler>();

        EnableInputs();
    }

    private void EnableInputs()
    {
        AttackInput.Enable();
        AttackInput.performed += OnAttack;

        HealInput.Enable();
        HealInput.performed += OnHeal;

        CanonInput.Enable();
        CanonInput.performed += OnCanon;

        BoatFixInput.Enable();
        BoatFixInput.performed += OnBoatFix;
    }

    private void DisableInputs()
    {
        AttackInput.Disable();
        HealInput.Disable();
        CanonInput.Disable();
        BoatFixInput.Disable();
    }

    private bool CanPlayerAct()
    {
        return battleHandler != null
            && !battleHandler.isTurnOver
            && battleHandler.currentUnit != null
            && battleHandler.currentUnit.CompareTag("Player")
            && statsManager != null
            && statsManager.inputActionMap.enabled;
    }

    public IEnumerator Starter(Player player, Action<int, int> onChoiceComplete)
    {
        if (!CanPlayerAct())
        {
            Debug.LogWarning("Player can't act: not their turn or input map disabled.");
            yield break;
        }

        currentPlayer = player;
        selectedAction = -1;
        selectedTarget = -1;
        actionChosen = false;
        targetChosen = false;

        EnableInputs();

        // Attendre le choix d'action
        yield return new WaitUntil(() => actionChosen);

        // Si pas besoin de cible (canon ou réparation), on considère la cible choisie
        if (selectedAction == 2 || selectedAction == 3)
        {
            targetChosen = true;
        }
        else
        {
            // Sinon activer panel cible et attendre choix
            choicePanel.SetActive(false);
            targetPanel.SetActive(true);
            yield return StartCoroutine(HandleTargetSelection());
        }

        // Désactiver les panels seulement après que cible soit choisie
        

        DisableInputs();

        onChoiceComplete?.Invoke(selectedAction, selectedTarget);
        battleHandler.isTurnOver = true;
    }
    private IEnumerator HandleTargetSelection()
    {
        targetPanel.SetActive(true);

        // Choix des cibles en fonction de l'action
        List<GameObject> targetList = selectedAction == 1 ? Players : Enemies;

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (i < targetList.Count)
            {
                int index = i;
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].GetComponentInChildren<Text>().text = targetList[i].name;
                targetButtons[i].onClick.RemoveAllListeners();
                targetButtons[i].onClick.AddListener(() => SelectTarget(index));
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }

        yield return new WaitUntil(() => targetChosen);

        targetPanel.SetActive(false);
    }

    private void SelectTarget(int index)
    {
        selectedTarget = index;
        targetChosen = true;
        Debug.Log("Target sélectionnée : " + index);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!CanPlayerAct()) return;

        selectedAction = 0; // Attaque
        actionChosen = true;
        Debug.Log("Action: Attaque choisie.");
    }

    private void OnHeal(InputAction.CallbackContext context)
    {
        if (!CanPlayerAct()) return;

        selectedAction = 1; // Soin
        actionChosen = true;
        Debug.Log("Action: Soin choisie.");
    }

    private void OnCanon(InputAction.CallbackContext context)
    {
        if (!CanPlayerAct()) return;

        selectedAction = 2; // Canon (aucune cible directe)
        selectedTarget = -1;
        actionChosen = true;
        Debug.Log("Action: Canon choisie.");
    }

    private void OnBoatFix(InputAction.CallbackContext context)
    {
        if (!CanPlayerAct()) return;

        selectedAction = 3; // Réparation bateau
        selectedTarget = -1;
        actionChosen = true;
        Debug.Log("Action: Réparation du bateau choisie.");
    }
}