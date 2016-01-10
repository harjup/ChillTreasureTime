using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Signpost : MonoBehaviour
{
    private GuiCanvas _guiCanvas;
    private TalkingUi _talkingUi;
    private GameObject QMark;

    private List<Line> _lines = new List<Line>()
    {
        new Line("Signpost", "I AM A BEAUTIFUL SIGNPOST THAT IS ALL"),
        new Line("Signpost", "ALSO, I AM MADE OUT OF WOOD"),
        new Line("Signpost", "ALRIGHT, I HAD A LITTLE EXTRA")
    };

    public void Start()
    {
        _guiCanvas = GuiCanvas.Instance;
        _talkingUi = _guiCanvas.TalkingUi;

        QMark = transform.FindChild("?").gameObject;
        QMark.SetActive(false);
    }

    public IEnumerator DisplayText(Action doneCallback)
    {
        QMark.SetActive(false);
        _guiCanvas.EnableTalking();

        foreach (var line in _lines)
        {
            yield return StartCoroutine(_talkingUi.TextCrawl(line));
        }
        
        _guiCanvas.EnableOverworldUi();
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
