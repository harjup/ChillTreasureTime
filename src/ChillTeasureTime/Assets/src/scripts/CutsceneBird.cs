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

        Animator.CrossFade("Walk", 0f);
        var direction = transform.position.x > target.x ? -1 : 1;
        transform.localScale = transform.localScale.SetX(direction);

        transform.DOMove(target, 1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Animator.CrossFade("Idle", 0f);
            done = true;
        });

        while (!done)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void Talk()
    {
        Animator.CrossFade("Talk", 0f);
    }
    public void Idle()
    {
        Animator.CrossFade("Idle", 0f);
    }
}
