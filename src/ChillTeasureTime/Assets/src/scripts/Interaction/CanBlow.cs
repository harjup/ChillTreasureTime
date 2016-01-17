using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CanInteract : MonoBehaviour
{
    public bool KillAfterAnimation;
    protected HitboxType InterestedHitboxType;

    public void OnTriggerEnter(Collider other)
    {
        var hitbox = other.GetComponent<Hitbox>();

        if (hitbox != null && hitbox.Type == InterestedHitboxType)
        {
            hitbox.AddToInteractables(this);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        var hitbox = other.GetComponent<Hitbox>();

        if (hitbox != null && hitbox.Type == InterestedHitboxType)
        {
            hitbox.RemoveFromInteractables(this);
        }
    }

    public virtual IEnumerator DoSequence(Action action)
    {
        Debug.LogError("DoSequence is not overloaded on '" + gameObject.name + "', probably unintended");
        yield return null;
    }
}

public class CanBlow : CanInteract
{
    public string Guid;
    public static Dictionary<string, int> CollectedCounts = new Dictionary<string, int>();


    public GameObject ShineySpawnLocation;
    public GameObject ShinyToSpawn;

    public Animator _animator;

    private int count;
    public int MaxCount = 1;

    void Start()
    {
        ShineySpawnLocation = transform.Find("ShinySpawn").gameObject;

        InterestedHitboxType = HitboxType.PlayerWing;

        if (!CollectedCounts.ContainsKey(Guid))
        {
            CollectedCounts.Add(Guid, 0);
        }

        count = CollectedCounts[Guid];

        _animator = GetComponentInChildren<Animator>();
    }

    public override IEnumerator DoSequence(Action action)
    {
        //transform.DOShakePosition(.25f, Vector3.one, 40);
        _animator.SetTrigger("GetBlown");

        if (count < MaxCount)
        {
            var go = Instantiate(ShinyToSpawn, ShineySpawnLocation.transform.position, Quaternion.identity) as GameObject;

            var shiny = go.GetComponent<Collectable>();
            shiny.SetSpeed(new Vector3(3f, 3f, 0f));

            shiny.SetCallback(() =>
            {
                 CollectedCounts[Guid]++;
            });

            count++;
        }

        yield return null;
    }
}
