using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

		private float frame = 0;
		private float stopFrame = 0;

		public bool isFast;
		public int rotateSpeed;

		// Use this for initialization
		void Start () {
				isFast = true;
				rotateSpeed = 90;
		}
		
		// Update is called once per frame
		void FixedUpdate () {
				if (isFast) {
						rotateSpeed = 90;
				} else {
						rotateSpeed = 0;
				}
				transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
		}

		void OnCollisionEnter(Collision c){
		}

		void OnCollisionExit(Collision c){
		}

//		void ButtonOn() {
//			rotateSpeed = 0;
//		}
//
//		void ButtonOff() {
//			rotateSpeed = 90;
//		}

		void TriggerOn() {
				isFast = !isFast;
		}

}
