using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSprintSpawner : MonoBehaviour
{
    public GameObject footprintPrefab;
    public float spawnInterval = 0.4f;
    public Transform leftFoot;
    public Transform rightFoot;
    public float raycastDistance = 1.5f;
    public LayerMask groundLayer;

    private float timer = 0f;
    private bool isLeftFootNext = true;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }


    void Update()
    {
        timer += Time.deltaTime;

        // Vérifie si le personnage se déplace
        if (Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            if (timer >= spawnInterval)
            {
                SpawnFootprint();
                timer = 0f;
            }
        }

        lastPosition = transform.position;
    }

    void SpawnFootprint()
    {
        Transform footTransform = isLeftFootNext ? leftFoot : rightFoot;

        Ray ray = new Ray(footTransform.position + Vector3.up * 0.2f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayer))
        {
            Quaternion rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            Vector3 spawnPos = hit.point + hit.normal * 0.01f; // Légère élévation pour éviter le clipping

            Instantiate(footprintPrefab, spawnPos, rotation);
        }

        isLeftFootNext = !isLeftFootNext;
    }
}
