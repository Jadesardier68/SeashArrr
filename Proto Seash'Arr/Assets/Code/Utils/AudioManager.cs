using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Random = System.Random;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;
    public AudioSource music, sfx;

    // Musics
    public AudioClip mainTheme, navTheme, fightTheme, loseTheme;
    //Actions sfx
    public AudioClip step1, step2, step3, step4, step5, step6, step7;
    //Ambient sfx
    public AudioClip cheers;
    //Jingle sfx
    public AudioClip attack, island, title, victory;
    //UI sfx
    public AudioClip choice, clic, undo;
    // Voices sfx
    /// <summary>
    /// //////////////////////////////////////////////////////////////////
    /// </summary>

    private List<AudioClip> steps = new List<AudioClip>();
    
    public enum Music
    {
        MainTheme,
        NavTheme,
        FightTheme
    }
    public enum SFX
    {
        A_Step,
        A_Cheers,
        J_AttackAnnounce,
        J_IslandAnnounce,
        J_Title,
        J_Victory,
        UI_Choice,
        UI_Clic,
        UI_Undo
    }

    private void Awake()
    {
        steps.Add(step1);
        steps.Add(step2);
        steps.Add(step3);
        steps.Add(step4);
        steps.Add(step5);
        steps.Add(step6);
        steps.Add(step7);
    }

    // Start is called before the first frame update
    void Start()
    {
        music.clip = mainTheme;
        music.loop = true;
        music.Play();
    }

    public void PlayMusic(Music new_music)
    {
        switch (new_music)
        {
            case Music.MainTheme:
                if(music.clip != mainTheme)
                {
                    music.clip = mainTheme;
                    music.Play();
                }
                break;
            case Music.NavTheme:
                if(music.clip != navTheme)
                {
                    music.clip = navTheme;
                    music.Play();
                }
                break;
            case Music.FightTheme:
                if(music.clip != fightTheme)
                {
                    music.clip = fightTheme;
                    music.Play();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(music), music, null);
        }
    }

    public void PlaySfx(SFX new_sfx)
    {
        switch (new_sfx)
        {
            case SFX.A_Step:
                var stepfx = steps[new Random().Next(0, 6)];
                sfx.PlayOneShot(stepfx);
                break;
            case SFX.A_Cheers:
                sfx.PlayOneShot(cheers);
                break;
            case SFX.J_AttackAnnounce:
                sfx.PlayOneShot(attack);
                break;
            case SFX.J_IslandAnnounce:
                sfx.PlayOneShot(island);
                break;
            case SFX.J_Title:
                sfx.PlayOneShot(title);
                break;
            case SFX.J_Victory:
                sfx.PlayOneShot(victory);
                break;
            case SFX.UI_Choice:
                sfx.PlayOneShot(choice);
                break;
            case SFX.UI_Clic:
                sfx.PlayOneShot(clic);
                break;
            case SFX.UI_Undo:
                sfx.PlayOneShot(undo);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(new_sfx), new_sfx, null);
        }
    }

    
}
