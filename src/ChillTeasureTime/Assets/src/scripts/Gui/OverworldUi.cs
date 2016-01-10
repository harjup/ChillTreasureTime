using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class OverworldUi : MonoBehaviour
{
    private string _shinyTextTemplate;
    private Text _shinyText;
    private State _state;

	// Use this for initialization
	void Start ()
	{
	    _state = State.Instance;
	    _shinyText = GetComponentsInChildren<Text>().First(c => c.name == "ShinyText");
	    _shinyTextTemplate = _shinyText.text;
	}
	
	// Update is called once per frame
	void Update ()
	{
        _shinyText.text = string.Format(_shinyTextTemplate, _state.PlayerShinyCount);
	}
}
