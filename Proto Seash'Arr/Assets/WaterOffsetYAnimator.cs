using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOffsetYAnimator : MonoBehaviour
{
    public float scrollSpeedY = 0.05f; // Vitesse verticale réduite (2 fois plus lent)

    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        offset.y -= scrollSpeedY * Time.deltaTime;
        if (offset.y < 0f) offset.y += 1f; // Réinitialisation cyclique pour rester entre 0 et 1
        rend.material.SetTextureOffset("_MainTex", offset);
    }
}
