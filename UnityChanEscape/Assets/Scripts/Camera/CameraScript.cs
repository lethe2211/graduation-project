using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

		Camera mainCamera;//メインカメラ
		Camera subCamera;//サブのカメラです
		GameObject unityChan;
		GameObject boxUnityChan;

		// Use this for initialization
		void Start () {
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
				unityChan = GameObject.Find("unitychan");
				boxUnityChan = GameObject.Find("BoxUnityChan");
				subCamera.enabled = false;
		}
	
		// Update is called once per frame
		void FixedUpdate () {

				// change camera 切り替え
				if (Input.GetKeyDown(KeyInputManager.changeCharacterKeyCode)) {
						if(mainCamera.enabled){
								mainCamera.enabled = false;
								subCamera.enabled = true;
						}else{
								mainCamera.enabled = true;
								subCamera.enabled = false;
						}
				} 
			
				// 主眼カメラ
				if(Input.GetKey (KeyInputManager.cameraFirstPersonKeyCode)) {
						transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
				}else{
					 transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
				}
						
		}
}
