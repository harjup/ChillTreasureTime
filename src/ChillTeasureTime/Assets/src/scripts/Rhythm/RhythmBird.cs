using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RhythmBird : MonoBehaviour, IBeat
{
    public int SuccessCount { get; set; }
    public int MistakeCount { get; set; }

    public int MistakesInRound { get; set; }

    public GameObject Plant;

    private Animator _animator;
    private AudioSource _audioSource;
    private AudioSource _failAudioSource;

    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            return _animator;
        }
    }

	void Start ()
	{
        _audioSource = GetComponent<AudioSource>();
	    _failAudioSource = transform.FindChild("Whoops").GetComponent<AudioSource>();
	    _animator = GetComponent<Animator>();
	}


    public void OnBeat(int bar, int beat)
    {
        /*if (beat == 4)
        {
            animator.CrossFade("WingReady", 0f);
            return;
        }*/
        
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("WingReady"))
        {
            animator.SetTrigger("PushButtonFailed");
            return;
        }*/

        Animator.SetTrigger("OnBeat");
    }

    public void Update()
    {
       /* if (animator.GetCurrentAnimatorStateInfo(0).IsName("WingReady")
            && Input.GetKeyDown(KeyCode.X))
        {
            audioSource.Play();
            animator.SetTrigger("PushedButton");
            Plant.transform.DOShakePosition(.25f, .25f, 30);
        }*/
    }

    public void Peck()
    {
        Debug.Log("Peck");
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Peck"))
        {
            // This will reset the animation despite already being on it, the alternatives do not
            Animator.ForceStateNormalizedTime(0f);
        }
        Animator.CrossFade("Peck", 0f);
        _audioSource.Play();
    }

    public void Whoops()
    {
        Debug.Log("EWhoops");
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Peck"))
        {
            // This will reset the animation despite already being on it, the alternatives do not
            Animator.ForceStateNormalizedTime(0f);
        }

        Animator.CrossFade("Peck", 0f);
        _failAudioSource.Play();
        MistakesInRound++;
    }

    public void EndAnim()
    {
        if (MistakesInRound <= 1)
        {
            Animator.CrossFade("RhythmSuccess", 0f);
            SuccessCount++;
        }
        else
        {
            Animator.CrossFade("RhythmFail", 0f);
            MistakeCount++;
        }

    }
}
