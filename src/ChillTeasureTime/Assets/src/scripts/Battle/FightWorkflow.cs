using System;
using UnityEngine;
using System.Collections;

public class FightWorkflow : MonoBehaviour
{
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
