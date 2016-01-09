using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{

    private Rigidbody _rigidbody;
    public float BaseSpeed = 20f; 

	// Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        var xVel = Input.GetAxisRaw("Horizontal") * BaseSpeed;
        var zVel = Input.GetAxisRaw("Vertical") * BaseSpeed;

        _rigidbody.velocity = new Vector3(xVel, _rigidbody.velocity.y, zVel);
	}
}
