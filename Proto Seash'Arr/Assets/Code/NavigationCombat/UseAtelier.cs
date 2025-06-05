using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAtelier : MonoBehaviour
{
    public StatsManager StatsManager;

    [SerializeField] private float refreshRate = 2f;

    private List<AtelierManager> ateliers = new List<AtelierManager>();
    private Dictionary<AtelierManager, CoroutineData> coroutineMap = new();
    private Dictionary<AtelierManager, Coroutine> activeCoroutines = new();
    private Dictionary<AtelierManager, bool> atelierBusyMap = new();

    public enum AtelierType
    {
        Cuisine,
        Ingenieur,
        Canon,
        PiqueNique
    }

    private class CoroutineData
    {
        public Coroutine Coroutine;
        public float TempsAction;
        public float Timer;
    }

    private void Start()
    {
        StartCoroutine(RefreshAtelierList());
    }

    private IEnumerator RefreshAtelierList()
    {
        while (true)
        {
            ateliers = new List<AtelierManager>(FindObjectsOfType<AtelierManager>());
            foreach (var atelier in ateliers)
            {
                if (!atelierBusyMap.ContainsKey(atelier))
                    atelierBusyMap[atelier] = false;
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }

    private bool IsAtelierBusy(AtelierManager atelier)
    {
        return atelierBusyMap.ContainsKey(atelier) && atelierBusyMap[atelier];
    }

    private void SetAtelierBusy(AtelierManager atelier, bool state)
    {
        if (atelierBusyMap.ContainsKey(atelier))
            atelierBusyMap[atelier] = state;
    }

    private void TryAction(string actionName, float tempsAction, System.Action<AtelierManager> action, System.Action onActionCompleted, AtelierManager atelier)
    {
        if (atelier == null)
        {
            Debug.LogWarning($"Aucun atelier fourni pour {actionName}.");
            return;
        }

        if (!IsAtelierBusy(atelier))
        {
            Coroutine routine = StartCoroutine(ActionRoutine(actionName, tempsAction, action, onActionCompleted, atelier));
            coroutineMap[atelier] = new CoroutineData
            {
                Coroutine = routine,
                TempsAction = tempsAction,
                Timer = 0
            };
        }
        else
        {
            Debug.Log($"Action bloquée : Atelier déjà occupé.");
        }
    }

    private IEnumerator ActionRoutine(string actionName, float tempsAction, System.Action<AtelierManager> action, System.Action onActionCompleted, AtelierManager atelier)
    {
        SetAtelierBusy(atelier, true);
        Debug.Log("Début de l'action : " + actionName);

        atelier.HideAllSliders();
        Slider slider = atelier.GetSliderForTypeByPanel();

        if (slider != null)
        {
            slider.gameObject.SetActive(true);
            slider.maxValue = tempsAction;
            slider.value = 0;
        }

        action?.Invoke(atelier);

        float timer = 0f;
        while (timer < tempsAction)
        {
            timer += Time.deltaTime;
            if (slider != null)
                slider.value = timer;

            if (coroutineMap.ContainsKey(atelier))
                coroutineMap[atelier].Timer = timer;

            yield return null;
        }

        if (slider != null)
            slider.gameObject.SetActive(false);

        onActionCompleted?.Invoke();
        SetAtelierBusy(atelier, false);
        coroutineMap.Remove(atelier);
        activeCoroutines.Remove(atelier);

        Debug.Log("Fin de l'action : " + actionName);
    }

    // ---------- ACTIONS ----------

    public void AmeliorationBateau(AtelierManager atelier)
    {
        TryAction("Amélioration Bateau", 20f, (a) =>
        {
            if (StatsManager.nbrWood >= 100 && StatsManager.nbrIron >= 50)
            {
                StatsManager.nbrWood -= 100;
                StatsManager.nbrIron -= 50;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.boatMaxHealth += 100;
            StatsManager.UpdateText();
        }, atelier);
    }

    public void AmeliorationCanon(AtelierManager atelier)
    {
        TryAction("Amélioration Canon", 16f, (a) =>
        {
            if (StatsManager.nbrWood >= 70 && StatsManager.nbrIron >= 30)
            {
                StatsManager.nbrWood -= 70;
                StatsManager.nbrIron -= 30;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.canonMaxHealth += 100;
            StatsManager.UpdateText();
        }, atelier);
    }

    public void ReparerCanon(AtelierManager atelier)
    {
        TryAction("Réparation Canon", 10f, (a) =>
        {
            if (StatsManager.nbrWood >= 20)
            {
                StatsManager.nbrWood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.canonHealth = Mathf.Min(
                StatsManager.canonHealth + (int)(0.2f * StatsManager.canonMaxHealth),
                StatsManager.canonMaxHealth
            );
            StatsManager.UpdateText();
        }, atelier);
    }

    public void ReparerBateau(AtelierManager atelier)
    {
        TryAction("Réparation Bateau", 10f, (a) =>
        {
            if (StatsManager.nbrIron >= 20)
            {
                StatsManager.nbrIron -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.boatHealth = Mathf.Min(
                StatsManager.boatHealth + (int)(0.2f * StatsManager.boatMaxHealth),
                StatsManager.boatMaxHealth
            );
            StatsManager.UpdateText();
        }, atelier);
    }

    public void CuisinerRhum(AtelierManager atelier)
    {
        TryAction("Cuisiner Rhum", 10f, (a) =>
        {
            if (StatsManager.nbrFood >= 20)
            {
                StatsManager.nbrFood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.nbrRhum += 1;
            StatsManager.UpdateText();
        }, atelier);
    }

    public void CuisinerRagout(AtelierManager atelier)
    {
        TryAction("Cuisiner Ragoût", 10f, (a) =>
        {
            if (StatsManager.nbrFood >= 20)
            {
                StatsManager.nbrFood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.nbrRagout += 1;
            StatsManager.UpdateText();
        }, atelier);
    }

    public void Manger(AtelierManager atelier)
    {
        TryAction("Manger", 5f, (a) =>
        {
            if (StatsManager.nbrRagout > 0)
            {
                StatsManager.nbrRagout -= 1;
                StatsManager.UpdateText();
            }
        },
        null, atelier);
    }

    // ---------- ANNULATIONS ----------

    public void AnnulerAction(AtelierManager atelier)
    {
        if (IsAtelierBusy(atelier))
        {
            if (coroutineMap.ContainsKey(atelier))
            {
                StopCoroutine(coroutineMap[atelier].Coroutine);
                coroutineMap.Remove(atelier);
            }

            atelier.HideAllSliders();
            SetAtelierBusy(atelier, false);

            Debug.Log("Action annulée sur atelier.");
        }
    }

    public void AnnulerToutesActions()
    {
        foreach (var atelier in new List<AtelierManager>(coroutineMap.Keys))
        {
            AnnulerAction(atelier);
        }
    }
}
