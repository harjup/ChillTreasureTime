using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Nest : MonoBehaviour
{
    public string TextId;

    private GuiCanvas _guiCanvas;

    private IEnumerator _cashInRoutine;

    public GameObject ShiningBitDisplay;

    public List<GameObject> ShinyBits = new List<GameObject>();

    public GameObject CanDepositObject;

    public List<Milestone> Milestones = new List<Milestone>
    {
        new Milestone(1, "1")
    };

    public void Start()
    {
        var shinyBitsTransform = transform
            .FindChild("ShinyBits");

        CanDepositObject = transform.FindChild("CanDeposit").gameObject;

        foreach (Transform t in shinyBitsTransform)
        {
            ShinyBits.Add(t.gameObject);
        }

        UpdateShinyDisplay();

        _guiCanvas = GuiCanvas.Instance;
    }

    public void Update()
    {
        CanDepositObject.SetActive(State.Instance.PlayerShinyCount > 0);
    }

    public void UpdateShinyDisplay()
    {
        var shinyBitsTransform = transform
          .FindChild("ShinyBits");


        var c = 0;
        foreach (Transform t in shinyBitsTransform)
        {
            t.gameObject.SetActive(false);

            if (c + 1 <= State.Instance.StoredShinyCount)
            {
                t.gameObject.SetActive(true);
            }

            c++;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null && _cashInRoutine == null)
        {
            _cashInRoutine = CashInAnim(player);
            StartCoroutine(_cashInRoutine);
        }
    }



    private IEnumerator CashInAnim(Player player)
    {
        // Amount to bank
        int amountToBank = State.Instance.PlayerShinyCount;

        var seqs = new List<YieldInstruction>();

        while (amountToBank > 0)
        {
            // Create collectable above player
            // Fly it into nest

            var bit = Instantiate(ShiningBitDisplay, player.transform.position, Quaternion.identity) as GameObject;

            var seq = DOTween.Sequence()
                .Append(bit.transform.DOMove(transform.position.SetY(transform.position.y + 5f), .5f))
                .Append(bit.transform.DOMove(transform.position, .5f))
                .OnComplete(() =>
                {
                    State.Instance.CashInShinyItem();

                    UpdateShinyDisplay();

                }).WaitForCompletion();

            amountToBank--;

            seqs.Add(seq);

            yield return new WaitForSeconds(Random.Range(0f, .2f));
        }

        foreach (var y in seqs)
        {
            yield return y;
        }


        var mileStones = Milestones.Where(m => m.Amount <= State.Instance.StoredShinyCount).ToList();
        foreach (var mileStone in mileStones)
        {
            Debug.Log(mileStone);
            Milestones.Remove(mileStone);

            // Fade Out
            // Spawn birds for mileStone
            yield return SceneFadeInOut.Instance.EndScene();

            var birdSpawn = FindObjectsOfType<BirdSpawn>().FirstOrDefault(b => b.Id == mileStone.Id);
            if (birdSpawn == null)
            {
                Debug.LogError("Expected BirdSpawn With Id '" + mileStone.Amount +"'");
            }

            birdSpawn.SpawnBird();

            yield return new WaitForSeconds(.25f);

            yield return SceneFadeInOut.Instance.StartScene();
            // Fade In
        }


        _cashInRoutine = null;
        // Shoot all the collectables into the nest
        yield return null;
    }

}
