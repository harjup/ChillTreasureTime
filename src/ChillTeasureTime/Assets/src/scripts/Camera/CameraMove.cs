﻿using System;
using UnityEngine;

// This script requires iTween because DoTween doesn't have a float update equivalent if I remember correctly
public class CameraMove : MonoBehaviour
{
    // How quickly the camera accelerates to keep up with the player.
    // Keep it under 1f.
    public float cameraSnapX = .1f;
    public float cameraSnapY = .9f;
    public float cameraSnapZ = .4f;

    // Distance the player can move away from the camera center before it starts trying to recenter.
    public float cameraFreeX = 2.5f;
    public float cameraFreeZ = 1.25f;

    public GameObject Player;
    public GameObject cameraCenter;

    public bool Pause { get; set; }

    private Vector3 _initialPosition;

    public void Start()
    {
        _initialPosition = transform.localPosition;

        Pause = false;
        Player = FindObjectOfType<Player>().gameObject;
    }

    public void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Fight")
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
            GetComponent<GUILayer>().enabled = false;
            GetComponent<FlareLayer>().enabled = false;
        }
        else
        {
            GetComponent<Camera>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
            GetComponent<GUILayer>().enabled = true;
            GetComponent<FlareLayer>().enabled = true;
        }
        
    }

	private void FixedUpdate () 
    {
	    if (Pause)
	    {
	        return;
	    }

        MoveCamera(cameraCenter.transform, Player.transform);
	}

    public void MoveCamera(Transform cameraTransform, Transform targetTransform)
    {
        // TODO: Maybe let's use DoTween and have it be reasonably smooth instead of falling on our old iTween business
        var positionDifference = targetTransform.position - cameraTransform.position;
        float xSpeed = Mathf.Abs(positionDifference.x) * cameraSnapX;
        //float ySpeed = Mathf.Abs(positionDifference.y) * cameraSnapY;
        float zSpeed = Mathf.Abs(positionDifference.z) * cameraSnapZ;

        //Cap the camera's speed so it doesn't go fucking nuts and start overshooting the player
        if (xSpeed > 60) { xSpeed = 60; }
        if (zSpeed > 60) { zSpeed = 60; }

        if (Mathf.Abs(positionDifference.x) >= cameraFreeX)
        {
            cameraTransform.position = cameraTransform.position.SetX(iTween.FloatUpdate(cameraTransform.position.x, targetTransform.position.x, xSpeed));
        }
        if (Mathf.Abs(positionDifference.z) >= cameraFreeZ)
        {
            cameraTransform.position = cameraTransform.position.SetZ(iTween.FloatUpdate(cameraTransform.position.z, targetTransform.position.z, zSpeed));
        }

        //TODO: Generalize in some way
        if (targetTransform.GetComponentInChildren<GroundCheck>() != null 
            && targetTransform.GetComponentInChildren<GroundCheck>().IsOnGround
            && !State.Instance.IsIntro)
        {
            cameraTransform.position = cameraTransform.position.SetY(iTween.FloatUpdate(cameraTransform.position.y, targetTransform.position.y, 5f));
        }
    }

    public void ResetPosition()
    {
        transform.localPosition = _initialPosition;
    }

    public void SetCenterPosition(Vector3 position)
    {
        cameraCenter.transform.position = position;
    }
}
