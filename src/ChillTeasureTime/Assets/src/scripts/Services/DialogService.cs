using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogService : Singleton<DialogService>
{

    private GuiCanvas _guiCanvas;

    // Use this for initialization
    void Start()
    {
        _guiCanvas = GuiCanvas.Instance;
    }

    public IEnumerator DisplayLines(List<Line> lines)
    {
        _guiCanvas.EnableTalking();

        foreach (var line in lines)
        {
            yield return StartCoroutine(_guiCanvas.TalkingUi.TextCrawl(line));
        }

        _guiCanvas.EnableOverworldUi();
    }

    public IEnumerator DisplayDirections(List<Direction> directions, Action startTalkCb = null, Action endTalkCb = null)
    {
        _guiCanvas.EnableTalking();

        foreach (var direction in directions)
        {
            if (startTalkCb != null)
            {
                startTalkCb();
            }

            if (direction is Line)
            {
                yield return StartCoroutine(_guiCanvas.TalkingUi.TextCrawl(direction as Line, endTalkCb));
            }

            if (endTalkCb != null)
            {
                endTalkCb();
            }

            if (direction is GetWingFlap)
            {
                State.Instance.UnlockWingFlap();
            }
        }
 
        _guiCanvas.EnableOverworldUi();
    }
}
