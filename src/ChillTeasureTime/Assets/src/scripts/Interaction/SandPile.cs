using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SandPile : CanInteract
{

    public string Guid;
    public static List<string> ClearedPiles = new List<string>();
    public static Dictionary<string, int> CollectedCounts = new Dictionary<string, int>();

    public GameObject ShineySpawnLocation;
    public GameObject ShinyToSpawn;
    public GameObject BlockingCollider;
    public GameObject Sprite;

    private int count;
    public int MaxCount = 3;

  
    void Start()
    {
        ShineySpawnLocation = transform.Find("ShinySpawn").gameObject;

        InterestedHitboxType = HitboxType.PlayerWing;

        if (!CollectedCounts.ContainsKey(Guid))
        {
            CollectedCounts.Add(Guid, 0);
        }

        count = CollectedCounts[Guid];

        ShineySpawnLocation.transform.SetParent(null);

        if (ClearedPiles.Contains(Guid))
        {
            Destroy(Sprite);
            Destroy(BlockingCollider);

            if (count < MaxCount)
            {
                StartCoroutine(SpawnShinysWithDelay(MaxCount - count));
            }
        }
    }

    IEnumerator SpawnShinysWithDelay(int amountLeft)
    {
        for (int i = 0; i < amountLeft; i++)
        {
            var go = Instantiate(ShinyToSpawn, ShineySpawnLocation.transform.position, Quaternion.identity) as GameObject;
            var shiny = go.GetComponent<Collectable>();
            shiny.SetCallback(() =>
            {
                CollectedCounts[Guid]++;
            });

            yield return new WaitForSeconds(.25f);
        }
    }

    public override IEnumerator DoSequence(Action action)
    {
        // If we already have been destroyed do nothing
        if (Sprite == null)
        {
            yield break;
        }

        yield return DOTween
            .Sequence()
            .Append(Sprite.transform.DOShakePosition(3f, Vector3.one.SetY(0f), 40))
            .Insert(0f, transform.DOMoveY(-1f, 3f))
            .InsertCallback(.3f, SpawnShiny)
            .InsertCallback(.6f, SpawnShiny)
            .WaitForCompletion();

        Destroy(BlockingCollider);

        if (!ClearedPiles.Contains(Guid))
        {
            ClearedPiles.Add(Guid);
        }
        
        yield return null;
    }

    private void SpawnShiny()
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
}
