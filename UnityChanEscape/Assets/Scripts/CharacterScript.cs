using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	protected Animator animator;
	protected GameObject boxUnityChan;
	protected GameObject unityChan;
	protected Camera mainCamera;
	protected GameObject cameraObject;
	protected Camera subCamera;
	
	protected int jumpFrame = 0;

	// Use this for initialization
	protected void Start () {
		animator = GetComponent<Animator>();
		mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
		boxUnityChan = GameObject.Find ("BoxUnityChan");
		unityChan = GameObject.Find ("unitychan");
	}

	protected void Update(){
		print ("update");
	}

	
	// Update is called once per frame
	protected void Move () {

		// Jump FIXME: Jump Flag
		// while jumping, Jump flag = true and finish jumping and 
		// Collision enter on Rand Plane, set Jump flag = false
		// rigidbody.useGravity = true;
		if (Input.GetKeyDown(KeyCode.Space) && jumpFrame == 0){
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

		/* old move method
		if(Input.GetKey("up")){
			transform.position += horizontal_forward * 0.1f;
			animator.SetBool("isRunning", true);
			
		}else if(Input.GetKey("down")){
			animator.SetBool("Back", true);
			transform.position -= horizontal_forward * 0.05f;
		}
		if(Input.GetKey ("right")) transform.Rotate(0, 3, 0);
		if(Input.GetKey ("left")) transform.Rotate (0, -3, 0);
		*/

		float h = - Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
		float v = - Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
		//Debug.Log ("h: " + h + " v: " + v);
		
		Vector3 input = new Vector3 (h, 0, v);
		Quaternion r = cameraObject.transform.rotation;
		input = r * input;
		Vector3 velocity = new Vector3 (0, 0, 0);		// 上下のキー入力からZ軸方向の移動量を取得
		
		if (v > 0.1 || v < -0.1 || h > 0.1 || h < -0.1) {
			animator.SetBool ("isRunning", true);
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (velocity), 0.1f);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (input), 0.1f); 
			velocity = transform.forward * System.Math.Max (System.Math.Abs (v), System.Math.Abs (h));
			velocity *= 5.0f;
		} else {
			animator.SetBool ("isRunning", false);
		}
		transform.position += velocity * Time.fixedDeltaTime;
		
		cameraObject.transform.position = transform.position + transform.up;
	}
}
