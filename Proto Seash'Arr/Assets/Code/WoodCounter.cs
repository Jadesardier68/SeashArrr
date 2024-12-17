using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WoodCounter : MonoBehaviour
{
    // R�f�rence au Text UI
    public TMP_Text woodText;
    // R�f�rence � la classe Inventaire
    public Inventaire inventaire;

    void Start()
    {
        // Initialise le texte � la valeur actuelle de Wood
        UpdateWoodText();
    }

    // M�thode pour mettre � jour le texte
    public void UpdateWoodText()
    {
        woodText.text = "Wood : " + inventaire.Wood;
    }

    void Update()
    {
        // Si n�cessaire, mets � jour le texte � chaque frame
        UpdateWoodText();
    }
}
