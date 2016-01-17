using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Control _control;

    private bool _isIntro;

    private Action setMovingGui;


    void Start()
    {
        setMovingGui = () =>
        {
            if (_isIntro)
            {
                _isIntro = false;
                GuiCanvas.Instance.FadeTitleCard();
            }
        };

        _isIntro = true;
        _control = GetComponent<Control>();
        _control.MovementCallbacks.Add(setMovingGui);
        PutPlayerInCorrectStartSpot();
    }

    public void OnLevelWasLoaded(int level)
    {
        _control.CurrentExaminables = new List<IExaminable>();

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

        if (Application.loadedLevelName == "Fight" || State.Instance.IsIntro)
        {
            _control.Disabled = true;
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
            .OnComplete(() => { StartCoroutine(LoadLevelAfterImportantEventsAreDone(level)); });
        // Start a timer
        // Load level once camera fade occurs
    }

    private IEnumerator LoadLevelAfterImportantEventsAreDone(LevelEntrance level)
    {
        while (!State.Instance.AllImportantSequencesAreDone())
        {
            yield return new WaitForEndOfFrame();
        }

        LevelLoader.Instance.LoadLevel(level);
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
        transform.DOMove(tab.EnterTarget.transform.position, .5f).OnComplete(() =>
        {
            _control.Disabled = false;
        });
        
        // Pull info off the door or tab for what our animation should be
    }

    public void AddExaminable(IExaminable signpost)
    {
        if (!_control.CurrentExaminables.Contains(signpost))
        {
            _control.CurrentExaminables.Add(signpost);
        }
    }

    public void RemoveExaminable(IExaminable signpost)
    {
        if (_control.CurrentExaminables.Contains(signpost))
        {
            _control.CurrentExaminables.Remove(signpost);
        }
    }

    public void DisableControl()
    {
        GetControlComponent().Disabled = true;
    }

    public void EnableControl()
    {
        GetControlComponent().Disabled = false;
    }

    private Control GetControlComponent()
    {
        if (_control == null)
        {
            _control = GetComponent<Control>();
        }

        return _control;
    }

    public void AddInteractable(CanInteract canInteract)
    {
        if (!_control.CurrentInteractables.Contains(canInteract))
        {
            _control.CurrentInteractables.Add(canInteract);
        }

    }

    public void RemoveInteractable(CanInteract canInteract)
    {
        if (!_control.CurrentInteractables.Contains(canInteract))
        {
            _control.CurrentInteractables.Add(canInteract);
        }
    }
}
