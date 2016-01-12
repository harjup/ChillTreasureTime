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

        Directions = new List<Direction>
        {
            new Line("Tiny Bird", "Nice Bauble You Have Here."),
            new Line("Tiny Bird", "You know, as you gain shiny stuff, you're going to attract more attention."),
            new Line("Tiny Bird", "To do that, you'll need some tricks. Here's WING FLAP."),
            new Line("Tiny Bird", "Try it on me. If you can impress me, I'll even join you!"),
            new LoadFight()
        };
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

            if (direction is LoadFight)
            {
                doneCallback();

                _guiCanvas.DisableAll();

                SceneFadeInOut.Instance.EndScene();

                yield return new WaitForSeconds(.5f);

                LevelLoader.Instance.LoadFight();
            }
        }

        doneCallback();
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
