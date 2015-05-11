using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	private Vector3 _originalCameraPosition;
	private const float _shakeAmount = 0.010f;
	public Camera mainCamera;

	void invokeCameraShake() {
		InvokeRepeating("shakeCamera", 0, .01f);
		Invoke("stopShaking", 0.3f);
	}
	
	void shakeCamera() {
		if (_shakeAmount>0) {
			float quakeAmountX = Random.value * _shakeAmount * 2 - _shakeAmount;
			float quakeAmountZ = Random.value * _shakeAmount * 2 - _shakeAmount;

			Vector3 camPos = mainCamera.transform.position;
			camPos.x += quakeAmountX; 
			camPos.z += quakeAmountZ;

			mainCamera.transform.position = camPos;
		}
	}
	
	void stopShaking() {
		CancelInvoke("shakeCamera");
		//mainCamera.transform.position = _originalCameraPosition;
	}
	
}