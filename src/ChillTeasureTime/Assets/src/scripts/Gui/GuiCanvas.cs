using UnityEngine;
using System.Collections;

public class GuiCanvas : Singleton<GuiCanvas>
{
    private GameObject OverworldUi;
    
    private TalkingUi _talkingUi;
    public GameObject TalkingUiObject;

    public TalkingUi TalkingUi
    {
        get
        {
            if (_talkingUi == null)
            {
                _talkingUi = transform.FindChild("TalkingUi").gameObject.GetComponent<TalkingUi>();
            }
            return _talkingUi;
        } 
    }

    void Start()
    {
        OverworldUi = transform.FindChild("OverworldUi").gameObject;
        TalkingUiObject = transform.FindChild("TalkingUi").gameObject;

        EnableOverworldUi();
    }

    public void EnableTalking()
    {
        DisableAll();
        TalkingUiObject.SetActive(true);
    }

    public void EnableOverworldUi()
    {
        DisableAll();
        OverworldUi.SetActive(true);
    }

    public void DisableAll()
    {
        OverworldUi.SetActive(false);
        TalkingUiObject.SetActive(false);
    }
}
