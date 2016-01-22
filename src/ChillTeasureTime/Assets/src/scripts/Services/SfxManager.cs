using UnityEngine;
using System.Collections;

public class SfxManager : Singleton<SfxManager>
{

    public AudioClip ChirpClip;
    public AudioClip WindClip;
    public AudioClip GetShiny;
    public AudioClip GetTrash;
    public AudioClip JumpClip;


    private AudioSource _chirpClip;
    private AudioSource _windClip;
    private AudioSource _getShiny;
    private AudioSource _getTrash;
    private AudioSource _jumpSource;


    // Use this for initialization
    void Start()
    {
        _chirpClip = gameObject.AddComponent<AudioSource>();
        _chirpClip.clip = ChirpClip;
        _chirpClip.volume = .75f;

        _windClip = gameObject.AddComponent<AudioSource>();
        _windClip.clip = WindClip;
        _windClip.volume = .5f;

        _getShiny = gameObject.AddComponent<AudioSource>();
        _getShiny.clip = GetShiny;

        _getTrash = gameObject.AddComponent<AudioSource>();
        _getTrash.clip = GetTrash;

        _jumpSource = gameObject.AddComponent<AudioSource>();
        _jumpSource.clip = JumpClip;

    }

    public void PlayChirpClip()
    {
        _chirpClip.Play();
    }

    public void PlayWindClip()
    {
        _windClip.Play();
    }

    public void PlayGetShiney()
    {
        _getShiny.Play();
    }

    public void PlayGetTrash()
    {
        _getTrash.Play();
    }

    public void PlayJump()
    {
        _jumpSource.Play();
    }

}
