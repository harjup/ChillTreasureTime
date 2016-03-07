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

    public void InitAttackSequence(GameObject target, string currentMove)
    {
        var initialPosition = Player.transform.position;

        DOTween
            .Sequence()
            .Append(Player.transform.DOMove(target.transform.position, 1f))
            .AppendCallback(() =>
            {
                // TODO: Tell the other guy they got hit
                //target.GetComponent<>()
            })
            .Append(Player.transform.DOMove(initialPosition, 1f))
            .AppendCallback(() =>
            {
                //_fightMenu.Enable();
                StartCoroutine(_fightEnemyManager.DoEnemyTurns());
                // Do the enemy turns here
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