using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public Color OnColor = new Color(0.5f, 1.0f, 0.5f);
	public Color OffColor = new Color(1.0f, 0.5f, 0.5f);
	public GameObject targetObj;
	private bool buttonFlag = false;

	// Use this for initialization
	void Start () {
		renderer.material.color = OffColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		// オブジェクトが接触した時
		print("OnCollisionEnter");
		renderer.material.color = OnColor;
		buttonFlag = true;
		targetObj.SendMessage ("ButtonOn");
	}
	
	void OnCollisionExit(Collision collision){
		// オブジェクトが離れた時
		print("OnCollisionExit");
		renderer.material.color = OffColor;
		buttonFlag = false;
		targetObj.SendMessage ("ButtonOff");
	}

	void OnCollisionStay(Collision collision){
		// オブジェクトが接触し続けている場合
	}
}
