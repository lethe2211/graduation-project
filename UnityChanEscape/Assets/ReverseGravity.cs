using UnityEngine;
using System.Collections;

public class ReverseGravity : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {	

		rigidbody.AddForce (transform.up * 50);

	}

}
