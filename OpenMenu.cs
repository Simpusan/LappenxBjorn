using UnityEngine;
using System.Collections;

public class OpenMenu : MonoBehaviour {
	static private bool menuOpen;
	
	public GameObject UI;
	public Camera mainCamera;
	
	void Start() {
		menuOpen = false;
	}
	
	void setMenuNotOpen() {
		menuOpen = false;
		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
	}
	
	void Update () {
		if(Input.GetButtonDown ("Menu")) {
			if(menuOpen) {
				Time.timeScale = 1;
				UI.SetActive (false);
				menuOpen = false;
				mainCamera.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
			} else {
				Time.timeScale = 0;
				UI.SetActive (true);
				menuOpen = true;
				mainCamera.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = true;
			}
		}
	}
}
