using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class TalkingUi : MonoBehaviour
{
    private Text _contentText;
    private Text _nameText; 
    private TextCrawler _textCrawler;
    private GameObject _doneImage;

    

    public void Awake()
    {
        var textComponents = GetComponentsInChildren<Text>();
        _contentText = textComponents.First(c => c.name == "ContentText");
        _nameText = textComponents.First(c => c.name == "NameText");
        _textCrawler = GetComponent<TextCrawler>();
        _doneImage = transform.FindChild("DoneImage").gameObject;

        _doneImage.SetActive(false);
    }

    public IEnumerator TextCrawl(Line line, Action doneCb = null)
    {
        _nameText.text = line.Name;
        yield return StartCoroutine(_textCrawler.TextCrawl(line.Content, (s) => _contentText.text = s));
        _doneImage.SetActive(true);

        if (doneCb != null)
        {
            doneCb();
        }

        while (!Input.GetKeyDown(KeyCode.X))
        {
            yield return null;
        }

        _doneImage.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _textCrawler.SkipToEnd();
        }
    }
}
