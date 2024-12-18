using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RagoutCounter : MonoBehaviour
{
    // R�f�rence au Text UI
    public TMP_Text ragoutText;
    // R�f�rence � la classe Inventaire
    public Inventaire inventaire;

    void Start()
    {
        // Initialise le texte � la valeur actuelle de Wood
        UpdateRagoutText();
    }

    // M�thode pour mettre � jour le texte
    public void UpdateRagoutText()
    {
        ragoutText.text = "Ragout : " + inventaire.Ragout;
    }

    void Update()
    {
        // Si n�cessaire, mets � jour le texte � chaque frame
        UpdateRagoutText();
    }
}
