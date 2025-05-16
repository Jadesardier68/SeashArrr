using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauOscillation : MonoBehaviour
{

    public float amplitude = 1f;      // Amplitude de l'oscillation (1°)
    public float vitesse = 0.05f;     // Vitesse de l'oscillation
    public float angleMoyen = -94f;   // Angle moyen autour duquel osciller

    private float angleInitialY;
    private float angleInitialZ;

    void Start()
    {
        Vector3 angles = transform.localEulerAngles;
        angleInitialY = angles.y;
        angleInitialZ = angles.z;
    }

    void Update()
    {
        float angleX = angleMoyen + Mathf.Sin(Time.time * vitesse) * amplitude;
        transform.localEulerAngles = new Vector3(angleX, angleInitialY, angleInitialZ);
    }
}
