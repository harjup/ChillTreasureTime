using System;
using UnityEngine;
using System.Collections;

public class ChoiceUi : MonoBehaviour
{
    private GameObject _topArrow;
    private GameObject _bottomArrow;

    private bool _selectedOption = false;

    void Start()
    {
        _topArrow = transform.Find("TopArrow").gameObject;
        _bottomArrow = transform.Find("BottomArrow").gameObject;
    }

    public IEnumerator WaitForPlayerChoice(Action<bool> callback)
    {
        _selectedOption = false;
        UpdateOptions(_selectedOption);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.S)
                || Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedOption = !_selectedOption;
                UpdateOptions(_selectedOption);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                callback(_selectedOption);
                yield break;
            }

            yield return null;
        }
    }

    void UpdateOptions(bool value)
    {
        if (value)
        {
            _topArrow.SetActive(false);
            _bottomArrow.SetActive(true);
        }

        if (!value)
        {
            _topArrow.SetActive(true);
            _bottomArrow.SetActive(false);
        }
    }
}
