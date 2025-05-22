using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class liaisonStatsManager : MonoBehaviour
{
    private StatsManager statsManager;
    private UseAtelier useAtelier;
    public AtelierManager atelierManager;

    public InputAction CuisinerRagout;
    public InputAction CuisinerRhum;
    public InputAction Manger;
    public InputAction RepCanon;
    public InputAction AmeBateau;
    public InputAction AmeCanon;

    void Start()
    {
        GameObject statsManagerObject = GameObject.FindGameObjectWithTag("StatsManager");

        if (statsManagerObject != null)
        {
            statsManager = statsManagerObject.GetComponent<StatsManager>();
            useAtelier = statsManagerObject.GetComponent<UseAtelier>();
        }
    }

    private void OnEnable()
    {
        CuisinerRagout.Enable();
        CuisinerRhum.Enable();
        Manger.Enable();
        RepCanon.Enable();
        AmeBateau.Enable();
        AmeCanon.Enable();

        CuisinerRagout.started += TriggerUseAtelierCuisineRagout;
        CuisinerRhum.started += TriggerUseAtelierCuisinerRhum;
        Manger.started += TriggerUseAtelierManger;
        RepCanon.started += TriggerUseAtelierRepCanon;
        AmeBateau.started += TriggerUseAtelierAmeBateau;
        AmeCanon.started += TriggerUseAtelierAmeCanon;
    }

    private void OnDisable()
    {
        CuisinerRagout.started -= TriggerUseAtelierCuisineRagout;
        CuisinerRhum.started -= TriggerUseAtelierCuisinerRhum;
        Manger.started -= TriggerUseAtelierManger;
        RepCanon.started -= TriggerUseAtelierRepCanon;
        AmeBateau.started -= TriggerUseAtelierAmeBateau;
        AmeCanon.started -= TriggerUseAtelierAmeCanon;

        CuisinerRagout.Disable();
        CuisinerRhum.Disable();
        Manger.Disable();
        RepCanon.Disable();
        AmeBateau.Disable();
        AmeCanon.Disable();
    }

    public void TriggerUseAtelierAmeBateau(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelTableIngenieur.activeSelf && useAtelier != null)
        {
            useAtelier.AmeliorationBateau();
        }
    }

    public void TriggerUseAtelierAmeCanon(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelTableIngenieur.activeSelf && useAtelier != null)
        {
            useAtelier.AmeliorationCanon();
        }
    }

    public void TriggerUseAtelierManger(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelPiqueNique.activeSelf && useAtelier != null)
        {
            useAtelier.Manger();
        }
    }

    public void TriggerUseAtelierRepCanon(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCanon.activeSelf && useAtelier != null)
        {
            useAtelier.ReparerCanon();
        }
    }

    public void TriggerUseAtelierCuisineRagout(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCuisine.activeSelf && useAtelier != null)
        {
            useAtelier.CuisinerRagout();
        }
    }

    public void TriggerUseAtelierCuisinerRhum(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCuisine.activeSelf && useAtelier != null)
        {
            useAtelier.CuisinerRhum();
        }
    }
}