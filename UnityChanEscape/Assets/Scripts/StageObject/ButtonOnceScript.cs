using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonOnceScript : MonoBehaviour {
		public Color onColor = new Color (0.5f, 1.0f, 0.5f);
		public Color offColor = new Color(1.0f, 0.5f, 0.5f);
		public List<GameObject> targetObject;

		bool isPushing;
		bool isPushed;

		// Use this for initialization
		void Start () {
				renderer.material.color = offColor;
				isPushing = false;
				isPushed = false;
		}
	
		// Update is called once per frame
		void Update () {
//				if (isPushing == true && isPushed == false) {
//						
//				}
		}

		void OnTriggerEnter(Collider other) {
				renderer.material.color = onColor;
				transform.localScale = new Vector3 (1.0f, 0.3f, 1.0f);
				foreach (GameObject tObj in targetObject) {
						tObj.SendMessage ("ButtonOn");
				}
		}
}
