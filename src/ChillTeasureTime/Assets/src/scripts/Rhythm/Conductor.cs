using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.VR;

public class Conductor : MonoBehaviour
{
    public AudioSource song;
    private int crotchetsperbar = 4;
    private float bpm = 150f;

    public float Crotchet
    {
        get { return 60.0f/bpm; }
    }

    public float Songposition
    {
        get { return song.timeSamples/44100.0f - offset; }
    }

    public float lasthit; // = 0.0f; //the last (snapped to beat) time spacebar was pressed
    public float actuallasthit;
    private float prevbeattime = 0.0f;
    private float nextbeattime = 0.0f;
    private float nextbartime = 0.0f;
    public float offset = 0.0f; //positive means the song must be minussed!
    public static float offsetstatic = 0.40f;
    public static bool hasoffsetadjusted = false;
    public int beatnumber = 0;
    public int barnumber = 0;
    public GUIText txtStatus;
    public GUIText txtOffset;

    [SerializeField] private AudioSource _successAudio;
    [SerializeField] private AudioSource _failureAudio;

    List<IBeat> beatEntitiyList = new List<IBeat>();
    List<IBar> barEntitiyList = new List<IBar>();

    public void Start()
    {
        _successAudio.clip.LoadAudioData();
        _failureAudio.clip.LoadAudioData();
     
        nextbeattime = 0;
        nextbartime = 0;

        StartMusic();

        FindObjectOfType<UserInput>().XButtonPressed += OnInput;

        beatEntitiyList = FindObjectsOfType<MonoBehaviour>().Where(m => m is IBeat).Cast<IBeat>().ToList();
        barEntitiyList = FindObjectsOfType<MonoBehaviour>().Where(m => m is IBar).Cast<IBar>().ToList();
    }

    private void StartMusic()
    {
        song.Play();
    }


    [UsedImplicitly]
    public void FixedUpdate()
    {
        CheckSongPosition();
    }

    public void CheckSongPosition()
    {
        if (Songposition > nextbeattime)
        {
            OnBeat();

            prevbeattime = nextbeattime;
            nextbeattime += Crotchet;
            beatnumber++;
        }

        if (Songposition > nextbartime)
        {
            OnBar();
            nextbartime += Crotchet*crotchetsperbar;
            barnumber++;
        }
    }

    public bool CheckIfInputTimeIsValid(float current)
    {
        // Let's be liberal with a whole 8th note
        var minTimeNext = nextbeattime - Crotchet / 4f;
        var maxTimeNext = nextbeattime + Crotchet / 4f;

        var minTimePrev = prevbeattime - Crotchet / 4f;
        var maxTimePrev = prevbeattime + Crotchet / 4f;

        var nextBeatValid = current > minTimeNext && current < maxTimeNext;
        var prevBeatValid = current > minTimePrev && current < maxTimePrev;

        return nextBeatValid || prevBeatValid;
    }

    public BeatTracker BeatTracker;

    public void OnInput(object caller)
    {
        BeatTracker.OnValidInputForBeat(Songposition, (Crotchet / 2f));

        /*if (CheckIfInputTimeIsValid(Songposition))
        {
            BeatTracker.OnValidInputForBeat(Songposition, (Crotchet / 2f));
            //_successAudio.Play();
        }
        else
        {
            //BeatTracker.OnInValidInputBeat();
        }*/
    }



    private BeatTracker _beatTracker;


    private void OnBeat()
    {
	    foreach (IBeat objBeat in beatEntitiyList) {
		    objBeat.OnBeat(barnumber, beatnumber);
	    }
    }

    private void OnBar()
    {
        //transform.DOShakePosition(.05f, Vector3.one);
        foreach (var obj in barEntitiyList)
        {
            obj.OnBar(barnumber);
        }
    }
}