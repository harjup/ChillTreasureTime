using UnityEngine;

public class TransitionTab : MonoBehaviour
{
    public LevelEntrance TargetLevelEntrance;

    public GameObject WalkTarget;
    public GameObject EnterTarget;

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Transition to " + TargetLevelEntrance);
            player.StartLevelTransitionOut(WalkTarget.transform, TargetLevelEntrance);
        }
    }
}
