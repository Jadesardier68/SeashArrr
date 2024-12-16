using System.Collections.Generic;
using UnityEngine;

public class SwipeCharacter : MonoBehaviour
{
    public List<GameObject> characters;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Assure que seul le premier personnage est actif au d�but
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].SetActive(i == currentIndex);
        }
    }

    // Fonction pour changer de personnage
    public void ChangeCharacterUp()
    {
        // D�sactive le personnage actuel
        characters[currentIndex].SetActive(false);

        // Incr�mente l'index et retourne au d�but si on est � la fin de la liste
        currentIndex = (currentIndex + 1) % characters.Count;

        // Active le nouveau personnage
        characters[currentIndex].SetActive(true);
    }
}

