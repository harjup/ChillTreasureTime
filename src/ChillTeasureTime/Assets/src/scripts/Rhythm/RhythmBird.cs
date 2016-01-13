using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RhythmBird : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private Conductor conductor;

    public GameObject Plant;

	void Start ()
	{
        audioSource = GetComponent<AudioSource>();
	    animator = GetComponent<Animator>();
	    conductor = FindObjectOfType<Conductor>();
	}


    private int beatNumber = 0;
    public void OnBeat()
    {
        beatNumber++;
        if (beatNumber == 4)
        {
            beatNumber = 0;
            animator.CrossFade("WingReady", 0f);
            return;
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WingReady"))
        {
            animator.SetTrigger("PushButtonFailed");
            return;
        }

        animator.SetTrigger("OnBeat");
    }

    public void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WingReady")
            && Input.GetKeyDown(KeyCode.X))
        {
            audioSource.Play();
            animator.SetTrigger("PushedButton");
            Plant.transform.DOShakePosition(.25f, .25f, 30);
        }
    }
}
