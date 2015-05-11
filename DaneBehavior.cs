using UnityEngine;
using System.Collections;

public class DaneBehavior : MonoBehaviour {
	public int health;
	public bool dead;

	public GameObject blood;
	public GameObject gore;
	public GameObject player;

	public NavMeshAgent agent;
	private GameObject bloodObject;
	private bool disappearing;
	new private Rigidbody rigidbody;

	private bool _lerpC;
	private Color _deadC;

	void Start() {
		// TODO implement this differently
		player = GameObject.Find ("Lappen");

		health = 100;
		dead = false;

		// Should dane be becoming more transparent?
		_lerpC = false;

		// This is to create a target colour for transparent dane.
		_deadC = this.GetComponent<MeshRenderer>().material.GetColor ("_Color");
		_deadC.a = 0;
	}

	void Update() {
		if(dead) {
			if(_lerpC) // We need to make dane more transparent before it disappears, so as to not look weird.
				this.GetComponent<MeshRenderer>().material.SetColor ("_Color", Color.Lerp (this.GetComponent<MeshRenderer>().material.GetColor ("_Color")
				                                                                           , _deadC,
				                                                                           Time.deltaTime));
		} else {
			// This is just to check if player is within reasonable range. Otherwise dane is too lazy to follow.
			if(Vector3.Distance (player.transform.position, this.transform.position) < 20) {
				NavMeshPath path = new NavMeshPath();
				agent.CalculatePath (player.transform.position, path);

				if(path.status == NavMeshPathStatus.PathComplete) {
					agent.SetDestination (player.transform.position);
					agent.Resume ();
				} else // If agent cannot find a complete path, no purpose in trying.
					agent.Stop ();
			}
			else
				agent.Stop ();
		}
	}

    void Slash() {
        for (int i = 3; i <= 3; i++) {
            Shot();
        }
    }

	void Shot() {
		//transform.rigidbody.AddForce (player.transform.forward * -100.0f);
		if(health > 30) { // Essentially, if shot won't kill dane.
			health -= 30;
			//agent.speed += 0.1f;

			bloodObject = (GameObject)Instantiate (blood, new Vector3(this.transform.position.x + Random.Range (-0.5f, 0.5f),
			                                                          this.transform.position.y + Random.Range (-0.5f, 0.5f),
			                                                          this.transform.position.z + Random.Range (-0.5f, 0.5f)),
			                                       Quaternion.identity);

			bloodObject.transform.parent = this.transform; // Otherwise blood won't follow dane.
		}
		else if(health > 0){
			health -= 10;
			bloodObject = (GameObject)Instantiate (gore, new Vector3(this.transform.position.x, 
			                                                         this.transform.position.y,
			                                                         this.transform.position.z),
			                                      Quaternion.identity);

			bloodObject.transform.parent = this.transform;

			// Now we can make a rigidbody without ruining pathfinding... because there is none
			rigidbody = this.gameObject.AddComponent<Rigidbody>();

			dead = true;
			agent.enabled = false;

			InvokeRepeating("Die", 5.0f, Time.deltaTime);
		}
	}
	void Die() {
		// TODO move implementation of lerpC down here
		if(this.GetComponent<MeshRenderer>().material.GetColor ("_Color") != _deadC) {
			_lerpC = true;
			rigidbody.useGravity = false;
			this.transform.position += new Vector3(0.0f, 0.1f, 0.0f);

			//this.GetComponent<MeshRenderer>().material.SetColor ("_Color", Color.Lerp (this.color, new Color(1.0f, 1.0f, 1.0f, 0.0f), 5.0f));
		}
		else
			Destroy(this.gameObject);
	}
}
