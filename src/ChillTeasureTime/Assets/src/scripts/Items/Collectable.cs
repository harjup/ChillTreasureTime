using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Sprite> GoodSprites;
    public List<Sprite> TrashSprites;

    public AudioClip OnCollectShiny;
    public AudioClip OnCollectTrash;

    public void Start()
    {
        if (MyType == CollectableType.Good)
        {
            GetComponent<SpriteRenderer>().sprite = GoodSprites.AsRandom().ToList().First();
        }
        if (MyType == CollectableType.Worthless)
        {
            GetComponent<SpriteRenderer>().sprite = TrashSprites.AsRandom().ToList().First();
        }
        
    }

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
            .Append(transform.DOLocalMoveY(transform.position.y + 3f, .5f).SetEase(Ease.Linear))
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

        if (MyType == CollectableType.Good)
        {
            SfxManager.Instance.PlayGetShiney();
        }
        else
        {
            SfxManager.Instance.PlayGetTrash();
        }
        
        if (_callBack != null)
        {
            _callBack();
        }

        // Kill self
        Destroy(gameObject);

        //TODO: Callback to say it's ok to level load
    }

    public void SetSpeed(Vector3 vector3)
    {
        GetComponent<Rigidbody>().velocity = vector3;
    }
}
