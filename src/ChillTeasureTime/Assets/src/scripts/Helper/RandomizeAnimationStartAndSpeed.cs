using UnityEngine;
using System.Collections;

public class RandomizeAnimationStartAndSpeed : MonoBehaviour
{

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        StartCoroutine(DelayAnimationStart(animator, Random.Range(0f, 1f)));
    }

    public IEnumerator DelayAnimationStart(Animator animator, float seconds)
    {
        animator.speed = 0f;
        yield return new WaitForSeconds(seconds);
        animator.speed = Random.Range(.9f, 1.1f);
    }
}
