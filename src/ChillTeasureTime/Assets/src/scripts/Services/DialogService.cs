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
}
