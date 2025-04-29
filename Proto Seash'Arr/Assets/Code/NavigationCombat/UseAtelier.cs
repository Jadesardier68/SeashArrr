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

    [SerializeField] private float refreshRate = 2f; // Temps entre chaque mise � jour (en secondes)


    private void Start()
    {
        StartCoroutine(RefreshAtelierList());
    }

    private IEnumerator RefreshAtelierList()
    {
        while (true)
        {
            ateliers = new List<AtelierManager>(FindObjectsOfType<AtelierManager>());
            Debug.Log("Nombre total de scripts AtelierManager dans la sc�ne : " + ateliers.Count);
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
            StartCoroutine(ActionRoutine(actionName, action, onActionCompleted));
        }
        else
        {
            Debug.Log("Action bloqu�e : une autre est d�j� en cours.");
        }
    }

    private IEnumerator ActionRoutine(string actionName, System.Action<AtelierManager> action, System.Action onActionCompleted)
    {
        isBusy = true;
        Debug.Log("D�but de l'action : " + actionName);

        AtelierManager activeAtelier = GetActiveAtelier();
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

        action?.Invoke(activeAtelier); // Phase 1 : d�but imm�diat

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
        Debug.Log("Fin de l'action : " + actionName);
    }

    // Actions (identiques � avant, inchang�es)
    public void AmeliorationBateau()
    {
        tempsAction = 20;
        TryAction("Am�lioration Bateau", (atelier) =>
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
            StatsManager.boatHealth += 30;
            StatsManager.canonHealth += 30;
            StatsManager.UpdateText();
        });
    }
}