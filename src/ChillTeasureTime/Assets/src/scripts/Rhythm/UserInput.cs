using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UserInput : MonoBehaviour
{
    public event ButtonPressed XButtonPressed;

    private bool buttonDown = false;
    [UsedImplicitly]
	void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.X) && !buttonDown)
        {
            OnXButtonPressed();
            buttonDown = true;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            buttonDown = false;
        }
    }

    protected virtual void OnXButtonPressed()
    {
        var handler = XButtonPressed;
        if (handler != null) handler(this);
    }
}

public delegate void ButtonPressed(object sender);