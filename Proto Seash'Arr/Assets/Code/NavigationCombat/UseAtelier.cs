using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAtelier : MonoBehaviour
{
    public StatsManager StatsManager;
    private List<AtelierManager> ateliers = new List<AtelierManager>();
    public float tempsAction;
    public float timer;
    public bool isBusy = false;
    private Coroutine currentActionCoroutine;
    private AtelierManager currentAtelier;

    [SerializeField] private float refreshRate = 2f; // Temps entre chaque mise à jour (en secondes)


    private void Start()
    {
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

    private void TryAction(string actionName, System.Action<AtelierManager> action, System.Action onActionCompleted = null)
    {
        if (!isBusy)
        {
            currentActionCoroutine = StartCoroutine(ActionRoutine(actionName, action, onActionCompleted));
        }
        else
        {
            Debug.Log("Action bloquée : une autre est déjà en cours.");
        }
    }

    private IEnumerator ActionRoutine(string actionName, System.Action<AtelierManager> action, System.Action onActionCompleted)
    {
        isBusy = true;
        Debug.Log("Début de l'action : " + actionName);

        AtelierManager activeAtelier = GetActiveAtelier();
        currentAtelier = activeAtelier;
        Slider activeSlider = null;

        if (activeAtelier != null)
        {
            activeAtelier.CuisineSlider?.gameObject.SetActive(false);
            activeAtelier.IngeniorSlider?.gameObject.SetActive(false);
            activeAtelier.CanonSlider?.gameObject.SetActive(false);
            activeAtelier.PiqueNiqueSlider?.gameObject.SetActive(false);

            if (activeAtelier.PanelCuisine.activeSelf) activeSlider = activeAtelier.CuisineSlider;
            else if (activeAtelier.PanelTableIngenieur.activeSelf) activeSlider = activeAtelier.IngeniorSlider;
            else if (activeAtelier.PanelCanon.activeSelf) activeSlider = activeAtelier.CanonSlider;
            else if (activeAtelier.PanelPiqueNique.activeSelf) activeSlider = activeAtelier.PiqueNiqueSlider;
        }

        if (activeSlider != null)
        {
            activeSlider.gameObject.SetActive(true);
            activeSlider.maxValue = tempsAction;
            activeSlider.value = 0;
        }

        action?.Invoke(activeAtelier); // Phase 1 : début immédiat

        timer = 0f;
        while (timer < tempsAction)
        {
            timer += Time.deltaTime;
            if (activeSlider != null)
                activeSlider.value = timer;

            yield return null;
        }

        if (activeSlider != null)
            activeSlider.gameObject.SetActive(false);

        onActionCompleted?.Invoke(); // Phase 2 : effet post-slider

        isBusy = false;
        currentAtelier = null;
        Debug.Log("Fin de l'action : " + actionName);
    }

    // Actions (identiques à avant, inchangées)
    public void AmeliorationBateau()
    {
        tempsAction = 20;
        TryAction("Amélioration Bateau", (atelier) =>
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
        tempsAction = 16;
        TryAction("AmeCanon", (atelier) =>
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
        tempsAction = 10;
        TryAction("RepCanon", (atelier) =>
        {
            if (atelier != null && atelier.PanelCanon.activeSelf && StatsManager.nbrWood >= 20)
            {
                StatsManager.nbrWood -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.canonHealth += (int)(0.2f * StatsManager.canonMaxHealth);
            if(StatsManager.canonHealth > StatsManager.canonMaxHealth)
            {
                StatsManager.canonHealth=StatsManager.canonMaxHealth;
            }
            StatsManager.UpdateText();
        });
    }

    public void ReparerBateau()
    {
        tempsAction = 10;
        TryAction("RepBateau", (atelier) =>
        {
            if (StatsManager.nbrIron >= 20)
            {
                StatsManager.nbrIron -= 20;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            StatsManager.boatHealth += (int)(0.2f * StatsManager.boatMaxHealth);
            StatsManager.UpdateText();
        });
    }

    public void CuisinerRhum()
    {
        tempsAction = 10;
        TryAction("Rhum", (atelier) =>
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
        tempsAction = 10;
        TryAction("Ragout", (atelier) =>
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
        tempsAction = 5;
        TryAction("Manger", (atelier) =>
        {
            if (atelier != null && atelier.PanelPiqueNique.activeSelf && StatsManager.nbrRagout > 0)
            {
                StatsManager.nbrRagout -= 1;
                StatsManager.UpdateText();
            }
        },
        () =>
        {
            
        });
    }

    public void AnnulerAction()
    {
        if (isBusy && currentActionCoroutine != null)
        {
            StopCoroutine(currentActionCoroutine);
            currentActionCoroutine = null;
            isBusy = false;
            timer = 0;

            if (currentAtelier != null)
            {
                currentAtelier.CuisineSlider?.gameObject.SetActive(false);
                currentAtelier.IngeniorSlider?.gameObject.SetActive(false);
                currentAtelier.CanonSlider?.gameObject.SetActive(false);
                currentAtelier.PiqueNiqueSlider?.gameObject.SetActive(false);
            }

            currentAtelier = null;

            Debug.Log("Action annulée.");
        }
    }
}
