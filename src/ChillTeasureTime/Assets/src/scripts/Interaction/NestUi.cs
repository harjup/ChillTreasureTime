using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class NestUi : MonoBehaviour
{

    private string _textTemplate;
    private Text _text;

    // Use this for initialization
    void Start()
    {
        _text = GetComponentsInChildren<Text>().First();
        _textTemplate = _text.text;
    }

    // Update is called once per frame
    void Update()
    {
        var storedCount = State.Instance.StoredShinyCount;


        _text.text = string.Format(_textTemplate, storedCount > 0 ? storedCount.ToString() : "");
    }
}
