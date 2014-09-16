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

		gameClearObject = GameObject.Find ("GameClearObject");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// gravity
		if(rigidbody.mass > 0.1)rigidbody.AddForce (unityChan.transform.up * rigidbody.mass * -7);

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


		//
		base.Move();

		// resummon Box Unity-chan
//		if (Input.GetKeyDown(KeyCode.X)){
//			boxUnityChan.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
//		}

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
		base.OnCollisionEnter (collision);

		string name = collision.gameObject.name;
		if(name == "Goal"){
			cleared = true;
			gameClearObject.SendMessage ("Cleared");
			Destroy(collision.gameObject);
		} 

		
		if(name.IndexOf("Plate") >= 0){
			jumpFrame = 0;
			animator.SetBool("Jump", false);
		}

	
	}

//	void ClearAnimation(){
//		print("anitmation!");
//		animator.SetBool("Clear", true);
//		mainCameraHorizontalObject.transform.Rotate(0, 180, 0);
//		//camera.transform.localPosition = new Vector3(0.0f, 0.7f, 3.0f);
//	}

	void GameOver(){
		print ("Game Over");
		Application.LoadLevel("StageSelect");
		animator.SetBool ("Fall", true);
		Vector3 v = transform.position;
		gameOverCameraObject.transform.position = new Vector3(v.x, -3, v.z);
		mainCamera.enabled = false;
		subCamera.enabled = false;
		gameOverCamera.enabled = true;
		// gameOverCamera.SendMessage("fadeOut");
	}
}
