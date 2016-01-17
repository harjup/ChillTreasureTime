using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CutsceneBird : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator
    {
        get { return _animator ?? (_animator = GetComponent<Animator>()); }
    }

    public IEnumerator WalkToTarget(Vector3 target)
    {
        bool done = false;

        Animator.SetTrigger("ToWalk");
        transform.DOMove(target, 1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Animator.SetTrigger("ToIdle");
            done = true;
        });

        while (!done)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
