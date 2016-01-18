using System;
using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour
{
    public bool IsOnGround { get; private set; }

    public void OnTriggerStay(Collider other)
    {
        var geometry = other.gameObject.GetComponent<LevelGeometry>();
        if (geometry != null)
        {
            IsOnGround = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var geometry = other.gameObject.GetComponent<LevelGeometry>();
        if (geometry != null)
        {
            IsOnGround = false;
        }
    }
}
