using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class OverworldUi : MonoBehaviour
{
    private string _shinyTextTemplate;
    private Text _shinyText;
    private State _state;

    private GameObject _itemCounts;

	// Use this for initialization
	void Start ()
	{
	    _state = State.Instance;
	    _shinyText = GetComponentsInChildren<Text>().First(c => c.name == "ShinyText");
	    _shinyTextTemplate = _shinyText.text;

        _itemCounts = transform.Find("ItemCounts").gameObject;
	}

    private int prevShinyCount;
    private bool _slidingBack = false;
	// Update is called once per frame
	void Update ()
	{
        // This got gnarlier than expected
	    if (prevShinyCount != _state.PlayerShinyCount)
	    {
            _timeElapsed = 0f;
            if (_itemCounts.transform.position.x <= 240f || _slidingBack)
            {
                // Reset timer, weh @ maipulating globals
                _itemCounts.transform.DOKill();
                _itemCounts.transform.DOMoveX(460f, 1f).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    StartCoroutine(WaitThenCallback(() =>
                    {
                        _slidingBack = true;
                        _itemCounts.transform.DOMoveX(240f, .5f).OnComplete(() =>
                        {
                            _slidingBack = false;
                        });
                    }));
                    // Wait for 5 uninterupted seconds and then slide out
                });
	        }

	        prevShinyCount = _state.PlayerShinyCount;
	    }

	    if (_shinyText.IsActive())
	    {
            _shinyText.text = string.Format(_shinyTextTemplate, _state.PlayerShinyCount);
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
