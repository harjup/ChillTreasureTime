﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextCrawler : MonoBehaviour
{
    private const int MaxPageLength = 200;
    private string _fullDisplayText;
    private string _currentDisplayText;
    private int _displayIndex = 1;
    private bool _inProcess;

    public string CurrentText;

    private IEnumerator _textDisplayRoutine = null;

    private Action<string> _textCallBack;

    public void Start()
    {
        CurrentText = "";
    }

    public IEnumerator TextCrawl(string text, Action<string> textCallBack)
    {
        InitText(text);

        _textCallBack = textCallBack;

        _textDisplayRoutine = CrawlText();
        yield return StartCoroutine(CrawlText());
        _textDisplayRoutine = null;
    }

    private void InitText(string text)
    {
        _fullDisplayText = text;
        _currentDisplayText = "";
        _displayIndex = 0;

        if (_textDisplayRoutine != null)
        {
            StopCoroutine(_textDisplayRoutine);
        }
    }


    IEnumerator CrawlText()
    {
        _inProcess = true;

        while (_displayIndex <= _fullDisplayText.Length)
        {
            _currentDisplayText = _fullDisplayText.Substring(0, _displayIndex);
            _displayIndex += 1;

            _textCallBack(_currentDisplayText);

            yield return new WaitForSeconds(.025f);
        }

        _textCallBack(_currentDisplayText);

        _inProcess = false;
    }

    public void SkipToEnd()
    {
        if (_fullDisplayText != null)
        {
            _displayIndex = _fullDisplayText.Length;
        }

        /*if (_textDisplayRoutine != null)
        {
            StopCoroutine(_textDisplayRoutine);
        }

        _textCallBack(_fullDisplayText);*/
    }
}
