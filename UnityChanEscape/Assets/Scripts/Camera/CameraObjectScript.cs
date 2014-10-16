using UnityEngine;
using System.Collections;

public class CameraObjectScript : MonoBehaviour {

		GameObject horizontalObject; // 水平方向に回転させるためのゲームオブジェクト
		GameObject verticalObject; // 垂直方向に回転させるためのゲームオブジェクト
		Camera mainCamera;//メインカメラ
		Camera subCamera;//サブのカメラです

		public string horizontalObjectName;
		public string verticalObjectName;
		public bool isMainCamera;

		GameObject unityChan;
		GameObject boxUnityChan;
		bool isRotateToBack;

//		Quaternion from;
//		Quaternion to;

		// Use this for initialization
		void Start () {
				horizontalObject = GameObject.Find (horizontalObjectName);
				verticalObject = GameObject.Find (verticalObjectName);
				
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();

				unityChan = GameObject.Find ("unitychan");
				boxUnityChan = GameObject.Find ("BoxUnityChan");
				isRotateToBack = false;
//				from = Quaternion.Euler(0, 0, 0);
//				to = Quaternion.Euler(0, 0, 0);
		}
		
		// Update is called once per frame
		void FixedUpdate () {
				Vector3 cameraPosition;
				Vector3 characterPosition;
				Vector3 characterRotation;
			

				if(isMainCamera && !mainCamera.enabled) return;
				if(!isMainCamera && !subCamera.enabled) return;

				// up-down
		//		if(Input.GetKey ("q")) verticalObject.transform.Rotate(3, 0, 0);
		//		if(Input.GetKey ("a")) verticalObject.transform.Rotate (-3, 0, 0);

				// right-left
				if(Input.GetKey (KeyInputManager.cameraLeftRotateKeyCode)) horizontalObject.transform.Rotate (0, -3, 0);
				if(Input.GetKey (KeyInputManager.cameraRightRotateKeyCode)) horizontalObject.transform.Rotate (0, 3, 0);

		//		if(Input.GetKey ("g")) {
		//			//Physics.gravity  *= -1;
		//		}

				// 背面カメラ
				if (Input.GetKeyDown (KeyInputManager.cameraBackKeyCode)) {
						isRotateToBack = true;



				}

				// TODO: キーを押しっぱなしにすると回転し続けてしまうので直す
				if (isRotateToBack) {

						Debug.Log ("isRotateToBack");
						// 操作中のキャラクターと利用中のカメラの位置を取得
						if (mainCamera.enabled) {
								cameraPosition = mainCamera.transform.position;
								characterPosition = unityChan.transform.position + unityChan.transform.up.normalized;
								//characterRotation = unityChan.transform.rotation + unityChan.transform
						} else if (subCamera.enabled) {
								cameraPosition = subCamera.transform.position;
								characterPosition = boxUnityChan.transform.position + boxUnityChan.transform.up.normalized;
						} else
								return;

						cameraPosition = characterPosition;
						//Vector3 cameraRotation = mainCamera.transform.rotation;
						Vector3 r = unityChan.transform.eulerAngles;
						Quaternion from = mainCamera.transform.rotation;
						Quaternion to = Quaternion.Euler(r.x, r.y, r.z);

						if (Quaternion.Dot (from, to) > 0.99f) {
								Debug.Log ("finished");
								isRotateToBack = false;
						} 

						Debug.Log ("isRotateBack is true");
						horizontalObject.transform.rotation = Quaternion.Slerp (from, to, 0.1f);
						//horizontalObject.transform.eulerAngles = r;
						horizontalObject.transform.Rotate (0f, 180f, 0f);
						// Debug.Log (horizontalObject.transform.rotation);
				}

	}
}
