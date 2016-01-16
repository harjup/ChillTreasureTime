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
        new Milestone(1, "1", new List<Line>{new Line("", "Some nearby birds were attracted by your newfound riches.")}),
        new Milestone(2, "2", new List<Line>{new Line("", "Here is another message for two shiny items")}),
        new Milestone(3, "3", new List<Line>{new Line("", "This message is for three.")})
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
        CanDepositObject.SetActive(State.Instance.PlayerShinyCount[CollectableType.Good] > 0);
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
        int amountToBank = State.Instance.PlayerShinyCount[CollectableType.Good];

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

        if (mileStones.Any())
        {
            // Fade Out
            // Spawn birds for mileStone
            yield return SceneFadeInOut.Instance.EndScene();

            foreach (var mileStone in mileStones)
            {
                Milestones.Remove(mileStone);

                var birdSpawn = FindObjectsOfType<BirdSpawn>().FirstOrDefault(b => b.Id == mileStone.Id);
                if (birdSpawn == null)
                {
                    Debug.LogError("Expected BirdSpawn With Id '" + mileStone.Amount + "'");
                }

                birdSpawn.SpawnBird();

                if (mileStone.Lines.Any())
                {
                    yield return StartCoroutine(DialogService.Instance.DisplayLines(mileStone.Lines));
                }
            }

            yield return new WaitForSeconds(.25f);

            yield return SceneFadeInOut.Instance.StartScene();
            // Fade In
        }

        


        _cashInRoutine = null;
        // Shoot all the collectables into the nest
        yield return null;
    }

}
