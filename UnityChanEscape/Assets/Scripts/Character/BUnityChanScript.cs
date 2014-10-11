using UnityEngine;
using System.Collections;

public class BUnityChanScript : CharacterScript {

	private GameObject weightAround;
	private Collider weightAroundCollider;
	private GameObject weightHaving;
	private Collider weightHavingCollider;

	// Use this for initialization
	protected void Start () {
		base.Start ();
		cameraObject = GameObject.Find ("SubCameraHorizontalObject");
		rotationZ = 180;
	}

	void Update() {
		// カメラが有効な時だけ動く 
		if(!subCamera.enabled) return;
		if(!moveEnabled) return;
		base.Update();
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
		// gravity
		if(rigidbody.mass > 0.1)rigidbody.AddForce (unityChan.transform.up * rigidbody.mass * 7); 
		
		animator.SetBool("isRunning", false);
		if(!subCamera.enabled) return;
		if(!moveEnabled) return;

		if (Input.GetKeyDown(KeyCode.X)){ 
			if(patema == 0){
				if(weightAround != null){
					// パテマしてなくてかつ重りが近くにある場合
					MonoBehaviour weight = weightAround.GetComponent<MonoBehaviour>();
					rigidbody.mass += weight.rigidbody.mass;
					weight.rigidbody.mass = 0.01f;
					weightAround.collider.enabled = false;
					GameObject ref_object = transform.FindChild("Character1_Reference").gameObject;
					GameObject hip_object = ref_object.transform.FindChild("Character1_Hips").gameObject;
					weight.transform.parent = hip_object.transform;
					// weight.transform.parent = transform;
					Vector3 p = new Vector3(-0.1f, 0.8f, -0.1f);
					weight.transform.localPosition = p;
					weightHaving = weightAround;
					weightAround = null;
					// weightAroundCollider.isTrigger = false;

				}else if(weightHaving != null){
					weightHaving.collider.enabled = true;
					// 重りを持っている場合は重りを捨てる処理
					weightHaving.transform.parent = null;
					weightHaving.rigidbody.mass = 2.0f;
					rigidbody.mass -= 2.0f;
					weightHaving = null;
				}
			}
		}

		base.Move ();
	}

	protected void OnCollisionEnter(Collision collision){ 
		base.OnCollisionEnter (collision);
		if(collision.gameObject.name.IndexOf("Plate") >= 0){
			jumpFrame = 0;
			animator.SetBool("Jump", false);
		}
	}

	
	private void OnTriggerEnter(Collider collider){
		weightAroundCollider = collider;
		weightAround = collider.gameObject;
	}
	
	private void OnTriggerExit(Collider collider){
		weightAround = null;
	}	

}
