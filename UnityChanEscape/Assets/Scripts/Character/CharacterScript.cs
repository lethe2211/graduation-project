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

	protected ArrayList weightArray;
	protected ArrayList weightHaving;

	protected int jumpFrame = 0;
	protected float prevMass;
	
	public static int patema = 0;
		


	// Use this for initialization
	protected void Start () {
		animator = GetComponent<Animator>();
		mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
		unityChan = GameObject.Find ("unitychan"); 
		boxUnityChan = GameObject.Find ("BoxUnityChan");
		unityChanComponent = unityChan.GetComponent<UnityChanScript>();
		boxUnityChanComponent = boxUnityChan.GetComponent<BUnityChanScript>();

		weightArray = new ArrayList ();
		weightHaving = new ArrayList ();
	}

	protected void Update(){
	}
	
	// Update is called once per frame
	protected void Move ()
	{	
		if (Input.GetKeyDown(KeyInputManager.subKeyCode)){
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

				// FIXME: 体重を元に戻す(仮)
				// print ("buem " + boxUnityChanComponent.extendedMass + " uem " + boxUnityChanComponent.extendedMass);
				unityChan.rigidbody.mass =  unityChanComponent.prevMass;
				boxUnityChan.rigidbody.mass = boxUnityChanComponent.prevMass;
			
			}else if(weightArray.Count > 0){
				// get weight item
				GameObject weightObject = weightArray[0] as GameObject;
				
				print ("get weight item! " + weightObject.name); 

				// mass plus
				rigidbody.mass += weightObject.rigidbody.mass; 
				Destroy(weightObject);
			}


		}

		// Zボタンでジャンプ
		if (Input.GetKeyDown(KeyInputManager.jumpKeyCode) && jumpFrame == 0){
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

	protected void OnCollisionEnter(Collision collision){
		string name = collision.gameObject.name;
		GameObject obj = collision.gameObject; // object of collision
		
		if(name.IndexOf("weight") >= 0){
			weightArray.Add (collision.gameObject); 
			print("OnCollisionEnter: " + name + "\t array count: " + weightArray.Count);
		}

		// patema パテマフラグが0でないとパテマされない
		// 体重の重い方がパテマ処理する (全部どちらかに処理させないと厄介になる)
		if(name.IndexOf("Chan") >= 0 && patema == 0)
			if(rigidbody.mass > obj.rigidbody.mass)
				doPatema(collision);
	}

	private void doPatema(Collision collision){
		// パテマするのはジャンプ時のみ
		if(!unityChanComponent.animator.GetBool("Jump") && !boxUnityChanComponent.animator.GetBool("Jump")) return;
		
		GameObject obj = collision.gameObject; // object of collision

		patema = 2; // パテマフラグ2はパテマされてる状態 
		
		// 体重の記憶
		unityChanComponent.prevMass = unityChan.rigidbody.mass;
		boxUnityChanComponent.prevMass = boxUnityChan.rigidbody.mass;
		
		// DEBUG CODE
		print("UnityChan.mass: " + unityChan.rigidbody.mass);
		print("BoxUnityChan.mass: " + boxUnityChan.rigidbody.mass);
		print ("patema!!");
		
		// 両手を挙げさせる
		animator.SetBool("Patema", true);
		obj.GetComponent<Animator>().SetBool("Patema", true);
		
		// パテマされた方のコライダを無効化
		obj.collider.enabled = false; 
		// その代わり自分のコライダを大きくする
		CapsuleCollider cc = (CapsuleCollider)collider;
		cc.center = new Vector3(cc.center.x, cc.center.y + 0.7f, cc.center.z);
		cc.height = 3.2f; 
		
		// 合体させる
		obj.transform.parent = transform;
		
		// mass を0.1以下にすると重力が無効になる
		// 相方の重力を切る 
		obj.rigidbody.mass = 0.01f;
		
		// 相方の位置を補正
		Vector3 p = new Vector3(-0.05f, 3.0f, 0.0f);
		obj.transform.localPosition = p;
		Vector3 r = obj.transform.eulerAngles;
		obj.transform.localEulerAngles = new Vector3(r.x, 0, r.z);
		
		
		// effect
		(GameObject.Find("PatemaParticle").GetComponent("ParticleSystem") as ParticleSystem).Play();

	}

	protected void OnCollisionExit(Collision collision){
		
		string name = collision.gameObject.name;

		
		if(collision.gameObject.name.IndexOf("Plate") >= 0 && patema < 0){
			patema++;
		}
		
		if(name.IndexOf("weight") >= 0){
			weightArray.Remove(collision.gameObject);
			print("OnCollisionExit: " + name + "\t array count: " + weightArray.Count);
		}
	}
}
