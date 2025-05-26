using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class Wave : MonoBehaviour
{
    public Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    [Header("Wave Settings")]
    public float waveHeight = 0.5f;
    public float waveFrequency = 1f;
    public float waveSpeed = 1f;

    void Start()
    {
        // Récupère et clone les vertices
       // mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        float time = Time.time * waveSpeed;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Déformation uniquement en Y
            float wave = Mathf.Sin(vertex.x * waveFrequency + time)
                       + Mathf.Cos(vertex.z * waveFrequency + time);

            vertex.y = wave * waveHeight;

            displacedVertices[i] = vertex;
        }

        // Appliquer et recalculer
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals(); // pour un éclairage réaliste
        mesh.RecalculateBounds();  // au cas où les bords bougent
    }
}
