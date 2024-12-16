using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoneUIController : MonoBehaviour
{
    public GameObject linkedUI;  // L'UI qui sera activ�e et d�sactiv�e pour cette zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (linkedUI != null)
            {
                linkedUI.SetActive(true);  // Activer l'UI quand le joueur entre
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (linkedUI != null)
            {
                linkedUI.SetActive(false);  // D�sactiver l'UI quand le joueur sort
            }
        }
    }
}