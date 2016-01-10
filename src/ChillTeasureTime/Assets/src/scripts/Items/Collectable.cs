using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Collectable : MonoBehaviour 
{

    public void OnTriggerEnter(Collider other)
    {
        // Do animation
        GetComponent<Collider>().enabled = false;

        DOTween
            .Sequence()
            .Append(transform.DOLocalMoveY(3f, 1f).SetEase(Ease.Linear))
            .Append(transform.transform.DOMoveY(20, .5f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                // Increment the shinies 
                State.Instance.AddToShineyCount(1);
                // Kill self
                Destroy(gameObject);
            });
    }
}
