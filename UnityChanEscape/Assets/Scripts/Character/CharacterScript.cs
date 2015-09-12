using UnityEngine;
using System.Collections;

/**
 * UnityChanScript と BoxUnityChanScript の共通した部分を記述するクラス
 *
 * TODO: パテマ時にもともとはmassをいじくりまわしていたが、
 * 今は gravityEnabled を使っているので修正する(saveMassあたり)
 */
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
    protected GameObject gameOverCameraObject;
    protected Camera gameOverCamera;
    protected GameObject gameOverObject;
    /**
     * 重力を有効にするかどうか
     * パテマの時などに変更される
     */
    protected bool gravityEnabled = true;
    /* 接触しているPlateの数を保持する変数 */
    protected int collidingPlateCount = 0;
    /* ジャンプボタンを押してから実際にジャンプするまでの時間 */
    protected const int JUMP_DELAY_FRAME = 5;

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
        if(boxUnityChan != null)
            boxUnityChanComponent = boxUnityChan.GetComponent<BUnityChanScript>();
        moveEnabled = true;
        gravityEnabled = true;
        jumpFrame = 0;
        patema = 0;
        whichPatema = 0;
        subKeyFlag = false;
        
        gameOverCameraObject = GameObject.Find("GameOverCamera");
        gameOverCamera = gameOverCameraObject.GetComponent<Camera>();
        gameOverCamera.enabled = false;
        gameOverObject = GameObject.Find ("GameOverObject");
    }

    /**
     * ジャンプキーの押下を取得してジャンプフラグを立てる
     */
    protected void Update()
    {
        if(!moveEnabled) return;
        
        // Zボタンでジャンプ
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("jumpButton")) && IsOnPlate()){
            jumpFrame = 1;
            animator.SetBool("Jump", true);
        }

        if(Input.GetKeyDown(KeyInputManager.subKeyCode) || Input.GetButtonDown("subButton")) subKeyFlag = true;
    }
    

    /**
     * 子オブジェクトのFixedUpdate() から呼ぶメソッド
     *
     * 中では、
     * - 移動に関する処理
     * - ジャンプに関する処理
     * - パテマに関する処理
     * が行われている
     */
    protected void Move ()
    {    
        // ここの判定をsubKeyFlagにすると重りを持っていてパテマ状態のときにパテマを解除できなくなる
        if (Input.GetKeyDown(KeyInputManager.subKeyCode) || Input.GetButtonDown("subButton")){
            // subkeyが押されているとき、パテマしてる場合はパテマ解除  
            if(boxUnityChan != null && patema > 0){
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

                // MoveEnabled フラグ、Kinematicフラグを正常に設定
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
        subKeyFlag = false;

        // ジャンプのaddForceを行う
        if (jumpFrame >= 1) {
            jumpFrame++;
            if(jumpFrame >= JUMP_DELAY_FRAME){ 
                rigidbody.AddForce(transform.up * 4, ForceMode.VelocityChange);
                jumpFrame = 0;
            }else{
                return;
            }
        }

        // 移動はカメラの向きに対してスティックの倒してある方向に移動する
        Vector3 horizontal_forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        float h = - Input.GetAxis ("Horizontal");                // 入力デバイスの水平軸をhで定義
        float v = - Input.GetAxis ("Vertical");                // 入力デバイスの垂直軸をvで定義
        
        Vector3 input = new Vector3 (h, 0, v);
        Quaternion r = cameraObject.transform.rotation;
        input = r * input;
        Vector3 velocity = new Vector3 (0, 0, 0);        // 上下のキー入力からZ軸方向の移動量を取得
        
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

    /**
     * パテマの判定と両者をくっつける処理を行う
     */
    protected void DoPatema(Collision collision)
    {
        // patema処理はboxUnityChahが存在しない場合はできない
        if (boxUnityChan == null)
            return;
        
        // パテマするのはジャンプ時のみとする
        if(unityChanComponent.GetAnimatorPatema() || boxUnityChanComponent.GetAnimatorPatema()) return;
        
        patema = 2; // パテマフラグ2はパテマされてる状態 
        
        // もとの体重を記憶しておく
        unityChanComponent.SaveMass();
        boxUnityChanComponent.SaveMass();
        
        // DEBUG CODE
        // print("UnityChan.mass: " + unityChan.rigidbody.mass);
        // print("BoxUnityChan.mass: " + boxUnityChan.rigidbody.mass);
        // print ("patema!!");
        
        // 両手を挙げさせる
        unityChanComponent.SetAnimatorPatema(true);
        boxUnityChanComponent.SetAnimatorPatema(true);
        
        // 体重が重い方: sbj　軽い方: obj として、
        // sbj を obj の子オブジェクトとする
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
        // その代わりパテマした方のコライダを大きくする
        CapsuleCollider cc = (CapsuleCollider)sbj.collider;
        cc.center = new Vector3(cc.center.x, cc.center.y + 0.7f, cc.center.z);
        cc.height = 3.2f; 
        
        // 合体させる
        obj.transform.parent = sbj.transform;
        
        // 相方の重力を切る 
        obj_component.SetGravityEnabled(false);
        
        // 相方が動かないようにする
        obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        obj_component.SetMoveEnabled(false);
        obj.rigidbody.isKinematic = true;
        
        // 相方の位置を補正
        Vector3 p = new Vector3(-0.05f, 3.0f, 0.0f);
        obj.transform.localPosition = p;
        Vector3 r = obj.transform.localEulerAngles;
        obj.transform.localEulerAngles = new Vector3(r.x, 0, r.z);
        
        
        // TODO: エフェクトがマージの関係で消えてしまった？直す。
        // (GameObject.Find("PatemaParticle").GetComponent("ParticleSystem") as ParticleSystem).Play();
        
    }

    /**
     * 物体と接触したときに呼ばれるメソッド
     * 
     * 相方と触れた時はパテマ処理を行う
     */
    protected void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name;
        GameObject obj = collision.gameObject;

        if(collision.gameObject.name.IndexOf("Plate") >= 0){
            if(patema < 0) patema++;
            animator.SetBool("Jump", false);
            collidingPlateCount++;
        }
        
        // パテマフラグが0でないとパテマされない
        // 体重の重い方がパテマ処理する
        if(obj.tag.CompareTo("Player") == 0 && patema == 0){
            if(rigidbody.mass > obj.rigidbody.mass) DoPatema(collision);
        }
    }
    

    /**
     * 物体と離れたときに呼ばれるメソッド
     */
    protected void OnCollisionExit(Collision collision)
    {
        string name = collision.gameObject.name;

        if(collision.gameObject.name.IndexOf("Plate") >= 0){
            collidingPlateCount--;
        }
    }

    /**
     * 手を上げた状態のアニメーションになっているかどうかを取得する
     */ 
    public bool GetAnimatorPatema()
    {
        return animator.GetBool("Patema");
    }


    /**
     * true を入力すると手を上げた状態になる
     */
    public void SetAnimatorPatema(bool b)
    {
        animator.SetBool("Patema", b);
    }


    /**
     * trueを入力すると移動できるようになる。
     * false を入力すると移動できなくなる
     * パテマ時に使う
     */
    public void SetMoveEnabled(bool b)
    {
        moveEnabled = b;
    }


    /**
     * 体重を記憶する。Loadで記憶していた体重に戻す
     *
     * パテマ前に大樹を記憶する時に使う
     */
    public void SaveMass()
    {
        prevMass = rigidbody.mass;
    }

    
    /**
     * Saveしていた体重に戻す
     *
     * パテマする時に体重を戻す時に使う
     */
    public void LoadMass()
    {
        rigidbody.mass = prevMass;
    }


    /**
     * true で重力を有効にする
     */
    public void SetGravityEnabled(bool b)
    {
        gravityEnabled = b;
    }

    /**
     * キャラクターがプレートに接しているかを返す
     * 
     * true: 接していいる
     * false: 接していなない
     * 
     * TODO: 接している物体の上に乗っているかどうかの判断はまだしていない
     */
    public bool IsOnPlate() {
        return collidingPlateCount > 0;
    }
    
    /**
     * ゲームオーバー時の処理
     */ 
    protected void GameOver(int characterId){
        gameOverObject.SendMessage("Over");
        animator.SetBool ("Fall", true);
        Vector3 v = transform.position;
        if(characterId == CharacterConst.UNITY_CHAN_ID){
            gameOverCameraObject.transform.position = new Vector3(v.x, transform.position.y + 2, v.z);
        }else if(characterId == CharacterConst.BOX_UNITY_CHAN_ID){
            gameOverCameraObject.transform.position = new Vector3(v.x, transform.position.y - 2, v.z);
            gameOverCameraObject.transform.Rotate(0, 180, 0);
        }
        mainCamera.enabled = false;
        subCamera.enabled = false;
        gameOverCamera.enabled = true;
        // gameOverCamera.SendMessage("fadeOut");
    }
}
