using System;
using UnityEngine;
using System.Collections;

public enum LevelEntrance
{
    Unknown,
    BeachLeft,
    BeachRight,
    Lighthouse,
    VilliageLeft
}

public class TransitionTab : MonoBehaviour
{
    public LevelEntrance TargetLevelEntrance;

    public GameObject WalkTarget;

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Transition to " + TargetLevelEntrance);
            player.StartLevelTransition(WalkTarget.transform);
            // mainCamera.StartLevelTransition();
        }
    }
}
