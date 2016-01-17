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
    public GameObject CutsceneBird;

    public GameObject OtherBirdStart;
    public GameObject OtherBirdEnd;


    public List<GameObject> ShinyBits = new List<GameObject>();

    public GameObject CanDepositObject;

    public List<Milestone> Milestones = new List<Milestone>
    {
        new Milestone(1, null, new List<Line>{new Line("Helpful Birdly", "Ah. Hello! Another straggler! Staying warm in this cold weather?"), new Line("Helpful Birdly", "Most other birds have already gone south for the winter."), new Line("Helpful Birdly", "That's a nice bauble you found there. With enough of those, you could attract a bird-crew to head south with you.")}),

        new Milestone(5, null, new List<Line>{new Line("Winston \"Collecto\"", "Hmm. I see you found a paltry sum of shiny objects."), new Line("Winston \"Collecto\"", "UNFORTUNATELY. YOU WILL BE NO MATCH FOR MY COOL COLLECTION!"), /*TODO Show many baubles on person. Open wings.*/ new Line("Winston \"Collecto\"", "Have fun. Don't overwork yourself too much, bird.")}),
        new Milestone(5, null, new List<Line>{new Line("Helpful Birdly", "Hey bird-dude, I saw Winston talkin to you."), new Line("Helpful Birdly", "You know, as you gain shiny stuff, you're going to attract more attention. To keep up, you'll need some tricks."), new GetWingFlap(), new Line("Helpful Birdly", "If you press X, you can flap your wings and blow all sorts of stuff around! Try it on plants or sand piles!")})
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
           FindObjectOfType<Player>().DisableControl();
            // Fade Out
            // Spawn birds for mileStone
            yield return SceneFadeInOut.Instance.EndScene();

            var camMove = FindObjectOfType<CameraMove>();
            camMove.Pause = true;

            foreach (var mileStone in mileStones)
            {
                Milestones.Remove(mileStone);

                if (mileStone.Id != null)
                {
                    var birdSpawn = FindObjectsOfType<BirdSpawn>().FirstOrDefault(b => b.Id == mileStone.Id);
                    if (birdSpawn == null)
                    {
                        Debug.LogError("Expected BirdSpawn With Id '" + mileStone.Amount + "'");
                    }

                    birdSpawn.SpawnBird();
                }
                
                if (mileStone.Lines.Any())
                {
                    camMove.SetCenterPosition(GameObject.Find("CutsceneCameraFocus").transform.position);

                    FindObjectOfType<Player>().transform.position =
                        GameObject.Find("CutscenePlayerBirdPosition").transform.position;

                    yield return SceneFadeInOut.Instance.StartScene();

                    // Spawn bird of correct type on off-cam
                    var go = Instantiate(CutsceneBird, OtherBirdStart.transform.position, Quaternion.identity) as GameObject;
                    var bird = go.GetComponent<CutsceneBird>();

                    yield return StartCoroutine(bird.WalkToTarget(OtherBirdEnd.transform.position));
                    // Walk bird from off-cam to on cam
                    
                    yield return StartCoroutine(DialogService.Instance.DisplayLines(mileStone.Lines));
                    yield return StartCoroutine(bird.WalkToTarget(OtherBirdStart.transform.position));
                    // Walk bird back off cam
                    Destroy(go);

                    yield return SceneFadeInOut.Instance.EndScene();
                }
            }

            camMove.Pause = false;
            yield return new WaitForSeconds(.25f);

            yield return SceneFadeInOut.Instance.StartScene();
            // Fade In
            FindObjectOfType<Player>().EnableControl();
        }

        _cashInRoutine = null;
        // Shoot all the collectables into the nest
        yield return null;
    }

}
