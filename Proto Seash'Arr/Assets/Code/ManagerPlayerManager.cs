using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerPlayerManager : MonoBehaviour
{
    public GameObject[] prefabs; // Tableau de pr�fabriqu�s � instancier
    public int numberToSpawn = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangePrefab(InputAction.CallbackContext context)
    {
        if (context.started) // V�rifie que l'action a commenc�
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                // S�lectionne un pr�fabriqu� al�atoire dans la liste
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            }
        }
    }
}

