using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Control : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    public float BaseSpeed = 8;

    public List<Signpost> CurrentExaminables = new List<Signpost>();

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
            _collider.enabled = !_disabled;
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
	    if (Disabled)
	    {
	        return;
	    }

        //TODO: Limit to when you're on the ground
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (CurrentExaminables.Any())
            {
                // TODO: Take closest
                var first = CurrentExaminables.First();                
                StartCoroutine(first.DisplayText(() =>
                {
                    Disabled = false;
                }));

                Disabled = true;
                _rigidbody.velocity = Vector3.zero;
                return;
            }
            // Start text mode
        }

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
