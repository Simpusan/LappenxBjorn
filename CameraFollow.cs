using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform targetObject;
	private float _smoothingSpeed = 5.0f;

	Vector3 offsetAmount;

	void Start () {
		offsetAmount = transform.position - targetObject.position;
	}

	void Update () {
		Vector3 targetCameraPos = targetObject.position + offsetAmount;

		transform.position = Vector3.Lerp (transform.position, targetCameraPos, _smoothingSpeed * Time.deltaTime);
	}
}
