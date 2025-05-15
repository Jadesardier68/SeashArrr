using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ordrePanel;
    public GameObject targetPanel;
    public GameObject choicePanel;
    public Slider[] slidersVie;

    public GameObject[] listPictures;

    public Button[] targetButtons; // Filled dynamically

    private int selectedAction = -1;
    private int selectedTarget = -1;

    private bool actionChosen = false;
    private bool targetChosen = false;

    private Player currentPlayer;
    public Battle_Handler battleHandler;

    public Enemy[] enemies;
    public Player[] players;

    public InputAction AttackInput;
    public InputAction HealInput;
    public InputAction CanonInput;
    public InputAction BoatFixInput;

    public StatsManager statsManager;
    public int currentTargetIndex;

    private void Start()
    {
        EnableInputs();
        GameObject playerObject = GameObject.FindGameObjectWithTag("StatsManager");
        GameObject battleManager = GameObject.FindGameObjectWithTag("BattleManager");
        GameObject UIManagerObject = GameObject.FindGameObjectWithTag("BattleManager");

        statsManager = playerObject.GetComponent<StatsManager>();
        battleHandler = battleManager.GetComponent<Battle_Handler>();
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

    public IEnumerator Starter(Player player, Action<int, int> onChoiceComplete)
    {
        ordrePanel.SetActive(true);
        currentPlayer = player;
        selectedAction = -1;
        selectedTarget = -1;
        actionChosen = false;
        targetChosen = false;

        EnableInputs(); // S'assurer que les inputs sont prêts

        yield return new WaitUntil(() => actionChosen && targetChosen);

        ordrePanel.SetActive(false);
        onChoiceComplete?.Invoke(selectedAction, selectedTarget);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        selectedAction = 0;
        selectedTarget = 0; // ou implémenter choix dynamique d’ennemi
        actionChosen = true;
        targetChosen = true;
        Debug.Log("Action: Attaque choisie.");
    }

    private void OnHeal(InputAction.CallbackContext context)
    {
        selectedAction = 1;
        selectedTarget = 0; // ou implémenter choix de l’allié à soigner
        actionChosen = true;
        targetChosen = true;
        Debug.Log("Action: Soin choisie.");
    }

    private void OnCanon(InputAction.CallbackContext context)
    {
        selectedAction = 2;
        selectedTarget = -1; // canon touche tous les ennemis
        actionChosen = true;
        targetChosen = true;
        Debug.Log("Action: Canon choisie.");
    }

    private void OnBoatFix(InputAction.CallbackContext context)
    {
        selectedAction = 3;
        selectedTarget = -1; // réparation du bateau
        actionChosen = true;
        targetChosen = true;
        Debug.Log("Action: Réparation du bateau choisie.");
    }

    void OnDestroy()
    {
        AttackInput.performed -= OnAttack;
        HealInput.performed -= OnHeal;
        CanonInput.performed -= OnCanon;
        BoatFixInput.performed -= OnBoatFix;
    }
}