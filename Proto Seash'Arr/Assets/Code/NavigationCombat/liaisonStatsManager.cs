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

    public Anim_Nav animationPlayer;

    void Start()
    {
        if (animationPlayer == null)
        {
            animationPlayer = FindObjectOfType<Anim_Nav>(); // ⚠️ Attention : prend le **premier** trouvé
            if (animationPlayer == null)
            {
                Debug.LogError("Anim_Nav not found in scene! LiaisonStatsManager needs it.");
            }
        }

        GameObject statsManagerObject = GameObject.FindGameObjectWithTag("StatsManager");
        if (statsManagerObject != null)
        {
            statsManager = statsManagerObject.GetComponent<StatsManager>();
            useAtelier = statsManagerObject.GetComponent<UseAtelier>();
        }
        else
        {
            Debug.LogWarning("StatsManager not found with tag.");
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
            animationPlayer.tableIngenieurActive = true;
            animationPlayer.Ingenieur();
            useAtelier.AmeliorationBateau(atelierManager); // ✅ paramètre ajouté
            animationPlayer.StopMoving();
        }
    }

    public void TriggerUseAtelierAmeCanon(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelTableIngenieur.activeSelf && useAtelier != null)
        {
            animationPlayer.tableIngenieurActive = true;
            animationPlayer.Ingenieur();
            useAtelier.AmeliorationCanon(atelierManager); // ✅
            animationPlayer.StopMoving();
        }
    }

    public void TriggerUseAtelierManger(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelPiqueNique.activeSelf && useAtelier != null)
        {
            animationPlayer.piqueNiqueActive = true;
            animationPlayer.Manger();
            useAtelier.Manger(atelierManager); // ✅
            animationPlayer.StopMoving();
        }
    }

    public void TriggerUseAtelierRepCanon(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCanon.activeSelf && useAtelier != null)
        {
            animationPlayer.canonActive = true;
            animationPlayer.Canon();
            useAtelier.ReparerCanon(atelierManager); // ✅
            animationPlayer.StopMoving();
        }
    }

    public void TriggerUseAtelierCuisineRagout(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCuisine.activeSelf && useAtelier != null)
        {
            animationPlayer.cuisineActive = true;
            animationPlayer.Cuisiner();
            useAtelier.CuisinerRagout(atelierManager); // ✅
            animationPlayer.StopMoving();
        }
    }

    public void TriggerUseAtelierCuisinerRhum(InputAction.CallbackContext context)
    {
        if (atelierManager.PanelCuisine.activeSelf && useAtelier != null)
        {
            animationPlayer.cuisineActive = true;
            animationPlayer.Cuisiner();
            useAtelier.CuisinerRhum(atelierManager); // ✅
            animationPlayer.StopMoving();
        }
    }
}