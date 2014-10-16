﻿using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{

		private Vector3 init_position;
		private Vector3 pos;
		public bool reversed = false;

		private float width;

		private float minHeight = -5.0f;
		private float maxHeight = 25.0f;

		private float collisionError = 0.2f;
		private float collisionWidth = 0.3f;

		// Use this for initialization
		void Start ()
		{
			init_position = transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
				pos = transform.position;

				if (reversed == true) {
						rigidbody.AddForce (transform.up * 1000);
						if (pos.y > maxHeight)
								transform.position = init_position;
				} else {
						if (pos.y < minHeight)
								transform.position = init_position;
				}
		}
				
		void OnCollisionStay (Collision col)
		{
				// Debug.Log ("ontriggerenter");

				double error = collisionError;
		
				if (col.gameObject.tag == "Player") {
						ContactPoint contact = col.contacts [0];
					
						bool canPushForward = System.Math.Abs (contact.point.x - transform.position.x) <= collisionWidth && System.Math.Abs (contact.point.z - (transform.position.z + 0.5)) <= error;
						bool canPushBack = System.Math.Abs (contact.point.x - transform.position.x) <= collisionWidth && System.Math.Abs (contact.point.z - (transform.position.z - 0.5)) <= error;
						bool canPushLeft = System.Math.Abs (contact.point.z - transform.position.z) <= collisionWidth && System.Math.Abs (contact.point.x - (transform.position.x - 0.5)) <= error;
						bool canPushRight = System.Math.Abs (contact.point.z - transform.position.z) <= collisionWidth && System.Math.Abs (contact.point.x - (transform.position.x + 0.5)) <= error;

						col.gameObject.GetComponent<Animator> ().SetBool ("isPushing", true);
					
						if (canPushForward) {		
								transform.Translate (0, 0, -1 * Time.deltaTime);
						} else if (canPushBack) {
								transform.Translate (0, 0, 1 * Time.deltaTime);
						} else if (canPushLeft) {
								transform.Translate (1 * Time.deltaTime, 0, 0);
						} else if (canPushRight) {
								transform.Translate (-1 * Time.deltaTime, 0, 0);
						}

						if (!(Input.GetKey ("up") || Input.GetKey ("down") || Input.GetKey ("right") || Input.GetKey ("left"))) {
									col.gameObject.GetComponent<Animator>().SetBool("isPushing", false);
						}	
			}
		}
		
		void OnCollisionExit (Collision col)
		{
				if (col.gameObject.tag == "Player") {
						col.gameObject.GetComponent<Animator> ().SetBool ("isPushing", false);
				}
		}		
}
