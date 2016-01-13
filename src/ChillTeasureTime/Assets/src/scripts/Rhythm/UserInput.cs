using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UserInput : MonoBehaviour
{
    public event ButtonPressed XButtonPressed;

    [UsedImplicitly]
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnXButtonPressed();
        }
    }

    protected virtual void OnXButtonPressed()
    {
        var handler = XButtonPressed;
        if (handler != null) handler(this);
    }
}

public delegate void ButtonPressed(object sender);