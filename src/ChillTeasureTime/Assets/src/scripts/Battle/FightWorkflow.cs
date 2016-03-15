using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FightWorkflow : MonoBehaviour
{
    public GameObject Player;
    public string CurrentPlayerMove;

    private FightMenu _fightMenu;

    private FightEnemyManager _fightEnemyManager;

    private bool _listenForJumpInput = false;
    private bool _inputHit = false;

    public float WalkUpTime = .5f;
    public float PauseTime = .125f;
    public float FirstJumpTime = .5f;
    public float SecondJumpTime = .5f;
    public float JumpOffTime = .25f;


    public void InitAttackSequence(GameObject target, string currentMove)
    {
        var initialPosition = Player.transform.position;

        // Let's do a jump attack
        // 1. Walk 
        var enemyPosition = target.transform.position;

        var playerHeight = 1.2f;
        var playerWidth = 1f;
        

        //TODO: Specify where the "head" is on each enemy. Maybe assume body if no head.
        var enemyHead = target.transform.position;
        var stopPosition = enemyHead.SetY(Player.transform.position.y).SetX(enemyHead.x - (playerWidth * 3));

        DOTween
            .Sequence()
            .Append(Player.transform.DOMove(stopPosition, WalkUpTime).SetEase(Ease.Linear))
            .AppendInterval(PauseTime)
            .Append(Player.transform.DOJump(enemyHead, playerHeight * 2f, 1, FirstJumpTime).SetEase(Ease.Linear))
            .InsertCallback(WalkUpTime + FirstJumpTime, () =>
            {
                _listenForJumpInput = true;
            })
            .InsertCallback(WalkUpTime + PauseTime + FirstJumpTime, () =>
            {
                _listenForJumpInput = false;
                target.GetComponent<CharacterStatus>().AddDamage(1);
                if (_inputHit)
                {
                    _inputHit = false;

                    DOTween
                        .Sequence()
                        .Append(Player.transform.DOJump(enemyHead, playerHeight * 2f, 1, SecondJumpTime).SetEase(Ease.Linear))
                        .AppendCallback(() =>
                        {
                            target.GetComponent<CharacterStatus>().AddDamage(1);
                        })
                        .Append(Player.transform.DOJump(stopPosition, playerHeight, 1, JumpOffTime).SetEase(Ease.Linear))
                        .Append(Player.transform.DOMove(initialPosition, 1f).SetEase(Ease.Linear))
                        .AppendCallback(() =>
                        {
                            // Lets ignore the enemy turns for now.
                            // StartCoroutine(_fightEnemyManager.DoEnemyTurns());
                            _fightMenu.Enable();
                        });
                }
                else
                {
                    DOTween
                      .Sequence()
                      .Append(Player.transform.DOJump(stopPosition, playerHeight, 1, JumpOffTime).SetEase(Ease.Linear))
                      .Append(Player.transform.DOMove(initialPosition, 1f).SetEase(Ease.Linear))
                      .AppendCallback(() =>
                      {
                          // Lets ignore the enemy turns for now.
                          // StartCoroutine(_fightEnemyManager.DoEnemyTurns());
                          _fightMenu.Enable();
                      });
                }
            });

        



    }     

    // Play intro
    // Set up player, enemies
    //- Show player menu
    //  Moves (image, description)
    //  MoveDetails (image, name, description)
    //  Target (enemy list, validate against which types are attackable)
    //  Animation (target, which move)
    //- Enemy Animation (for each enemy)
    //   Pick what to do
    //   Do Animation
    // Outro


    // PlayerFightWorkflow

    //



    void Start()
    {
        Player = GameObject.Find("BirdSprite");
        _fightMenu = FindObjectOfType<FightMenu>();
        _fightEnemyManager = FindObjectOfType<FightEnemyManager>();
    }

    void Update()
    {
        if (_listenForJumpInput && Input.GetKey(KeyCode.X))
        {
            _inputHit = true;
        }
    }
}

public class Intro
{
    public IEnumerator DoIntro(Action callback)
    {
        Debug.Log("Do the intro");

        callback();

        yield return null;
    }
}


public class PlayerFightWorkflow
{
    public IEnumerator DoMenuSelect()
    {
        Debug.Log("Do the menu select");

        // Activate/Draw FightMenu, wait for yield
        // callback with result should get called

        yield return null;
    }

    public IEnumerator DoMenuDetailSelect(FightMenuItem.Kind kind)
    {
        // Activate/Draw FightDetailMenu, wait for yield
        // result is either go back or 



        yield return null;
    }
}