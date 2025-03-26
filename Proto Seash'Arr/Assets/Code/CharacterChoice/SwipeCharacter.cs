using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeCharacter : MonoBehaviour
{
    public List<GameObject> characters;
    private int currentIndex = 0;
    public InputAction Left;
    public InputAction Right;

    
    void Start()
    {
        // Assure que seul le premier personnage est actif au d�but
        UpdateCharacterVisibility();
    }

    private void OnEnable()
    {
        Left.Enable();
        Right.Enable();
        Left.started += ChangeCharacterDown;
        Right.started += ChangeCharacterUp;
    }

    private void OnDisable()
    {
        Left.started -= ChangeCharacterDown;
        Right.started -= ChangeCharacterUp;
        Left.Disable();
        Right.Disable();
    }


    public void ChangeCharacterUp(InputAction.CallbackContext context)
    {
        characters[currentIndex].SetActive(false); // D�sactive le personnage actuel
        currentIndex = (currentIndex + 1) % characters.Count; // Incr�mente et boucle
        UpdateCharacterVisibility();
    }

    public void ChangeCharacterDown(InputAction.CallbackContext context)
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