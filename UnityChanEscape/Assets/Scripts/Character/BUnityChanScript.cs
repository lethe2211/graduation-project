using UnityEngine;
using System.Collections;

public class BUnityChanScript : CharacterScript {


	// Use this for initialization
	protected void Start () {
		base.Start ();
		cameraObject = GameObject.Find ("SubCameraHorizontalObject");
		rotationZ = 180;
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {

		// when patemed
		/*
		if(unityChanComponent.patema == 1){
			Vector3 v = unityChan.transform.position;
			transform.position = new Vector3(v.x, v.y + 2.5f, v.z);
			return;
		} 
		*/

		// else when pateming 

		// gravity
		if(rigidbody.mass > 0.1)rigidbody.AddForce (unityChan.transform.up * rigidbody.mass * 7); 
		
		animator.SetBool("isRunning", false);
		if(!subCamera.enabled) return;

		base.Move ();
	}

	protected void OnCollisionEnter(Collision collision){ 
		base.OnCollisionEnter (collision);
		if(collision.gameObject.name.IndexOf("Plate") >= 0){
			jumpFrame = 0;
			animator.SetBool("Jump", false);
		}

	}
}
