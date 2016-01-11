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

    public List<Line> Lines;

    public void Start()
    {
        _guiCanvas = GuiCanvas.Instance;
        _talkingUi = _guiCanvas.TalkingUi;

        QMark = transform.FindChild("?").gameObject;
        QMark.SetActive(false);

        Lines = new List<Line>
        {
            new Line("Tiny Bird", "Nice Bauble You Have Here."),
            new Line("Tiny Bird", "You know, as you gain shiny stuff, you're going to attract more attention."),
            new Line("Tiny Bird", "To do that, you'll need some tricks. Here's WING FLAP."),
            new Line("Tiny Bird", "Try it on me. If you can impress me, I'll even join you!"),
        };
    }

    public IEnumerator StartSequence(Action doneCallback)
    {
        QMark.SetActive(false);
        _guiCanvas.EnableTalking();

        foreach (var line in Lines)
        {
            yield return StartCoroutine(_talkingUi.TextCrawl(line));
        }

        // TRANSITION INTO FIGHT INSTANCE

        Debug.Log("TRANSITION INTO FIGHT INSTANCE");
        
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
