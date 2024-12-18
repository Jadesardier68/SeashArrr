using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class IronCounter : MonoBehaviour
{
    // R�f�rence au Text UI
    public TMP_Text ironText;
    // R�f�rence � la classe Inventaire
    public Inventaire inventaire;

    void Start()
    {
        // Initialise le texte � la valeur actuelle de Wood
        UpdateIronText();
    }

    // M�thode pour mettre � jour le texte
    public void UpdateIronText()
    {
        ironText.text = "Iron : " + inventaire.Iron;
    }

    void Update()
    {
        // Si n�cessaire, mets � jour le texte � chaque frame
        UpdateIronText();
    }
}
