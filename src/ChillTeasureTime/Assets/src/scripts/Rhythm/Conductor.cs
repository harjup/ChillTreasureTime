using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.VR;

public class Conductor : MonoBehaviour
{
    public AudioSource song;
    private int crotchetsperbar = 8;
    private float bpm = 145f;

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

    public void Start()
    {
        _successAudio.clip.LoadAudioData();
        _failureAudio.clip.LoadAudioData();
        /*if (!hasoffsetadjusted)
	    {
	        if (Application.platform == RuntimePlatform.OSXWebPlayer)
	            offset = 0.35f;
	        if (Application.platform == RuntimePlatform.WindowsWebPlayer)
	            offset = 0.45f;
	    }*/

        //offset = offsetstatic;

        //AudioSource[] sounds = GetComponents<AudioSource>();
        //song = sounds[0];

        /*lasthit = 3.0f / 2.0f * crotchet;

		Debug.Log ("crotch" + crotchet);
		Debug.Log (lasthit);*/
        nextbeattime = 0;
        nextbartime = 0;

        StartMusic();

        FindObjectOfType<UserInput>().XButtonPressed += OnInput;
    }

    private void StartMusic()
    {
        song.Play();
    }


    [UsedImplicitly]
    public void Update()
    {
        CheckSongPosition();
        /*if (scrController.debug)
			{

				if (Input.GetKeyDown(KeyCode.LeftArrow))
				    {
				    	offset -= 0.01f;
				offsetstatic = offset;
				hasoffsetadjusted = true;
					}
				
				if (Input.GetKeyDown(KeyCode.RightArrow))
				    {
				    	offset += 0.01f;
					offsetstatic = offset;
				hasoffsetadjusted = true;
				    }

			txtOffset.enabled = true;
			txtOffset.text = offset.ToString();
			}*/

        /*if (beatnumber >= 3 || !scrController.isgameworld)
						scrController.started = true;*/
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

        var result = nextBeatValid || prevBeatValid ? "true" : "false";

        Debug.Log(
            string.Format("({0} < {1} > {2}) || ({3} > {1} > {4}) = {5}", 
            minTimeNext, 
            current, 
            maxTimeNext,
            minTimePrev, 
            maxTimePrev,
            result));

        return nextBeatValid || prevBeatValid;
    }

    public void OnInput(object caller)
    {
        if (CheckIfInputTimeIsValid(Songposition))
        {
            _successAudio.Play();
        }
        else
        {
            _failureAudio.Play();
        }
    }


    private void OnBeat()
    {
        //FindObjectOfType<VisualCoordinator>().SpriteContainer.IncrementActive();
        GameObject[] arrBeat = GameObject.FindGameObjectsWithTag("Beat");
			foreach (GameObject objBeat in arrBeat) {
				objBeat.SendMessage ("OnBeat");
			}
    }

    private void OnBar()
    {
        transform.DOShakePosition(.05f, Vector3.one);
        /*GameObject[] arrBar = GameObject.FindGameObjectsWithTag("Bar");
			foreach (GameObject objBar in arrBar) {
				objBar.SendMessage ("OnBar");
			}*/
    }
}