using System;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{
    public VisualEffect vfx;
    public ParticleSystem particles;


    void Start()
    {
        if (vfx == null)
        {
            vfx = GetComponent<VisualEffect>();
        }

        if (vfx != null)
        {
            vfx.Stop();

        }

        // Stoppe immédiatement pour qu'il ne se voie pas

        //vfx.enabled = false;
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }

        if (particles != null)
        {
            // Arrête le système pour qu’il ne joue pas automatiquement
            particles.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
        }

    }


            public void PlayEffect()
    {
        if (vfx != null)
        {
            //fx.enabled = true;
            vfx.Play();
        }
        else
        {
          //  Debug.LogWarning("VFX non assigné !");
        }

        if (particles != null)
        {
            particles.Play();
        }
        else
        {
            //Debug.LogWarning("Particle System non assigné !");
        }
    }
}

