﻿using UnityEngine;
using System.Collections;

public class GuiCanvas : Singleton<GuiCanvas>
{
    private GameObject OverworldUi;
    private GameObject TitleCard;

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
        TitleCard = transform.FindChild("TitleCard").gameObject;

        EnableTitleCard();

        //EnableOverworldUi();
    }

    private void EnableTitleCard()
    {
        DisableAll();
        TitleCard.SetActive(true);
    }

    public void FadeTitleCard()
    {
        TitleCard.SetActive(false);
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
        TitleCard.SetActive(false);
        OverworldUi.SetActive(false);
        TalkingUiObject.SetActive(false);
    }
}
