using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

		Camera mainCamera; // メインカメラ
		Camera subCamera; // サブのカメラです
		GameObject unityChan;
		GameObject boxUnityChan;
		bool isFirstPersonCamera; // 主眼カメラを使用中かどうか
		private Vector3 defaultEulerAngles;

		// Use this for initialization
		void Start () {
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
				unityChan = GameObject.Find("unitychan");
				boxUnityChan = GameObject.Find("BoxUnityChan");
				subCamera.enabled = false;
				isFirstPersonCamera = false;
				defaultEulerAngles = transform.eulerAngles;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{

				float h = Input.GetAxis ("Horizontal"); // 入力デバイスの水平軸をhで定義
				float v = Input.GetAxis ("Vertical");	 // 入力デバイスの垂直軸をvで定義
				
				if (isFirstPersonCamera) {
						
						if (Input.GetKeyUp (KeyInputManager.cameraFirstPersonKeyCode)) {
								EnabledCamera ().transform.eulerAngles = defaultEulerAngles;
								EnabledCamera ().transform.localPosition = new Vector3 (0.0f, 0.0f, 3.0f);
								isFirstPersonCamera = false;
								unityChan.SendMessage ("SetMoveEnabled", true);
								boxUnityChan.SendMessage ("SetMoveEnabled", true);
						}
						
						Vector3 nr = EnabledCamera ().transform.eulerAngles;
						if (mainCamera.enabled) {
								EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x - 3.0f * v, nr.y + 3.0f * h, nr.z);
						} else if (subCamera.enabled) {
								EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x + 3.0f * v, nr.y - 3.0f * h, nr.z);

						}
		}
				
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
				if(Input.GetKeyDown (KeyInputManager.cameraFirstPersonKeyCode) && h == 0 && v == 0) {
						EnabledCamera().transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
						defaultEulerAngles = EnabledCamera().transform.eulerAngles;
						isFirstPersonCamera = true;
						unityChan.SendMessage("SetMoveEnabled", false);
						boxUnityChan.SendMessage("SetMoveEnabled", false);
				}
						
		}
		
		GameObject EnabledCamera ()
		{
				if(mainCamera.enabled) return GameObject.Find("MainCamera");
				else if(subCamera.enabled) return GameObject.Find("SubCamera");
				else return null;
		}
}
