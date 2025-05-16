using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{

  
        public float rotationSpeed = 1.2f; // 360 degrés en 5 minutes

        void Update()
        {
            float rotation = Time.time * rotationSpeed;
            RenderSettings.skybox.SetFloat("_Rotation", rotation);
            DynamicGI.UpdateEnvironment();
        }
    
}
