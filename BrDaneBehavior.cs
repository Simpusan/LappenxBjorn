using UnityEngine;
using System.Collections;

public class BrDaneBehavior : MonoBehaviour {
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

		dead = false;
	}
	
	void Update() {
		if(dead) {

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

	void Shot() {
		this.GetComponent<MeshRenderer>().material.SetColor("_Color", 
		                                                    this.GetComponent<MeshRenderer>().material.GetColor("_Color") -
		                                                    new Color(-0.01f, 0.01f, 0.01f, 0.0f));
        


		agent.speed += 0.1f;
	}
	void Die() {

	}
}
