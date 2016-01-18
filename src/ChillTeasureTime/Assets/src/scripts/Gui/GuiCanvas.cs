using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GuiCanvas : Singleton<GuiCanvas>
{
    private GameObject OverworldUi;
    private GameObject IntroUi;

    private ChoiceUi _choiceUi;
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

    public ChoiceUi ChoiceUi
    {
        get
        {
            if (_choiceUi == null)
            {
                _choiceUi = transform.FindChild("ChoiceUi").gameObject.GetComponent<ChoiceUi>();
            }
            return _choiceUi;
        }
    }

    void Start()
    {
        OverworldUi = transform.FindChild("OverworldUi").gameObject;
        TalkingUiObject = transform.FindChild("TalkingUi").gameObject;
        IntroUi = transform.FindChild("IntroUi").gameObject;

        EnableTitleCard();

        //EnableOverworldUi();
    }

    private void EnableTitleCard()
    {
        DisableAll();
        IntroUi.SetActive(true);
    }

    public void FadeTitleCard()
    {
        IntroUi.SetActive(false);
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
        IntroUi.SetActive(false);
        OverworldUi.SetActive(false);
        TalkingUiObject.SetActive(false);
        ChoiceUi.gameObject.SetActive(false);
    }

    // Returns all the children of introUi
    public List<GameObject> GetTitleCards()
    {
        // This is stupid
        return transform
            .FindChild("IntroUi")
            .transform
            .Cast<Transform>()
            .Select(t => t.gameObject)
            .ToList();
    }

    public void EnableChoice()
    {
        ChoiceUi.gameObject.SetActive(true);
    }

    public void DisableChoice()
    {
        ChoiceUi.gameObject.SetActive(false);
    }
}
