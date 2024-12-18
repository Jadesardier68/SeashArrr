using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public int Wood;
    public int Iron;
    public int Food;
    public int Rhum;
    public int Ragout;



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

    // M�thode pour ajouter du fer
    public void AddIron(int amount)
    {
        Iron += amount;
        Debug.Log(Iron.ToString());
    }

    // M�thode pour retirer du fer
    public void RemoveIron(int amount)
    {
        Iron = Mathf.Max(Iron - amount, 0); // Emp�che des valeurs n�gatives
        Debug.Log(Iron.ToString());
    }

    // M�thode pour ajouter de la nourriture
    public void AddFood(int amount)
    {
        Food += amount;
        Debug.Log(Food.ToString());
    }

    // M�thode pour retirer de la nourriture
    public void RemoveFood(int amount)
    {
        Food = Mathf.Max(Food - amount, 0); // Emp�che des valeurs n�gatives
        Debug.Log(Food.ToString());
    }

    // M�thode pour ajouter du rhum
    public void AddRhum(int amount)
    {
        Rhum += amount;
        Debug.Log(Rhum.ToString());
    }

    // M�thode pour retirer du rhum
    public void RemoveRhum(int amount)
    {
        Rhum = Mathf.Max(Rhum - amount, 0); // Emp�che des valeurs n�gatives
        Debug.Log(Rhum.ToString());
    }

    // M�thode pour ajouter du rago�t
    public void AddRagout(int amount)
    {
        Ragout += amount;
        Debug.Log(Ragout.ToString());
    }

    // M�thode pour retirer du rago�t
    public void RemoveRagout(int amount)
    {
        Ragout = Mathf.Max(Ragout - amount, 0); // Emp�che des valeurs n�gatives
        Debug.Log(Ragout.ToString());
    }


}
