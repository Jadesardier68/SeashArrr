using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Procedural : MonoBehaviour
{
    private List<Vector3> predefinedPositions; // Liste des positions actuelles des boutons
    private List<Vector3> availablePositions; // Liste des positions disponibles pour le shuffle
    private List<GameObject> objectsToShuffle; // Liste des objets (boutons) � m�langer

    public GameObject BoatButton; // R�f�rence au bouton du bateau
    public List<GameObject> IslandButtonsParent; // Conteneur des boutons des �les (si n�cessaire)

    // Cette m�thode sera appel�e au lancement de la sc�ne
    void Start()
    {
        // Trouver tous les boutons dans la sc�ne
        objectsToShuffle = new List<GameObject>();
        Button[] buttons = FindObjectsOfType<Button>();

        // Ajouter les boutons � la liste et r�cup�rer leurs positions actuelles
        predefinedPositions = new List<Vector3>(); // Initialiser la liste des positions
        foreach (Button button in buttons)
        {
            if (button.gameObject == BoatButton)
            {
                // Ne pas ajouter le bouton du bateau � la liste des objets � m�langer
                continue;
            }

            objectsToShuffle.Add(button.gameObject);
            predefinedPositions.Add(button.gameObject.transform.position); // Ajouter la position actuelle � la liste
        }

        // V�rifier si nous avons suffisamment de positions pr�d�finies
        if (predefinedPositions.Count < objectsToShuffle.Count)
        {
            Debug.LogError("Il n'y a pas assez de positions pr�d�finies pour tous les objets.");
            return;
        }

        // Initialiser la liste des positions disponibles avec les positions pr�d�finies
        availablePositions = new List<Vector3>(predefinedPositions);

        // M�langer et placer les objets (boutons) sauf le bouton du bateau
        ShuffleAndPlaceObjects();
    }

    // Cette m�thode sera appel�e pour m�langer et placer les objets
    void ShuffleAndPlaceObjects()
    {
        // Cr�er une copie de la liste des positions pr�d�finies
        availablePositions = new List<Vector3>(predefinedPositions);

        // M�lange les objets � placer
        System.Random rand = new System.Random();
        for (int i = objectsToShuffle.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            GameObject temp = objectsToShuffle[i];
            objectsToShuffle[i] = objectsToShuffle[j];
            objectsToShuffle[j] = temp;
        }

        // Assigner � chaque objet une position de la liste disponible
        List<Vector3> usedPositions = new List<Vector3>();

        foreach (GameObject obj in objectsToShuffle)
        {
            Vector3 chosenPosition;

            // Trouver une position valide pour chaque objet
            do
            {
                int randomIndex = Random.Range(0, availablePositions.Count);
                chosenPosition = availablePositions[randomIndex];

                // V�rifier si la position choisie est adjacente � une position d�j� utilis�e
                bool isAdjacent = false;
                foreach (Vector3 usedPosition in usedPositions)
                {
                    if (IsAdjacent(usedPosition, chosenPosition))
                    {
                        isAdjacent = true;
                        break;
                    }
                }

                // Si la position est adjacente � une autre, on essaie une autre position
            } while (usedPositions.Exists(pos => IsAdjacent(pos, chosenPosition)));

            // Placer l'objet � cette position
            obj.transform.position = chosenPosition;

            // Ajouter la position utilis�e � la liste des positions utilis�es
            usedPositions.Add(chosenPosition);

            // Enlever cette position de la liste des positions disponibles
            availablePositions.Remove(chosenPosition);
        }

        // Placer le BoatButton � sa position initiale ou dans une position sp�cifique
        BoatButton.transform.position = BoatButton.transform.position; // Vous pouvez d�finir une nouvelle position pour le BoatButton si n�cessaire
    }

    // M�thode pour v�rifier si deux positions sont adjacentes
    bool IsAdjacent(Vector3 pos1, Vector3 pos2)
    {
        // On consid�re les positions adjacentes si elles sont directement � gauche, � droite, en haut ou en bas
        return Mathf.Abs(pos1.x - pos2.x) <= 1f && Mathf.Abs(pos1.y - pos2.y) <= 1f && pos1 != pos2;
    }
}