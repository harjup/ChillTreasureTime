using UnityEngine;
using System.Collections;

public class RhythmRival : MonoBehaviour, IBeat
{

    private Animator _animator;
    private AudioSource _audioSource;

    public Animator Animator {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            return _animator;
        }}

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void Peck()
    {
        Animator.CrossFade("Peck", 0f);
        _audioSource.Play();
    }

    public void OnBeat(int bar, int beat)
    {
        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Peck"))
        {
            Animator.SetTrigger("OnBeat");
        }        
    }
}
