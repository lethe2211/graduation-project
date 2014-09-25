using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	private float frame = 0;
	private float stopFrame = 0;

	public int RotateSpeed = 90;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0, 0, RotateSpeed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision c){
	}

	void OnCollisionExit(Collision c){
	}

	void ButtonOn() {
		RotateSpeed = 0;
	}

	void ButtonOff() {
		RotateSpeed = 90;
	}

}
