using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FoodCounter : MonoBehaviour
{
    // R�f�rence au Text UI
    public TMP_Text foodText;
    // R�f�rence � la classe Inventaire
    public Inventaire inventaire;

    void Start()
    {
        // Initialise le texte � la valeur actuelle de Wood
        UpdateFoodText();
    }

    // M�thode pour mettre � jour le texte
    public void UpdateFoodText()
    {
        foodText.text = "Food : " + inventaire.Wood;
    }

    void Update()
    {
        // Si n�cessaire, mets � jour le texte � chaque frame
        UpdateFoodText();
    }
}
