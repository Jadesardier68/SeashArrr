using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RhumCounter : MonoBehaviour
{
    // R�f�rence au Text UI
    public TMP_Text rhumText;
    // R�f�rence � la classe Inventaire
    public Inventaire inventaire;

    void Start()
    {
        // Initialise le texte � la valeur actuelle de Wood
        UpdateRhumText();
    }

    // M�thode pour mettre � jour le texte
    public void UpdateRhumText()
    {
        rhumText.text = "Rhum : " + inventaire.Rhum;
    }

    void Update()
    {
        // Si n�cessaire, mets � jour le texte � chaque frame
        UpdateRhumText();
    }
}
