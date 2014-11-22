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
		Quaternion characterRotation;
		
		bool cameraBackKeyPressed = false;

		// Use this for initialization
		void Start () {
				horizontalObject = GameObject.Find (horizontalObjectName);
				verticalObject = GameObject.Find (verticalObjectName);
				
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();

				unityChan = GameObject.Find ("unitychan");
				boxUnityChan = GameObject.Find ("BoxUnityChan");
				isRotateToBack = false;
		}
		
		void Update ()
		{
				if (Input.GetKeyDown (KeyInputManager.cameraBackKeyCode) || Input.GetButtonDown ("cameraBackButton")) {
						cameraBackKeyPressed = true;
				}
		}
		
		// Update is called once per frame
		void FixedUpdate ()
		{
				Vector3 cameraPosition;
				Vector3 characterPosition;

				if (isMainCamera && !mainCamera.enabled)
						return;
				if (!isMainCamera && !subCamera.enabled)
						return;

				// up-down
				//		if(Input.GetKey ("q")) verticalObject.transform.Rotate(3, 0, 0);
				//		if(Input.GetKey ("a")) verticalObject.transform.Rotate (-3, 0, 0);

				// right-left
				if (Input.GetKey (KeyInputManager.cameraLeftRotateKeyCode) || Input.GetButton("cameraLeftRotationButton"))
						horizontalObject.transform.Rotate (0, -3, 0);
				if (Input.GetKey (KeyInputManager.cameraRightRotateKeyCode) || Input.GetButton("cameraRightRotationButton"))
						horizontalObject.transform.Rotate (0, 3, 0);

				//		if(Input.GetKey ("g")) {
				//			//Physics.gravity  *= -1;
				//		}
				
				// 背面カメラ
				if (cameraBackKeyPressed && !isRotateToBack) {
						isRotateToBack = true;
						if (mainCamera.enabled)
								characterRotation = unityChan.transform.rotation;
						else if (subCamera.enabled)
								characterRotation = boxUnityChan.transform.rotation;
				}
				cameraBackKeyPressed = false;

				// TODO: キーを押しっぱなしにすると回転し続けてしまうので直す
				if (isRotateToBack) {
						// 操作中のキャラクターと利用中のカメラの位置を取得
						if (mainCamera.enabled) {
								cameraPosition = mainCamera.transform.position;
								characterPosition = unityChan.transform.position + unityChan.transform.up.normalized;
						} else if (subCamera.enabled) {
								cameraPosition = subCamera.transform.position;
								characterPosition = boxUnityChan.transform.position + boxUnityChan.transform.up.normalized;
						} else
								return;
						
						// カメラの角度を背面に来るように変更
						cameraPosition = characterPosition;
						Quaternion from = mainCamera.transform.rotation;
						if (subCamera.enabled) from = subCamera.transform.rotation;
						Quaternion to = characterRotation;

						if (Quaternion.Dot (from, to) > 0.999f) {
								Debug.Log ("finished");
								isRotateToBack = false;
						} 

						Debug.Log ("isRotateBack is true");
						horizontalObject.transform.rotation = Quaternion.Slerp (from, to, 0.1f);
						horizontalObject.transform.Rotate (0f, 180f, 0f);
				}
		}
}
