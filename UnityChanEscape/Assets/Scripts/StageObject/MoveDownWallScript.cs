using UnityEngine;
using System.Collections;

public class MoveDownWallScript : MonoBehaviour {

		private Vector3 initPosition;
		private Vector3 pos;

		private bool buttonFlag;

		// Use this for initialization
		void Start () {

				initPosition = transform.position;
				buttonFlag = false;

		}

		// Update is called once per frame
		void Update () {

				pos = transform.position;

				if (buttonFlag) {
						if (pos.y >= initPosition.y - 2.0) {
								transform.Translate (0, -0.1f, 0);
						}
				} else {
						if (pos.y <= initPosition.y) {
								transform.Translate (0, 0.1f, 0);
						}
				}

		}

		void ButtonOn() {

				buttonFlag = true;

		}

		void ButtonOff() {

				buttonFlag = false;
		}
}
