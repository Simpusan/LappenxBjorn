using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour {
	
	// Relativity variables
	private float _rotationSpeed = 400;
	public float playerSpeed = 5.0f;
	
	// Target variables
	public GunBehavior gunObject;
	public AudioSource gunStart, gunEnd;
	public GameObject minigun;
	private CharacterController _controller;
	private Quaternion _targetRotation;
	private Camera _camTarget;
	private Vector3 _mousePos;
		
	void Start () {
		_controller = GetComponent<CharacterController>();
		_camTarget = Camera.main;
	}
	
	void Update () {
		movementInput ();
		if(Input.GetButtonDown ("Switch")) {
			gunObject.switchWeapon();
		}

		if(gunObject.currentType == GunBehavior.weaponType.Gun) {
			if(Input.GetButtonDown("Shoot")) { 
				minigun.GetComponent<Animator>().enabled = true;
				gunStart.Play ();
			}
			else if(Input.GetButtonUp ("Shoot") && Input.GetAxis ("Shoot") > 0.5) { // Release shoot, but only after having it held enough.
				minigun.GetComponent<Animator>().enabled = false;
				gunEnd.Play ();
			}

			if(Input.GetAxis ("Shoot") == 1) {
				minigun.GetComponent<Animator>().enabled = true;
				gunObject.GenericShoot ();
			}
		}
        else if (gunObject.currentType == GunBehavior.weaponType.Melee) {
            if (Input.GetButtonDown("Shoot")) {
                gunObject.Slash();
            }
            else if(Input.GetButtonUp("Shoot")) {
                gunObject.ReleaseSlash();
            }
        }
	}

	void movementInput() {
		_mousePos = Input.mousePosition;
		_mousePos = _camTarget.ScreenToWorldPoint (new Vector3(_mousePos.x, _mousePos.y, _camTarget.transform.position.y - transform.position.y));

		Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

		// Character should always face the way it's walking if you're not aiming.
		if(Input.GetButton ("Aim")) {
			_targetRotation = Quaternion.LookRotation ((_mousePos - new Vector3(transform.position.x, 0.0f, transform.position.z)) * -1);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, _targetRotation.eulerAngles.y, _rotationSpeed * Time.deltaTime);
		}
		else if(inputVector != Vector3.zero) {
			_targetRotation = Quaternion.LookRotation (inputVector * -1);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, _targetRotation.eulerAngles.y, _rotationSpeed * Time.deltaTime);
		}

		Vector3 finalMotion = inputVector;

		// This is to not cause a higher movement speed while walking diagonally.
		finalMotion *= (Mathf.Abs (inputVector.x) == 1 && Mathf.Abs (inputVector.z) == 1)?0.7f:1;

		finalMotion *= playerSpeed;

		// Self implemented gravity.
		finalMotion += Vector3.up * -8;
		
		_controller.Move (finalMotion * Time.deltaTime);
	}
}
