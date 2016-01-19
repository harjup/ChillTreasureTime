using UnityEngine;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;

public class MusicService : Singleton<MusicService>
{
    public AudioClip BeachSnow;
    public AudioClip Lighthouse;
    public AudioSource PrimaryAudioSource;


    // Use this for initialization
    void Start()
    {
        PrimaryAudioSource = GetComponent<AudioSource>();
    }

    public void PlayBeachSnow()
    {
        PrimaryAudioSource.clip = BeachSnow;
        PrimaryAudioSource.Play();
    }

    public void TransitionToLighthouse()
    {
        DOTween.Sequence()
            .Append(PrimaryAudioSource.DOFade(0f, .5f))
            .AppendCallback(() =>
            {
                PrimaryAudioSource.clip = Lighthouse;
                PrimaryAudioSource.Play();
            })
            .Append(PrimaryAudioSource.DOFade(1f, 1f));
    }

    public void TransitionToBeachSnow()
    {
        DOTween.Sequence()
            .Append(PrimaryAudioSource.DOFade(0f, .5f))
            .AppendCallback(() =>
            {
                PrimaryAudioSource.clip = BeachSnow;
                PrimaryAudioSource.Play();
            })
            .Append(PrimaryAudioSource.DOFade(1f, 1f));
    }

    public void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Lighthouse")
        {
            TransitionToLighthouse();
        }

        if (Application.loadedLevelName == "Main")
        {
            TransitionToBeachSnow();
        }
    }

    
}
