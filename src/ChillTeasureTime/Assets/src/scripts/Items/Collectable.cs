using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum CollectableType
{
    Unknown,
    Good,
    Worthless
}

public class Collectable : MonoBehaviour
{
    private Action _callBack;

    public CollectableType MyType;

    public static bool ShineyHasBeenCollected = false;

    public void SetCallback(Action cb)
    {
        _callBack = cb;
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            // Do animation
            GetComponent<Collider>().enabled = false;
            StartCoroutine(GoToPlayerSack(player));
        }
    }

    private IEnumerator GoToPlayerSack(Player player)
    {
        yield return DOTween
            .Sequence()
            .Append(transform.DOLocalMoveY(3f, .5f).SetEase(Ease.Linear))
            .WaitForCompletion();

        transform.DOScale(transform.localScale / 4f, .25f).SetEase(Ease.OutQuad);

        var timeElapsed = 0f;
        while ((transform.position - player.transform.position).magnitude > 1f)
        {
            transform.position = iTween.Vector3Update(transform.position, player.transform.position, 10f);
            timeElapsed += Time.smoothDeltaTime;

            if (timeElapsed > 2f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        State.Instance.AddToPlayerItemCount(1, MyType);
        
        if (_callBack != null)
        {
            _callBack();
        }

        // Kill self
        Destroy(gameObject);

        //TODO: Callback to say it's ok to level load
    }
}
