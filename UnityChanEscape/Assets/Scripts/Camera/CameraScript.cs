using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

		static Camera mainCamera; // メインカメラ
		static Camera subCamera; // サブのカメラです
		GameObject unityChan;
		GameObject boxUnityChan;
		bool isFirstPersonCamera; // 主観カメラを使用中かどうか
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

	void Update(){
		// change camera 切り替え
		if (Input.GetKeyDown(KeyInputManager.changeCharacterKeyCode) && CharacterScript.patema == 0) {
			if(mainCamera.enabled){
				mainCamera.enabled = false;
				subCamera.enabled = true;
			}else{
				mainCamera.enabled = true;
				subCamera.enabled = false;
			}
		} 
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				if(!mainCamera.enabled && !subCamera.enabled) return;

				float h = Input.GetAxis ("Horizontal"); // 入力デバイスの水平軸をhで定義
				float v = Input.GetAxis ("Vertical"); // 入力デバイスの垂直軸をvで定義
				
				if (isFirstPersonCamera) {
						
						// Wキーの入力がなくなったら元のカメラに戻す
						if (!Input.GetKey (KeyInputManager.cameraFirstPersonKeyCode)) {
								EnabledCamera ().transform.eulerAngles = defaultEulerAngles; // 保存しておいた向きにカメラを戻す
								EnabledCamera ().transform.localPosition = new Vector3 (0.0f, 0.0f, 3.0f);
								isFirstPersonCamera = false;
								// キャラを移動可能に
								unityChan.SendMessage ("SetMoveEnabled", true);
								boxUnityChan.SendMessage ("SetMoveEnabled", true);
						}
						
						// 上下左右キーの入力分だけカメラの向きを変える
						Vector3 nr = EnabledCamera ().transform.eulerAngles;
						// ボックスユニティちゃんとユニティちゃんでは回転方向が逆転するのでその対応
						if (mainCamera.enabled) {
								EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x - 3.0f * v, nr.y + 3.0f * h, nr.z);
						} else if (subCamera.enabled) {
								EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x + 3.0f * v, nr.y - 3.0f * h, nr.z);

						}
		}
				
				
			
				// 主観カメラに切り替える
				// 移動中でない場合にのみ使用可能
				if(Input.GetKeyDown (KeyInputManager.cameraFirstPersonKeyCode) && h == 0 && v == 0) {
						EnabledCamera().transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
						defaultEulerAngles = EnabledCamera().transform.eulerAngles; // 元々のカメラの向きを保存しておく
						isFirstPersonCamera = true;
						// キャラを両方とも動けないようにする
						unityChan.SendMessage("SetMoveEnabled", false);
						boxUnityChan.SendMessage("SetMoveEnabled", false);
				}
						
		}
		
		// 使用中のカメラを取得
		GameObject EnabledCamera ()
		{
				if(mainCamera.enabled) return GameObject.Find("MainCamera");
				else if(subCamera.enabled) return GameObject.Find("SubCamera");
				else return null;
		}
}
