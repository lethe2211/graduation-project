using UnityEngine;
using System.Collections;

public class MoveDownOnceWallScript : MonoBehaviour {
		Vector3 initPosition;
		Vector3 pos;
		bool buttonFlag;
		AudioSource audioSource;

		// Use this for initialization
		void Start () {
				initPosition = transform.position;
				buttonFlag = false;
				audioSource = this.gameObject.GetComponent<AudioSource> ();
		}
				
		void FixedUpdate () {
				pos = transform.position;

				if (buttonFlag) {
						if (pos.y >= initPosition.y - 2.0) {
								transform.Translate (0, -0.1f, 0);
						}
				}
		}

		void TriggerOn() {
				if (buttonFlag == false) {
						audioSource.PlayOneShot (audioSource.clip);
						buttonFlag = true;
				}
		}
}
