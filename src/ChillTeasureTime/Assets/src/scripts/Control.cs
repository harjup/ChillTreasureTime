using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    public float BaseSpeed = 10;

    private bool _disabled;

    public bool Disabled
    {
        get
        {
            return _disabled;
        }
        set
        {
            _disabled = value;
            GetComponent<Collider>().enabled = !_disabled;
        }
    }

    // Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        var xVel = Input.GetAxisRaw("Horizontal") * BaseSpeed;
        var zVel = Input.GetAxisRaw("Vertical") * BaseSpeed;

        var yVel = _rigidbody.velocity.y - (20 * Time.fixedDeltaTime);
	    
	    if (Input.GetKeyDown(KeyCode.Z) && yVel < 1f)
	    {
	        yVel = 10f;
	    }

        _rigidbody.velocity = new Vector3(xVel, yVel, zVel);

	}
}
