using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class MusicService : Singleton<MusicService>
{
    public AudioClip BeachSnow;
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
}
