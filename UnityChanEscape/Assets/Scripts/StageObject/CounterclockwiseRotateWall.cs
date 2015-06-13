using UnityEngine;
using System.Collections;

public class CounterclockwiseRotateWall : MonoBehaviour {

		private bool buttonFlag;
		private bool moving;
		private double angle;

		public Vector3 centroid;
		public Vector3 axis;
		public int rotateSpeed = 90;

		void Start () {
				buttonFlag = false;
				moving = false;
		}
	
		void FixedUpdate () {

				angle = transform.eulerAngles.x;
				Debug.Log (angle);
				if (buttonFlag && 0.0 <= angle && angle < 89.0) {
						moving = true;
				} else {
						moving = false;
				}
				if (moving) {
						transform.RotateAround (
								centroid, 
								axis, 40 * 
								Time.deltaTime);
				}
		}

		void TriggerOn() {
				buttonFlag = !buttonFlag;
		}

}
