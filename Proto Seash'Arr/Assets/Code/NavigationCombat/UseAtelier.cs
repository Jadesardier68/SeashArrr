using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAtelier : MonoBehaviour
{
    public StatsManager StatsManager;
    private List<AtelierManager> ateliers = new List<AtelierManager>();
    private Dictionary<AtelierType, CoroutineData> coroutineMap = new();
    private AtelierManager currentAtelier;

    [SerializeField] private float refreshRate = 2f;

    // Occupation par type d'atelier
    private Dictionary<AtelierType, bool> atelierBusyMap = new Dictionary<AtelierType, bool>();
    private Dictionary<AtelierType, Coroutine> activeCoroutines = new Dictionary<AtelierType, Coroutine>();
    private Dictionary<AtelierType, AtelierManager> activeAteliers = new Dictionary<AtelierType, AtelierManager>();
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

        // Initialisation de l'état des ateliers
        foreach (AtelierType type in System.Enum.GetValues(typeof(AtelierType)))
        {
            atelierBusyMap[type] = false;
        }
    }

    private IEnumerator RefreshAtelierList()
    {
        while (true)
        {
            ateliers = new List<AtelierManager>(FindObjectsOfType<AtelierManager>());
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private AtelierManager GetActiveAtelier()
    {
        foreach (var atelier in ateliers)
        {
            if (atelier.PanelCuisine.activeSelf || atelier.PanelTableIngenieur.activeSelf ||
                atelier.PanelCanon.activeSelf || atelier.PanelPiqueNique.activeSelf)
            {
                return atelier;
            }
        }
        return null;
    }

    private bool IsAtelierBusy(AtelierType type)
    {
        return atelierBusyMap.ContainsKey(type) && atelierBusyMap[type];
    }

    private void SetAtelierBusy(AtelierType type, bool state)
    {
        if (atelierBusyMap.ContainsKey(type))
            atelierBusyMap[type] = state;
    }

    private void TryAction(string actionName, AtelierType type, float tempsAction, System.Action<AtelierManager> action, System.Action onActionCompleted = null)
    {
        if (!IsAtelierBusy(type))
        {
            Coroutine routine = StartCoroutine(ActionRoutine(actionName, type, tempsAction, action, onActionCompleted));
            coroutineMap[type] = new CoroutineData
            {
                Coroutine = routine,
                TempsAction = tempsAction,
                Timer = 0
            };
        }
        else
        {
            Debug.Log($"Action bloquée : {type} déjà occupé.");
        }
    }

    private IEnumerator ActionRoutine(string actionName, AtelierType type, float tempsAction, System.Action<AtelierManager> action, System.Action onActionCompleted)
    {
        SetAtelierBusy(type, true);
        Debug.Log("Début de l'action : " + actionName);

        AtelierManager activeAtelier = GetActiveAtelier();
        activeAteliers[type] = activeAtelier;

        Slider activeSlider = null;

        if (activeAtelier != null)
        {
            activeAtelier.HideAllSliders();
            activeSlider = activeAtelier.GetSliderForType(type);

            if (activeSlider != null)
            {
                activeSlider.gameObject.SetActive(true);
                activeSlider.maxValue = tempsAction;
                activeSlider.value = 0;
            }
        }

        action?.Invoke(activeAtelier);

        float timer = 0f;
        while (timer < tempsAction)
        {
            timer += Time.deltaTime;
            if (activeSlider != null)
                activeSlider.value = timer;

            coroutineMap[type].Timer = timer;
            yield return null;
        }

        if (activeSlider != null)
            activeSlider.gameObject.SetActive(false);

        onActionCompleted?.Invoke();

        SetAtelierBusy(type, false);
        activeCoroutines.Remove(type);
        activeAteliers.Remove(type);
        coroutineMap.Remove(type);

        Debug.Log("Fin de l'action : " + actionName);
    }
    public void AmeliorationBateau()
    {
        TryAction("Amélioration Bateau", AtelierType.Ingenieur, 20f, (atelier) =>
        {
            if (atelier != null && atelier.PanelTableIngenieur.activeSelf &&
                StatsManager.nbrWood >= 100 && StatsManager.nbrIron >= 50)
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
        });
    }

    public void AmeliorationCanon()
    {
        
        TryAction("AmeCanon", AtelierType.Ingenieur, 16f,(atelier) =>
        {
            if (atelier != null && atelier.PanelTableIngenieur.activeSelf &&
                StatsManager.nbrWood >= 70 && StatsManager.nbrIron >= 30)
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
        });
    }

    public void ReparerCanon()
    {
        
        TryAction("RepCanon", AtelierType.Canon, 10, (atelier) =>
        {
            if (atelier != null && atelier.PanelCanon.activeSelf && StatsManager.nbrWood >= 20)
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
        });
    }

    public void ReparerBateau()
    {
       
        TryAction("RepBateau", AtelierType.Ingenieur, 10, (atelier) =>
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
        });
    }

    public void CuisinerRhum()
    {
        TryAction("Rhum", AtelierType.Cuisine, 10, (atelier) =>
        {
            if (atelier != null && atelier.PanelCuisine.activeSelf && StatsManager.nbrFood >= 20)
            {
                StatsManager.nbrFood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.nbrRhum += 1;
            StatsManager.UpdateText();
        });
    }

    public void CuisinerRagout()
    {
        
        TryAction("Ragout", AtelierType.Cuisine, 10, (atelier) =>
        {
            if (atelier != null && atelier.PanelCuisine.activeSelf && StatsManager.nbrFood >= 20)
            {
                StatsManager.nbrFood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.nbrRagout += 1;
            StatsManager.UpdateText();
        });
    }

    public void Manger()
    {
        
        TryAction("Manger", AtelierType.PiqueNique, 5, (atelier) =>
        {
            if (atelier != null && atelier.PanelPiqueNique.activeSelf && StatsManager.nbrRagout > 0)
            {
                StatsManager.nbrRagout -= 1;
                StatsManager.UpdateText();
            }
        });
    }

    public void AnnulerAction(AtelierType type)
    {
        if (atelierBusyMap.ContainsKey(type) && atelierBusyMap[type])
        {
            if (coroutineMap.ContainsKey(type))
            {
                StopCoroutine(coroutineMap[type].Coroutine);
                coroutineMap.Remove(type);
            }

            if (activeAteliers.ContainsKey(type) && activeAteliers[type] != null)
            {
                activeAteliers[type].HideAllSliders();
                activeAteliers.Remove(type);
            }

            SetAtelierBusy(type, false);
            Debug.Log("Action annulée sur atelier : " + type);
        }
    }

    public void AnnulerToutesActions()
    {
        foreach (AtelierType type in System.Enum.GetValues(typeof(AtelierType)))
        {
            AnnulerAction(type);
        }
    }
}
