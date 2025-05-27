using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVFX : MonoBehaviour
{
    public PlayVFX vfxScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (vfxScript != null)
            {
                vfxScript.PlayEffect(); // Appel du VFX depuis un autre script
            }
        }
    }
}
