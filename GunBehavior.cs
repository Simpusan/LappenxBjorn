using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class GunBehavior : MonoBehaviour {

	public enum weaponType {
		Melee, 
		Gun
	}

	public weaponType currentType;
	public Transform gunTransform;
	public float rpm;
	public GameObject blood;
	public AudioClip concreteShot;
    public AudioSource katanaSound;
	public GameObject spinObject;
	public ParticleSystem particleSystem;
	public GameObject swordTransform;
	public GameObject minigunTransform;
    public MeleeWeaponTrail trailObject;

    // TODO: Implement array for this mess...
    public Vector3 swordStartPos;
    public Vector3 swordEndPos;

    public Vector3 swordPosFrame1;
    public Vector3 swordPosFrame2;
    public Vector3 swordPosFrame3;

    public Vector3 swordRotationFrame1;
    public Vector3 swordRotationFrame2;
    public Vector3 swordRotationFrame3;

    public Vector3 swordStartRotation;
    public Vector3 swordEndRotation;

	private float _secondsBetweenShots;
	private float _nextPossibleShootTime;

    private bool _swordIsSlashing;

	private LineRenderer tracer;



	void Start() {
		Random.seed = (int)System.DateTime.Now.Ticks;
		_secondsBetweenShots = 60/rpm;
		currentType = weaponType.Gun;

		tracer = GetComponent<LineRenderer>();
	}

	public void GenericShoot() {

		if(CanShoot()) { // Defined by amount of time since last shot
			particleSystem.Emit (1); // gunshell
			this.GetComponent<CameraShake>().SendMessage ("invokeCameraShake");

			Ray ray = new Ray(gunTransform.position, gunTransform.forward);
			RaycastHit hit;

			float shotDistance = 40.0f;

			if(Physics.Raycast (ray, out hit, shotDistance)) {
				shotDistance = hit.distance;
				GameObject hitTarget = hit.collider.gameObject;

				if(hitTarget.tag == "Enemy") { // All enemies should have a tag for enemy, and implement a "Shot" interface.
					hitTarget.SendMessage ("Shot", SendMessageOptions.RequireReceiver);
					if(hitTarget.GetComponent<Rigidbody>() != null) // For added effect, if the enemy has a rigidbody make it fly
						hitTarget.GetComponent<Rigidbody>().AddExplosionForce (300.0f, hit.point - new Vector3(0.0f, 1.0f, 0.0f), 5.0f);
				} else if(hitTarget.tag == "ConcreteWall")
					AudioSource.PlayClipAtPoint (concreteShot, hit.transform.position);
			}

			_nextPossibleShootTime = Time.time + _secondsBetweenShots;

			GetComponent<AudioSource>().Play ();

			// Creates "effect" of shot on screen. 
			StartCoroutine ("RenderTracer", ray.direction * shotDistance + new Vector3(Random.Range (-1.0f, 1.0f),
			                                                                           Random.Range (-1.0f, 1.0f),
			                                                                           Random.Range (-1.0f, 1.0f)));
		}
	}
	public void switchWeapon() {
		if(currentType == weaponType.Gun) {
            minigunTransform.SetActive(false);
			swordTransform.GetComponent<Renderer>().enabled = true;
			currentType = weaponType.Melee;
		}
		else {
            minigunTransform.SetActive(true);
            swordTransform.GetComponent<Renderer>().enabled = false;
			currentType = weaponType.Gun;
		}
	}

    void transformSword(Vector3 newLoc, Vector3 newRot) {
        swordTransform.transform.localPosition = newLoc;
        swordTransform.transform.localRotation = Quaternion.Euler(newRot);
    }

    IEnumerator slashAnimation() {
        //TODO: Improve the animation with lerping or something. Three frames may look okay but it's not.
        _swordIsSlashing = true;

        while (this.transform.localPosition != swordEndPos && this.transform.localRotation != Quaternion.Euler(swordEndRotation)) {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, swordEndPos, Time.deltaTime);
            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(swordEndRotation), Time.deltaTime);
            yield return null;
        }

        _swordIsSlashing = false;
    }

	public void Slash() {
        StartCoroutine("slashAnimation");
        
	} 

    public void ReleaseSlash() {
        transformSword(swordStartPos, swordStartRotation);
    }

	public bool CanShoot() { 
		bool canShoot = true;
		if(Time.time < _nextPossibleShootTime) {
			canShoot = false;
		}

		return canShoot;
	}

	IEnumerator RenderTracer(Vector3 hitPoint) {
		tracer.enabled = true;
		tracer.SetPosition (0, gunTransform.position);
		tracer.SetPosition (1, gunTransform.position + hitPoint);
		yield return null;
		tracer.enabled = false;
	}
}
