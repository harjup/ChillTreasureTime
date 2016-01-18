using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Control : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    private GroundCheck _groundCheck;

    public float BaseSpeed = 8;

    public bool OnGround;

    private Animator _animator;

    public List<IExaminable> CurrentExaminables = new List<IExaminable>();

    [SerializeField]
    private bool _disabled;

    public readonly List<Action> MovementCallbacks = new List<Action>();

    private bool _firstInputGiven = false;

    public bool Disabled
    {
        get
        {
            return _disabled;
        }
        set
        {
            _disabled = value;

            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }

            _collider.enabled = !_disabled;

            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }


            _rigidbody.velocity = Vector3.zero;
        }
    }

    public List<CanInteract> CurrentInteractables = new List<CanInteract>();

    // Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _animator = GetComponentInChildren<Animator>();
	    _groundCheck = GetComponentInChildren<GroundCheck>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Disabled)
	    {
	        return;
	    }

        //TODO: Limit to when you're on the ground
        if (Input.GetKeyDown(KeyCode.X) && _groundCheck.IsOnGround)
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
            else if (State.Instance.PowerList.Contains(Power.WingFlap))
            {
                _animator.SetTrigger("ToWingFlap");
                var firstBlow = CurrentInteractables.FirstOrDefault(i => i is CanBlow);
                if (firstBlow != null)
                {
                    StartCoroutine(firstBlow.DoSequence(() => { }));
                }

                var first = CurrentInteractables.FirstOrDefault(i => i is SandPile);
                if (first != null)
                {
                    StartCoroutine(first.DoSequence(() => { }));
                }

                // Play wing flap
                // Target Do animation / spawning
            }
        }

	    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("WingFlap"))
	    {
            _rigidbody.velocity = _rigidbody.velocity.SetX(0f).SetZ(0f);
	        return;
	    }



        var xVel = Input.GetAxisRaw("Horizontal") * BaseSpeed;
        var zVel = Input.GetAxisRaw("Vertical") * BaseSpeed;

        var yVel = _rigidbody.velocity.y - (20 * Time.fixedDeltaTime);
	    
	    //if (Input.GetKeyDown(KeyCode.Z) && yVel < 1f) // It'd be neat to have a number of flaps allowed before you have to touch ground
        if (Input.GetKeyDown(KeyCode.Z) && _groundCheck.IsOnGround)
	    {
	        yVel = 10f;
	    }

	  
        _rigidbody.velocity = new Vector3(xVel, yVel, zVel);

	    
        if (_groundCheck.IsOnGround)
        {
            if (_rigidbody.velocity.SetY(0f).magnitude > .1f)
            {
                foreach (var cb in MovementCallbacks)
                {
                    cb();
                    _firstInputGiven = true;
                }

                _animator.CrossFade("Walk", 0f);
            }
            else if (_firstInputGiven)
            {
                _animator.CrossFade("Idle", 0f);
            }
        }
        else if (_firstInputGiven)
        {
            if (_rigidbody.velocity.y > 0)
            {
                _animator.CrossFade("JumpUp", 0f);
            }
            else if (_rigidbody.velocity.y < 0)
            {
                _animator.CrossFade("JumpDown", 0f);
            }
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

    public void FaceRight()
    {
        _animator.transform.localScale = _animator.transform.localScale.SetX(1);
    }
}
