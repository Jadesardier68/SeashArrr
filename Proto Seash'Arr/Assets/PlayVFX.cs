using System;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{

    [Header("Réparations de fuite")]
    public ParticleSystem ReparationFuite1;
    public ParticleSystem ReparationFuite2;
    public ParticleSystem ReparationFuite3;

    [Header("Explosions ennemies")]
    public ParticleSystem ExplosionEnnemie1;
    public ParticleSystem ExplosionEnnemie2;
    public ParticleSystem ExplosionEnnemie3;

    [Header("Soin joueur")]
    public VisualEffect SoinJoueur1;
    public VisualEffect SoinJoueur2;
    public VisualEffect SoinJoueur3;

    public VisualEffect SoinEnnemie1;
    public VisualEffect SoinEnnemie2;
    public VisualEffect SoinEnnemie3;

    [Header("Effets divers")]
    public VisualEffect SmokeCanon;

    void Start()
    {

        // Initialisation de tous les autres effets
        InitParticle(ReparationFuite1);
        InitParticle(ReparationFuite2);
        InitParticle(ReparationFuite3);

        InitParticle(ExplosionEnnemie1);
        InitParticle(ExplosionEnnemie2);
        InitParticle(ExplosionEnnemie3);

        InitVFX(SoinJoueur1);
        InitVFX(SoinJoueur2);
        InitVFX(SoinJoueur3);

        InitVFX(SmokeCanon);
    }

    // Méthode utilitaire pour arrêter un système de particules
    private void InitParticle(ParticleSystem ps)
    {
        if (ps != null)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Méthode utilitaire pour arrêter un visual effect
    private void InitVFX(VisualEffect fx)
    {
        if (fx != null)
            fx.Stop();
    }


    public void PlayReparationFuite1() => PlayParticle(ReparationFuite1);
    public void PlayReparationFuite2() => PlayParticle(ReparationFuite2);
    public void PlayReparationFuite3() => PlayParticle(ReparationFuite3);

    public void PlayExplosionEnnemie1() => PlayParticle(ExplosionEnnemie1);
    public void PlayExplosionEnnemie2() => PlayParticle(ExplosionEnnemie2);
    public void PlayExplosionEnnemie3() => PlayParticle(ExplosionEnnemie3);

    public void PlaySoinJoueur1() => PlayVFXEffect(SoinJoueur1);
    public void PlaySoinJoueur2() => PlayVFXEffect(SoinJoueur2);
    public void PlaySoinJoueur3() => PlayVFXEffect(SoinJoueur3);

    public void PlaySoinEnnemie1() => PlayVFXEffect(SoinEnnemie1);
    public void PlaySoinEnnemie2() => PlayVFXEffect(SoinEnnemie2);
    public void PlaySoinEnnemie3() => PlayVFXEffect(SoinEnnemie3);

    public void PlaySmokeCanon() => PlayVFXEffect(SmokeCanon);

    // Méthodes utilitaires pour lancer les effets
    private void PlayParticle(ParticleSystem ps)
    {
        if (ps != null)
            ps.Play();
    }

    private void PlayVFXEffect(VisualEffect fx)
    {
        if (fx != null)
            fx.Play();
    }
}

