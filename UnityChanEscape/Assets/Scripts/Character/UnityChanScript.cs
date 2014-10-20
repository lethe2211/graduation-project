using UnityEngine;
using System.Collections;

public class UnityChanScript : CharacterScript {
	
	private bool gAlreadyPushed = false;
	private int gChangeFrame = 190;
	private bool cleared = false;
	private bool gameOverFlag = false;

	GameObject gameOverCameraObject;
	Camera gameOverCamera;
	GameObject gameClearObject;
	GameObject gameOverObject;
	
	protected GameObject mainCameraHorizontalObject;
	public float gameOverPosition;

	// Use this for initialization
	void Start () {
		base.Start ();
		mainCameraHorizontalObject = GameObject.Find("MainCameraHorizontalObject");
		
		gameOverCameraObject = GameObject.Find("GameOverCamera");
		gameOverCamera = gameOverCameraObject.GetComponent<Camera>();
		gameOverCamera.enabled = false;

		cameraObject = GameObject.Find ("MainCameraHorizontalObject");
		rotationZ = 0;

		gameOverObject = GameObject.Find ("GameOverObject");
		gameClearObject = GameObject.Find ("GameClearObject");
	}


	void Update() {
		// カメラが有効な時だけ動く 
		if(!mainCamera.enabled) return;
		// パテマとかで動きが制限されている場合
		if(!moveEnabled) return;

		base.Update ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// gravity
		if(gravityEnabled)rigidbody.AddForce (unityChan.transform.up * rigidbody.mass * -7);

		// GameOver
		if(gameOverFlag){
			transform.Rotate(0, 1, 0);
			return;
		}
		if(transform.position.y <= gameOverPosition){
			GameOver();
			gameOverFlag = true;
			return;
		}

		// if camera is selected, you can move unity-chan
		animator.SetBool("isRunning", false);
		if(!mainCamera.enabled) return;
		if(!moveEnabled){
			print ("unity-chan ugokenai");
			// Vector3 p = new Vector3(-0.05f, 3.0f, 0.0f);
			// transform.localPosition = p;
			return;
		}

		// flag for stage clear
		if(cleared){
			if(animator.GetBool("Clear")) return;
			animator.SetBool("Clear", true);
						mainCameraHorizontalObject.transform.position = transform.position + transform.up / 2;
			mainCameraHorizontalObject.transform.forward = transform.forward;
			return;
		}else{
			animator.SetBool("Clear", false);
		}

		base.Move();

		// reset camera position
		if(Input.GetKey("g")){
			if(!gAlreadyPushed){
				Physics.gravity *= -1;
				gChangeFrame = 0;
			}
			gAlreadyPushed = true;
		}else{
			gAlreadyPushed = false;
		}
		gChangeFrame++;
		if(gChangeFrame <= 90){
			transform.Rotate(0, 0, 2); 
		}
	}
	
	protected void OnCollisionEnter(Collision collision){
		print ("UnityChanScript.OnCollisionEnter");
		base.OnCollisionEnter (collision);
		GameObject obj = collision.gameObject; // object of collision

		string name = collision.gameObject.name;
		if(name == "Goal"){
			cleared = true;
			gameClearObject.SendMessage ("Cleared");
			Destroy(collision.gameObject);
		}

	}


	void GameOver(){
		print ("Game Over");
		gameOverObject.SendMessage("Over");
		animator.SetBool ("Fall", true);
		Vector3 v = transform.position;
		gameOverCameraObject.transform.position = new Vector3(v.x, -3, v.z);
		mainCamera.enabled = false;
		subCamera.enabled = false;
		gameOverCamera.enabled = true;
		// gameOverCamera.SendMessage("fadeOut");
	}
}
