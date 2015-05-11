using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {
	void Start () {
		Invoke ("Death", 6.0f);	
	}	

	void Death () {
		Destroy (this.gameObject);
	}
}
