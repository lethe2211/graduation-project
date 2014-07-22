using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	protected Animator animator;
	protected GameObject boxUnityChan;
	protected GameObject unityChan;
	protected Camera mainCamera;
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
			if(jumpFrame >= 20){
				rigidbody.AddForce(transform.up * 2000); //  , ForceMode.Impulse);
				jumpFrame = 1;
			}else{
				return;
			}
		}

		// move
		animator.SetBool("isRunning", false);
		animator.SetBool("Back", false);
		
		Vector3 horizontal_forward = new Vector3(transform.forward.x, 0, transform.forward.z);

		if(Input.GetKey("up")){
			transform.position += horizontal_forward * 0.05f;
			animator.SetBool("isRunning", true);
			
		}else if(Input.GetKey("down")){
			animator.SetBool("Back", true);
			transform.position -= horizontal_forward * 0.02f;
		}
		
		if(Input.GetKey ("right")) transform.Rotate(0, 3, 0);
		
		if(Input.GetKey ("left")) transform.Rotate (0, -3, 0);
	}
}
