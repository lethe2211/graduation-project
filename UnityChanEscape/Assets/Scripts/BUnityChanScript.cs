using UnityEngine;
using System.Collections;

public class BUnityChanScript : CharacterScript {


	// Use this for initialization
	protected void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected void Update () {

		// gravity
		rigidbody.AddForce (unityChan.transform.up * 50);

		if(!subCamera.enabled) return;


		base.Move ();
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name.IndexOf("Plate") >= 0){
			jumpFrame = 0;
			animator.SetBool("Jump", false);
		}
	}
}
