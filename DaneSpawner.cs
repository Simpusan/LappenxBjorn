using UnityEngine;
using System.Collections;

public class DaneSpawner : MonoBehaviour {

	public GameObject dane;

	private int daneAmount;

	void Start () {
		daneAmount = 0;
		InvokeRepeating ("spawnDane", 5.0f, 5.0f);
	}

	void spawnDane() {
		daneAmount = GameObject.FindGameObjectsWithTag ("Enemy").Length;
		if(daneAmount < 20) {
			Instantiate (dane, this.transform.position, Quaternion.identity);
		}
	}
}
