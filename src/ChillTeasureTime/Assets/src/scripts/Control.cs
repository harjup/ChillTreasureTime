using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Control : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    public float BaseSpeed = 8;

    private Animator _animator;

    public List<IExaminable> CurrentExaminables = new List<IExaminable>();

    private bool _disabled;

    public readonly List<Action> MovementCallbacks = new List<Action>();

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
	    _animator = GetComponentInChildren<Animator>();
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
                StartCoroutine(first.StartSequence(() =>
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

	    if (_rigidbody.velocity.SetY(0f).magnitude > .01f)
	    {
	        foreach (var cb in MovementCallbacks)
	        {
	            cb();
	        }

	        _animator.SetTrigger("ToWalk");
	    }
	    else
	    {
            _animator.SetTrigger("ToIdle");
	    }

	    if (xVel > 0)
	    {
	        _animator.transform.localScale = _animator.transform.localScale.SetX(1);
	    }
	    else if (xVel < 0)
	    {
            _animator.transform.localScale = _animator.transform.localScale.SetX(-1);
	    }

	}
}
