using UnityEngine;
using System.Collections;

/**
 * ボックスユニティちゃんを制御するためのスクリプト
 */
public class BUnityChanScript : CharacterScript {

    private GameObject weightAround; // 周りに重りがなかったらnull
    private Collider weightAroundCollider;
    private GameObject weightHaving; // 重りを持っていなかったらnull
    private Collider weightHavingCollider;

    /**
     * 変数の初期化などを行う
     */
    protected void Start ()
    {
        base.Start ();
        cameraObject = GameObject.Find ("SubCameraHorizontalObject");
        rotationZ = 180;
    }

    /**
     * GetKeyDown などはここで取得する
     */
    void Update ()
    {
        // 動作可能な時だけ実行する
        if (!subCamera.enabled)
            return;
        if (!moveEnabled) {
            cameraObject.transform.position = transform.position + transform.up;
            return;
        }
        base.Update();
    }
    
    /**
     * 重力や移動を行う
     */
    protected void FixedUpdate ()
    {
        // ユニティちゃんの頭の方向に重力をかける
        // （ボックスユニティちゃんの下方向に重力をかけているのではないので注意）
        if(gravityEnabled)rigidbody.AddForce (unityChan.transform.up * rigidbody.mass * 7); 

        // スティックを倒しながらキャラチェンジしてもRunningフラグがちゃんと解除されるようにする
        animator.SetBool("isRunning", false);
        // ボックスユニティちゃんを操作可能な条件
        if(!subCamera.enabled) return;
        if(!moveEnabled) return;

        // サブキーが押された時の処理
        if (subKeyFlag){ 
            subKeyFlag = false;
            if(patema == 0){
                if(weightAround != null){
                    // パテマしてなくてかつ重りが近くにある場合
                    // 重りを取得する
                    MonoBehaviour weight = weightAround.GetComponent<MonoBehaviour>();
                    weight.rigidbody.isKinematic = true;
                    rigidbody.mass += 2.0f;
                    weight.rigidbody.mass = 0.01f;

                    weightAround.collider.enabled = false;
                    weightAround.transform.FindChild("WeightObject").gameObject.collider.enabled = false;
                    GameObject ref_object = transform.FindChild("Character1_Reference").gameObject;
                    GameObject hip_object = ref_object.transform.FindChild("Character1_Hips").gameObject;
                    weight.transform.parent = hip_object.transform;
                    Vector3 p = new Vector3(0.1f, -0.3f, -0.1f);
                    weight.transform.localPosition = p;
                    weightHaving = weightAround;
                    weightAround = null;

                }else if(weightHaving != null){
                    // 重りを持っている場合は重りを捨てる処理                
                    weightHaving.rigidbody.isKinematic = false;
                    weightHaving.collider.enabled = true;
                    weightHaving.transform.FindChild("WeightObject").gameObject.collider.enabled = true;
                    weightHaving.transform.parent = null;
                    weightHaving.rigidbody.mass = 2.0f;
                    rigidbody.mass -= 2.0f;
                    weightHaving = null;
                }
            }
        }

        // ジャンプや移動を行う部分
        base.Move ();
    }

    /**
     * 着地したらジャンプフラグの復活を行う
     */
    protected void OnCollisionEnter(Collision collision)
    { 
        base.OnCollisionEnter (collision);
        // plateが名前に含まれているかどうかで地面かどうか判定している
        if(collision.gameObject.name.IndexOf("Plate") >= 0){
            jumpFrame = 0;
            animator.SetBool("Jump", false);
        }
    }


    /**
     * 周りに重りがある場合、weightAround にその重りを追加
     */
    private void OnTriggerEnter(Collider collider)
    {
        weightAroundCollider = collider;
        weightAround = collider.gameObject;
    }

    /**
     * 重りから離れた場合は weightAround を null にする
     */
    private void OnTriggerExit(Collider collider)
    {
        weightAround = null;
    }    

}
