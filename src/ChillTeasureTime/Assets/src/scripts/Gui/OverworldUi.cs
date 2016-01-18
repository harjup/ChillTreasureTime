using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class OverworldUi : MonoBehaviour
{
    private string _shinyTextTemplate;
    private Text _shinyText;

    private Text _trashText;
    private Image _trashImage;

    private State _state;

    private GameObject _itemCounts;

	// Use this for initialization
	void Start ()
	{
	    _state = State.Instance;
	    _shinyText = GetComponentsInChildren<Text>().First(c => c.name == "ShinyText");
        _trashText = GetComponentsInChildren<Text>().First(c => c.name == "TrashText");
        _trashImage = GetComponentsInChildren<Image>().First(c => c.name == "TrashImage");

	    _shinyTextTemplate = _shinyText.text;

        _itemCounts = transform.Find("ItemCounts").gameObject;

        _trashText.gameObject.SetActive(false);
        _trashImage.gameObject.SetActive(false);
	}

    private Dictionary<CollectableType, int> prevShinyCount = new Dictionary<CollectableType, int>{{CollectableType.Good, 0}, {CollectableType.Worthless, 0}};
    private bool _slidingBack = false;
	// Update is called once per frame
	void Update ()
	{
	    var newShineyCount = _state.PlayerShinyCount[CollectableType.Good] 
                             != prevShinyCount[CollectableType.Good];

        var newTrashCount = _state.PlayerShinyCount[CollectableType.Worthless]
                            != prevShinyCount[CollectableType.Worthless] 
                            && _state.PlayerShinyCount[CollectableType.Worthless] > 0;

        // This got gnarlier than expected
        if (newShineyCount || newTrashCount)
	    {

	        if (newTrashCount)
	        {
                _trashText.gameObject.SetActive(true);
                _trashImage.gameObject.SetActive(true);
	        }
            
            // Reset timer, weh at manipulating globals
            _timeElapsed = 0f;
            if (_itemCounts.transform.position.x <= 240f || _slidingBack)
            {
                _itemCounts.transform.DOKill();
                _itemCounts.transform.DOMoveX(460f, 1f).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    StartCoroutine(WaitThenCallback(() =>
                    {
                        _slidingBack = true;
                        _itemCounts.transform.DOMoveX(240f, .5f).OnComplete(() =>
                        {
                            _slidingBack = false;
                            _trashText.gameObject.SetActive(false);
                            _trashImage.gameObject.SetActive(false);
                        });
                    }));
                    // Wait for 5 uninterupted seconds and then slide out
                });
	        }

            // Sure wish object assignment used copies instead of references
	        foreach (var i in _state.PlayerShinyCount)
	        {
	            prevShinyCount[i.Key] = i.Value;
	        }
	    }

	    if (_shinyText.IsActive())
	    {
            _shinyText.text = string.Format(_shinyTextTemplate, _state.PlayerShinyCount[CollectableType.Good]);
	    }

        if (_trashText.IsActive())
        {
            _trashText.text = string.Format(_shinyTextTemplate, _state.PlayerShinyCount[CollectableType.Worthless]);
        }
	}

    private float _timeElapsed = 0f;
    IEnumerator WaitThenCallback(Action cb)
    {
        while (_timeElapsed < 5f)
        {
            _timeElapsed += Time.smoothDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        cb();
        _timeElapsed = 0f;
    }

    
}
