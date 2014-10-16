using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonOnceScript : MonoBehaviour {
		public Color onColor = new Color (0.5f, 1.0f, 0.5f);
		public Color offColor = new Color(1.0f, 0.5f, 0.5f);

		public GameObject stageFlagManager;
		public List<GameObject> targetObject;

		bool isPushing;
		bool isPushed;

		// Use this for initialization
		void Start () {
				renderer.material.color = offColor;
				stageFlagManager = GameObject.Find ("StageFlagManager");
				isPushing = false;
				isPushed = false;
		}

		void OnTriggerEnter(Collider other) {
				// ボタンの形状変化
				renderer.material.color = onColor;
				transform.localScale = new Vector3 (1.0f, 0.3f, 1.0f);
				 
//				foreach (GameObject tObj in targetObject) {
//						tObj.SendMessage ("ButtonOn");
//				}
				// ボタンのフラグが変わったことをStageFlagManagerに通知する
				stageFlagManager.SendMessage ("FlagChanged", this.gameObject);
		}
}
