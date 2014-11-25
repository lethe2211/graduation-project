using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{

		private Vector3 init_position;
		private Vector3 pos;
		public bool reversed = false;

		private float width;

		private float minHeight = -5.0f;
		private float maxHeight = 25.0f;

		private float collisionError = 1.0f;
		private float collisionWidth = 1.0f;

		// Use this for initialization
		void Start ()
		{
			init_position = transform.position;
			width = transform.localScale.x / 2;
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

						BoxCollider myCol = (BoxCollider)(rigidbody.collider);
						collisionWidth = myCol.size.x;


						float xPlusBorder = transform.position.x + collisionWidth;
						float xMinusBorder = transform.position.x - collisionWidth;
						float zPlusBorder = transform.position.z + collisionWidth;
						float zMinusBorder = transform.position.z - collisionWidth;
						
						float xMove = 0, zMove = 0, velocity = 0.3f	;
						if (xPlusBorder <= contact.point.x)
							xMove = -velocity;
						if (xMinusBorder >= contact.point.x)
							xMove = velocity;
						if (zPlusBorder <= contact.point.z)
							zMove = -velocity;
						if (zMinusBorder >= contact.point.z)
							zMove = velocity;

						transform.Translate(xMove * Time.deltaTime, 0, zMove * Time.deltaTime); 
					
						bool canPushForward = System.Math.Abs (contact.point.x - transform.position.x) <= collisionWidth && System.Math.Abs (contact.point.z - (transform.position.z + width)) <= error;
						bool canPushBack = System.Math.Abs (contact.point.x - transform.position.x) <= collisionWidth && System.Math.Abs (contact.point.z - (transform.position.z - width)) <= error;
						bool canPushLeft = System.Math.Abs (contact.point.z - transform.position.z) <= collisionWidth && System.Math.Abs (contact.point.x - (transform.position.x - width)) <= error;
						bool canPushRight = System.Math.Abs (contact.point.z - transform.position.z) <= collisionWidth && System.Math.Abs (contact.point.x - (transform.position.x + width)) <= error;

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

						if (!(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
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
