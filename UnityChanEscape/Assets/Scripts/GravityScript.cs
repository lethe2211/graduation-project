using UnityEngine;
using System.Collections;

public class GravityScript : MonoBehaviour {

	public int g = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = new Vector3(0, -g, 0);
		rigidbody.AddForce (v * 50);
	}
}
