using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Control _control;

    void Start()
    {
        _control = GetComponent<Control>();
        PutPlayerInCorrectStartSpot();
    }

    public void OnLevelWasLoaded(int level)
    {
        PutPlayerInCorrectStartSpot();
    }

    public void PutPlayerInCorrectStartSpot()
    {
        var levelEntrance = State.Instance.LevelEntrance;
        var startSpot = FindObjectsOfType<PlayerStart>().FirstOrDefault(p => p.LevelEntrance == levelEntrance);
        if (startSpot == null)
        {
            Debug.LogError("Start Spot not found, resorting to default");
            startSpot = FindObjectsOfType<PlayerStart>().FirstOrDefault(p => p.LevelEntrance == LevelEntrance.Start);
        }

        StartLevelTransitionIn(startSpot);
    }

    public void StartLevelTransitionOut(Transform walkTarget, LevelEntrance level)
    {
        // Walk to target
        Debug.Log(string.Format("Walk to {0}", walkTarget.position));

        // Disable rigid body
        // Set animation to walking
        // Move toward walkTarget
        _control.Disabled = true;

        SceneFadeInOut.Instance.EndScene();

        transform
            .DOMove(walkTarget.position, .5f)
            .OnComplete(() => { LevelLoader.Instance.LoadLevel(level); });
        // Start a timer
        // Load level once camera fade occurs
    }

    public void StartLevelTransitionIn(PlayerStart start)
    {
        if (start.LevelEntrance == LevelEntrance.Start)
        {
            transform.position = start.transform.position;
            return;
        }

        var tab = start.gameObject.GetComponentInChildren<TransitionTab>();
        transform.position = tab.WalkTarget.transform.position;
        transform.DOMove(tab.EnterTarget.transform.position, .5f).OnComplete(() => { _control.Disabled = false; });
        
        // Pull info off the door or tab for what our animation should be



    }
}
