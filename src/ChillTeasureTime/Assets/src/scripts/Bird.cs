using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bird : MonoBehaviour, IExaminable
{
    public string TextId;
    private GuiCanvas _guiCanvas;
    private TalkingUi _talkingUi;
    private GameObject QMark;

    public List<Direction> Directions;

    public void Start()
    {
        _guiCanvas = GuiCanvas.Instance;
        _talkingUi = _guiCanvas.TalkingUi;

        QMark = transform.FindChild("?").gameObject;
        QMark.SetActive(false);

        Directions = DirectionService.Instance.GetDirectionsById(TextId);
    }

    public IEnumerator StartSequence(Action doneCallback)
    {
        QMark.SetActive(false);
        _guiCanvas.EnableTalking();

        foreach (var direction in Directions)
        {
            if (direction is Line)
            {
                yield return StartCoroutine(_talkingUi.TextCrawl(direction as Line));
            }

            // Not currently used, won't get hit
            if (direction is GetWingFlap)
            {
                Debug.Log("Unlock wing flap");
                State.Instance.UnlockWingFlap();
            }

            if (direction is LoadFight)
            {
                doneCallback();

                _guiCanvas.DisableAll();

                SceneFadeInOut.Instance.EndScene();

                yield return new WaitForSeconds(.5f);

                LevelLoader.Instance.LoadFight();
            }

            if (direction is LeaveChoicePrompt)
            {
                _guiCanvas.EnableChoice();
                yield return new WaitForEndOfFrame();
                var result = false;
                yield return StartCoroutine(_guiCanvas.ChoiceUi.WaitForPlayerChoice(res =>
                {
                    result = res;
                }));
                _guiCanvas.DisableChoice();

                if (result)
                {
                    yield return StartCoroutine(_talkingUi.TextCrawl(new Line("Mayor Brachie", "Awesome. Let's go!")));
                    yield return SceneFadeInOut.Instance.EndScene();
                    Application.LoadLevel("End");
                }
                else
                {
                    yield return StartCoroutine(_talkingUi.TextCrawl(new Line("Mayor Brachie", "Alright, let me know.")));
                }
            }
        }

        doneCallback();
        _guiCanvas.EnableOverworldUi();
        QMark.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            QMark.SetActive(true);
            player.AddExaminable(this);
            Debug.Log("Enter " + other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            QMark.SetActive(false);
            player.RemoveExaminable(this);
            Debug.Log("Enter " + other);
        }
    }
}
