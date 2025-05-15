using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    public void Start()
    {
        
    }
    public IEnumerator Starter(Player player, Action<int, int> onChoiceComplete)
    {
        ordrePanel.SetActive(true);
        currentPlayer = player;
        selectedAction = -1;
        selectedTarget = -1;
        actionChosen = false;
        targetChosen = false;

        yield return StartCoroutine(ActionSelection());

        // Appeler le callback avec le choix final
        onChoiceComplete?.Invoke(selectedAction, selectedTarget);
    }

    private IEnumerator ActionSelection()
    {
        ordrePanel.SetActive(true);

        choicePanel.SetActive(true);
        //targetPanel.SetActive(false);

       // listPictures.AddRange(battleHandler.turnOrder);
    
        // Wait for action to be selected
        yield return new WaitUntil(() => actionChosen);

        // Now choose target
        yield return StartCoroutine(TargetSelection());

        yield return null;
    }

    private void SelectAction(int index)
    {
        selectedAction = index;
        actionChosen = true;
        //actionPanel.SetActive(false);
    }

    private IEnumerator TargetSelection()
    {
        targetPanel.SetActive(true);
        targetChosen = false;

        // Clear old buttons
        foreach (Transform child in targetPanel.transform)
            Destroy(child.gameObject);


        // Create buttons based on selectedAction
        List<string> targets = new List<string>();

        switch (selectedAction)
        {
            case 0: // Attack
            case 1: // Canon
                for (int i = 0; i < Fight.Ennemies.Count; i++)
                    targets.Add("Enemy " + i);
                break;

            case 2: // Fix
                targets.Add("Boat");
                targets.Add("Canon");
                break;

            case 3: // Heal
            case 4: // Boost
                for (int i = 0; i < Fight.Players.Count; i++)
                    targets.Add("Player " + i);
                break;
        }

        for (int i = 0; i < targets.Count; i++)
        {
            int index = i;
            GameObject btnObj = new GameObject("TargetBtn" + i);
            btnObj.transform.SetParent(targetPanel.transform);
            Button btn = btnObj.AddComponent<Button>();
            TMP_Text txt = btnObj.AddComponent<TMP_Text>();
            txt.text = targets[i];
            txt.fontSize = 24;
            btn.onClick.AddListener(() => SelectTarget(index));
        }

        yield return new WaitUntil(() => targetChosen);
        targetPanel.SetActive(false);
    }

    private void SelectTarget(int index)
    {
        selectedTarget = index;
        targetChosen = true;
    }

    public static void UiUpdateHealthBar()
    {
        foreach (Player player in Fight.Players)
        {
            Slider slider = player.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                float ratio = (float)player.GetHP() / player.GetHPMax();
                slider.value = ratio;
            }
        }

        foreach (Ennemy enemy in Fight.Ennemies)
        {
            Slider slider = enemy.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                float ratio = (float)enemy.GetHP() / enemy.GetHPMax();
                slider.value = ratio;
            }
        }
    }
}