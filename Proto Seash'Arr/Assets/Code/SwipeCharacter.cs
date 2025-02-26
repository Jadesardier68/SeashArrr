using System.Collections.Generic;
using UnityEngine;

public class SwipeCharacter : MonoBehaviour
{
    public List<GameObject> characters;
    private int currentIndex = 0;

    void Start()
    {
        // Assure que seul le premier personnage est actif au d�but
        UpdateCharacterVisibility();
    }

    public void ChangeCharacterUp()
    {
        characters[currentIndex].SetActive(false); // D�sactive le personnage actuel
        currentIndex = (currentIndex + 1) % characters.Count; // Incr�mente et boucle
        UpdateCharacterVisibility();
    }

    public void ChangeCharacterDown()
    {
        characters[currentIndex].SetActive(false); // D�sactive le personnage actuel
        currentIndex = (currentIndex - 1 + characters.Count) % characters.Count; // D�cr�mente et boucle
        UpdateCharacterVisibility();
    }

    private void UpdateCharacterVisibility()
    {
        // D�sactive tous les personnages pour �viter les doublons
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        // Active uniquement le personnage courant
        characters[currentIndex].SetActive(true);
    }
}