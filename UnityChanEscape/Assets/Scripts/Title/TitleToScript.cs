using UnityEngine;
using System.Collections;

public class TitleToScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyInputManager.jumpKeyCode) || Input.GetButtonDown("jumpButton")) {

			Application.LoadLevel("StageSelect");

		}
	}
}
