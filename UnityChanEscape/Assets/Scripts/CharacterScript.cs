using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	protected Animator animator;
	protected GameObject boxUnityChan;
	protected GameObject unityChan;
	protected UnityChanScript unityChanComponent;
	protected BUnityChanScript boxUnityChanComponent;
	protected Camera mainCamera;
	protected GameObject cameraObject;
	protected Camera subCamera;
	protected int rotationZ;

	protected int jumpFrame = 0;


	// Use this for initialization
	protected void Start () {
		animator = GetComponent<Animator>();
		mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
		unityChan = GameObject.Find ("unitychan"); 
		boxUnityChan = GameObject.Find ("BoxUnityChan");
		unityChanComponent = unityChan.GetComponent<UnityChanScript>();
		boxUnityChanComponent = boxUnityChan.GetComponent<BUnityChanScript>();
	}

	protected void Update(){
	}

	
	// Update is called once per frame
	protected void Move () {

		// patema kaijo
		if (Input.GetKeyDown(KeyCode.X)){
			 if(unityChanComponent.patema > 0){

//				if(unityChanComponent.patema == 1){
//					Vector3 uv = unityChan.transform.position;
//					boxUnityChan.transform.position = new Vector3(uv.x, uv.y + 5.2f, uv.z);
//				} 

				unityChanComponent.patema *= -1;

				CapsuleCollider cc = (CapsuleCollider)unityChan.collider;
				cc.enabled = true;
				cc.center = new Vector3(0.0f , 0.8f, 0.0f);
				cc.height = 1.6f; 
				 
				CapsuleCollider bcc = (CapsuleCollider)boxUnityChan.collider;
				bcc.enabled = true;
				bcc.center = new Vector3(0.0f , 0.7f, 0.0f);
				bcc.height = 1.5f; 
			}
		}

		// Jump FIXME: Jump Flag
		// while jumping, Jump flag = true and finish jumping and 
		// Collision enter on Rand Plane, set Jump flag = false
		// rigidbody.useGravity = true;
		if (Input.GetKeyDown(KeyCode.Z) && jumpFrame == 0){
			print ("JUMP!");
			jumpFrame = 2;
			animator.SetBool("Jump", true);
		}
		if (jumpFrame >= 2) {
			jumpFrame++;
			if(jumpFrame >= 5){
				rigidbody.AddForce(transform.up * 1700); //  , ForceMode.Impulse);
				jumpFrame = 1;
			}else{
				return;
			}
		}

		// move
		animator.SetBool("isRunning", false);
		animator.SetBool("Back", false);
		
		Vector3 horizontal_forward = new Vector3(transform.forward.x, 0, transform.forward.z);
		float h = - Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
		float v = - Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
		
		Vector3 input = new Vector3 (h, 0, v);
		Quaternion r = cameraObject.transform.rotation;
		input = r * input;
		Vector3 velocity = new Vector3 (0, 0, 0);		// 上下のキー入力からZ軸方向の移動量を取得
		
		if (v > 0.1 || v < -0.1 || h > 0.1 || h < -0.1) {
			animator.SetBool ("isRunning", true);
			float ue = transform.up.z;
			Quaternion to = Quaternion.Euler(0, Quaternion.LookRotation (input).eulerAngles.y, rotationZ);
						transform.rotation = Quaternion.Slerp (transform.rotation, to, 0.5f); 
			velocity = transform.forward * System.Math.Max (System.Math.Abs (v), System.Math.Abs (h));
			velocity *= 5.0f;
		} else {
			animator.SetBool ("isRunning", false);
		}
		transform.position += velocity * Time.fixedDeltaTime;
		
		cameraObject.transform.position = transform.position + transform.up;
	}
}
