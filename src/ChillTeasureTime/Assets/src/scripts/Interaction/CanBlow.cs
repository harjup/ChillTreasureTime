using System;
using UnityEngine;
using System.Collections;
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

    public virtual IEnumerator DoSequence(Action action)
    {
        Debug.LogError("DoSequence is not overloaded on '" + gameObject.name + "', probably unintended");
        yield return null;
    }
}

public class CanBlow : CanInteract
{
    public GameObject ShineySpawnLocation;
    public GameObject ShinyToSpawn;

    void Start()
    {
        ShineySpawnLocation = transform.Find("ShinySpawn").gameObject;

        InterestedHitboxType = HitboxType.PlayerWing;
    }

    public override IEnumerator DoSequence(Action action)
    {
        transform.DOShakePosition(.25f, Vector3.one, 40);
        var go = Instantiate(ShinyToSpawn, ShineySpawnLocation.transform.position, Quaternion.identity) as GameObject;

        var shiny = go.GetComponent<Collectable>();
        shiny.SetSpeed(new Vector3(1f, 1f, 0f));

        yield return null;
    }
}
