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


    public RuntimeAnimatorController GreenAnimatorController;
    public RuntimeAnimatorController RedAnimatorController;

    public List<GameObject> ShinyBits = new List<GameObject>();

    public GameObject CanDepositObject;

    // We want our list to persist over time
    public static List<Milestone> Milestones = new List<Milestone>
    {
        new Milestone(1, birdType: BirdType.Green, directions: new List<Direction>{new Line("Rocko", "Ah. Hello! Another straggler! Staying warm in this cold weather?"), new Line("Rocko", "Most other birds have already gone south for the winter."), new Line("Rocko", "That's a nice bauble you found there. With enough of those, you could attract a bird-crew to head south with you.")}),
        new Milestone(1, "Main-JumpHelper"),
        new Milestone(1, "Main-Observer"),

        new Milestone(5, birdType: BirdType.Red, directions:  new List<Direction>{new Line("Winston \"Collecto\"", "Hmm. I see you found a paltry sum of shiny objects."), new Line("Winston \"Collecto\"", "UNFORTUNATELY. YOU WILL BE NO MATCH FOR MY COOL COLLECTION!"), /*TODO Show many baubles on person. Open wings.*/ new Line("Winston \"Collecto\"", "Have fun. Don't overwork yourself too much, bird.")}),
        new Milestone(5, directions:  new List<Direction>{new Line("Pokey", "Hey bird-dude, I saw Winston talkin to you."), new Line("Pokey", "You know, as you gain shiny stuff, you're going to attract more attention. To keep up, you'll need some tricks."), new GetWingFlap(), new Line("Pokey", "If you press X, you can flap your wings and blow all sorts of stuff around! Try it on plants or sand piles!")}),
        new Milestone(5, "Main-FlapHelp"),

        new Milestone(10, "Main-Mayor", directions: new List<Direction>{new Line("Mayor Brachie", "Hello, I am the bird-mayor of the nearby town. I am going around looking for birds that still need a group to head down south."), new Line("Mayor Brachie", "Come talk to me if you'd like to head out. I'll be over near the water.")}),

        new Milestone(20, "Rival", BirdType.Red, new List<Direction>{new Line("Winston \"Collecto\"", "Wow, that's quite a collection! Almost as good as mine."), new Line("Winston \"Collecto\"", "If you really wanna impress me, meet me by the beach shore and you can show me your moves!")})
    };

    public static List<Milestone> ExecutedMilestones = new List<Milestone>(); 

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

        foreach (var mileStone in ExecutedMilestones)
        {
            if (mileStone.Id != null)
            {
                var birdSpawn = FindObjectsOfType<BirdSpawn>().FirstOrDefault(b => b.Id == mileStone.Id);
                if (birdSpawn == null)
                {
                    Debug.LogError("Expected BirdSpawn With Id '" + mileStone.Amount + "'");
                    continue;
                }

                birdSpawn.SpawnBird();
            }
        }
    }

    public void Update()
    {
        CanDepositObject.SetActive(State.Instance.PlayerShinyCount[CollectableType.Good] > 0);
    }

    public void UpdateShinyDisplay()
    {
        var shinyBitsTransform = transform
          .FindChild("ShinyBits");

        foreach (Transform t in shinyBitsTransform)
        {
            t.gameObject.SetActive(false);
            var amount = int.Parse(t.name);
            if (amount <= State.Instance.StoredShinyCount)
            {
                t.gameObject.SetActive(true);
            }
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
                ExecutedMilestones.Add(mileStone);
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
                
                if (mileStone.Directions.Any())
                {
                    camMove.SetCenterPosition(GameObject.Find("CutsceneCameraFocus").transform.position);

                    FindObjectOfType<Player>().transform.position =
                        GameObject.Find("CutscenePlayerBirdPosition").transform.position;

                    FindObjectOfType<Player>().FaceRight();

                    yield return SceneFadeInOut.Instance.StartScene();



                    // Spawn bird of correct type on off-cam
                    var go = Instantiate(CutsceneBird, OtherBirdStart.transform.position, Quaternion.identity) as GameObject;
                    var bird = go.GetComponent<CutsceneBird>();

                    if (mileStone.BirdType == BirdType.Green)
                    {
                        bird.GetComponent<Animator>().runtimeAnimatorController = GreenAnimatorController;
                    }
                    if (mileStone.BirdType == BirdType.Red)
                    {
                        bird.GetComponent<Animator>().runtimeAnimatorController = RedAnimatorController;
                    }
   
                    yield return StartCoroutine(bird.WalkToTarget(OtherBirdEnd.transform.position));
                    // Walk bird from off-cam to on cam
                    
                    yield return StartCoroutine(DialogService.Instance.DisplayDirections(mileStone.Directions, () => {bird.Talk();}, () => {bird.Idle();}));
                    yield return StartCoroutine(bird.WalkToTarget(OtherBirdStart.transform.position));
                    // Walk bird back off cam
                    Destroy(go);

                    //yield return SceneFadeInOut.Instance.EndScene();
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
