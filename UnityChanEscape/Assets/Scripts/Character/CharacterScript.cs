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
	protected bool moveEnabled = true;
	protected bool gravityEnabled = true;

	protected int jumpFrame = 0;
	protected float prevMass;
	
	public static int patema = 0;
	public static int whichPatema = 0;

	// around key
	protected bool subKeyFlag = false;


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
		if(!moveEnabled) return;
		
		// Zボタンでジャンプ
			if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("jumpButton")) && jumpFrame == 0){
			print ("JUMP!");
			jumpFrame = 2;
			animator.SetBool("Jump", true);
		}

		if(Input.GetKeyDown(KeyInputManager.subKeyCode) || Input.GetButtonDown("subButton")) subKeyFlag = true;
	}
	
	// Update is called once per frame

	protected void Move ()
	{	
		// ここの判定をsubKeyFlagにすると重りを持っていてパテマ状態のときにパテマを解除できなくなる
		if (Input.GetKeyDown(KeyInputManager.subKeyCode) || Input.GetButtonDown("subButton")){
			// パテマしてる場合はパテマ解除  
			 if(patema > 0){
				print("patema kaijo");
				// 合体を解除
				unityChan.transform.parent = null;
				boxUnityChan.transform.parent = null;
				 
				// パテマフラグを負にする(すぐパテマしなおしてしまわないように)
				patema *= -1;
				// 手を下げる
				unityChanComponent.animator.SetBool("Patema", false);
				boxUnityChanComponent.animator.SetBool("Patema", false);
				 
				// コライダ復活
				CapsuleCollider cc = (CapsuleCollider)unityChan.collider;
				cc.enabled = true;
				cc.center = new Vector3(0.0f , 0.8f, 0.0f);
				cc.height = 1.6f; 
				 
				CapsuleCollider bcc = (CapsuleCollider)boxUnityChan.collider;
				bcc.enabled = true;
				bcc.center = new Vector3(0.0f , 0.7f, 0.0f);
				bcc.height = 1.5f; 

				// reset flags
				unityChanComponent.SetMoveEnabled(true);
				boxUnityChanComponent.SetMoveEnabled(true);
				unityChanComponent.SetGravityEnabled(true);
				boxUnityChanComponent.SetGravityEnabled(true);
				unityChan.rigidbody.isKinematic = false;
				boxUnityChan.rigidbody.isKinematic = false;

				unityChan.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				boxUnityChan.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				whichPatema = 0;
			}
		}
		subKeyFlag = false; // subKeyFlagをもどす
						
		if (jumpFrame >= 2) {
			jumpFrame++;
			if(jumpFrame >= 5){ 
				print ("Add Jump Force");
				rigidbody.AddForce(transform.up * 4, ForceMode.VelocityChange);
				jumpFrame = 1;
			}else{
				return;
			}
		}

		// move
		// animator.SetBool("isRunning", false);
		
		Vector3 horizontal_forward = new Vector3(transform.forward.x, 0, transform.forward.z);
		float h = - Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
		float v = - Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
		
		Vector3 input = new Vector3 (h, 0, v);
		Quaternion r = cameraObject.transform.rotation;
		input = r * input;
		Vector3 velocity = new Vector3 (0, 0, 0);		// 上下のキー入力からZ軸方向の移動量を取得
		
		if (v > 0.1 || v < -0.1 || h > 0.1 || h < -0.1) {
			animator.SetBool ("isRunning", true);
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

	protected void DoPatema(Collision collision){
		print ("UnityChanScript.DoPatema");
		
		// パテマするのはジャンプ時のみ FIXME
		if(unityChanComponent.GetAnimatorPatema() || boxUnityChanComponent.GetAnimatorPatema()) return;
		
		patema = 2; // パテマフラグ2はパテマされてる状態 
		
		// もとの体重を記憶しておく
		unityChanComponent.SaveMass();
		boxUnityChanComponent.SaveMass();
		
		// DEBUG CODE
		print("UnityChan.mass: " + unityChan.rigidbody.mass);
		print("BoxUnityChan.mass: " + boxUnityChan.rigidbody.mass);
		print ("patema!!");
		
		// 両手を挙げさせる
		unityChanComponent.SetAnimatorPatema(true);
		boxUnityChanComponent.SetAnimatorPatema(true);
		
		// 体重が重い方: sbj　軽い方: obj
		float unity_mass = unityChanComponent.rigidbody.mass;
		float bunity_mass = boxUnityChanComponent.rigidbody.mass;
		GameObject sbj, obj;
		CharacterScript sbj_component, obj_component;
		if(unity_mass >= bunity_mass){
			print ("unity-chanの方が重い");
			whichPatema = 1;
			sbj = unityChan;
			sbj_component = unityChanComponent;
			obj = boxUnityChan;
			obj_component = boxUnityChanComponent;
		}else{
			print ("box-unity-chanの方が重い");
			whichPatema = 2;
			sbj = boxUnityChan;
			sbj_component = boxUnityChanComponent;
			obj = unityChan;
			obj_component = unityChanComponent;
		}     
		
		// パテマされた方のコライダを無効化
		obj.collider.enabled = false; 
		// その代わり自分のコライダを大きくする
		CapsuleCollider cc = (CapsuleCollider)sbj.collider;
		cc.center = new Vector3(cc.center.x, cc.center.y + 0.7f, cc.center.z);
		cc.height = 3.2f; 
		
		// 合体させる
		obj.transform.parent = sbj.transform;
		
		// mass を0.1以下にすると重力が無効になる
		// 相方の重力を切る 
		// obj.rigidbody.mass = 0.01f;
		obj_component.SetGravityEnabled(false);
		
		// 相方が動かないようにする
		//  obj.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
		obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		obj_component.SetMoveEnabled(false);
		obj.rigidbody.isKinematic = true;
		
		// 相方の位置を補正
		Vector3 p = new Vector3(-0.05f, 3.0f, 0.0f);
		obj.transform.localPosition = p;
		Vector3 r = obj.transform.localEulerAngles;
		obj.transform.localEulerAngles = new Vector3(r.x, 0, r.z);
		
		
		// effect
		//(GameObject.Find("PatemaParticle").GetComponent("ParticleSystem") as ParticleSystem).Play();
		
	}

	protected void OnCollisionEnter(Collision collision){
		string name = collision.gameObject.name;
		GameObject obj = collision.gameObject; // object of collision

		print ("CharacterScript.OnColisionEnter");
		if(collision.gameObject.name.IndexOf("Plate") >= 0){
			print ("着地");
			if(patema < 0) patema++;
			animator.SetBool("Jump", false);
			jumpFrame = 0;
		}
		
		// patema パテマフラグが0でないとパテマされない
		// 体重の重い方がパテマ処理する (全部どちらかに処理させないと厄介になる)
		if(obj.tag.CompareTo("Player") == 0 && patema == 0){
			if(rigidbody.mass > obj.rigidbody.mass) DoPatema(collision);
		}
	}


	protected void OnCollisionExit(Collision collision){
		string name = collision.gameObject.name;
	}

	// setter / getter
	public bool GetAnimatorPatema(){ return animator.GetBool("Patema"); }
	public void SetAnimatorPatema(bool b){ animator.SetBool("Patema", b); }
	public void SetMoveEnabled(bool b){ moveEnabled = b; }
	public void SaveMass(){ prevMass = rigidbody.mass; }
	public void LoadMass(){ rigidbody.mass = prevMass; } 
	public void SetGravityEnabled(bool b){ gravityEnabled = b; }
}
