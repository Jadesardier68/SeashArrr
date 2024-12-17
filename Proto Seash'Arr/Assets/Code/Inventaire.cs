using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public int Wood;
    

    // M�thode pour ajouter du bois
    public void AddWood(int amount)
    {
        Wood += amount;
        Debug.Log(Wood.ToString());
    }

    // M�thode pour retirer du bois
    public void RemoveWood(int amount)
    {
        Wood = Mathf.Max(Wood - amount, 0); // Emp�che des valeurs n�gatives
        Debug.Log(Wood.ToString());
    }
}
