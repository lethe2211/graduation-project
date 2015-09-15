using UnityEngine;
using System.Collections;

/**
 * ユニティちゃんを制御するためのスクリプト
 *
 * 重力に関しては、キャラによって重力の方向が違うため、
 * Unityデフォルトの重力機能は使っていない
 */
public class UnityChanScript : CharacterScript {
    
    private bool gAlreadyPushed = false;
    private int gChangeFrame = 190;
    private bool cleared = false;
    private bool gameOverFlag = false;

    GameObject gameClearObject;
    
    protected GameObject mainCameraHorizontalObject;

    // この座標よりも小さくなったらゲームオーバーと判定する
    public float gameOverPosition;

    /**
     * 各種値の初期化を行う
     */
    void Start ()
    {
        base.Start ();
        mainCameraHorizontalObject = GameObject.Find("MainCameraHorizontalObject");

        cameraObject = GameObject.Find ("MainCameraHorizontalObject");
        rotationZ = 0;

        gameClearObject = GameObject.Find ("GameClearObject");
    }


    /**
     * ユニティちゃん操作の時のみUpdateが働く
     *
     * mainCamera.enabled == true の時はユニティちゃん操作であると判定している
     */
    void Update ()
    {
        // ユニティちゃん操作でないなら以下実行されない
        if (!mainCamera.enabled)
            return;
        // パテマとかで動きが制限されている場合
        if (!moveEnabled) {
            cameraObject.transform.position = transform.position + transform.up;
            return;
        }

        base.Update ();
    }
    

    /**
     * キャラの移動や重力まわり
     */
    void FixedUpdate ()
    {
        // gravityEnabled = true の時のみ下方向に重力をかける
        if(gravityEnabled){
            rigidbody.AddForce(unityChan.transform.up * rigidbody.mass * GameConst.GRAVITY * -1);
        }

        // ゲームオーバーまわり
        if(gameOverFlag){
            transform.Rotate(0, 1, 0);
            return;
        }
        if(transform.position.y <= gameOverPosition){
            GameOver(CharacterConst.UNITY_CHAN_ID);
            gameOverFlag = true;
            return;
        }

        // スティックを倒したままキャラチェンジしてもRunningアニメーションが切れるように一度falseにする
        animator.SetBool("isRunning", false);
        // unity-chanが操作可能な条件
        if(!mainCamera.enabled) return;
        if(!moveEnabled){
            return;
        }

        // ステージクリアまわり
        if(cleared){
            if(animator.GetBool("Clear")) return;
            animator.SetBool("Clear", true);
            mainCameraHorizontalObject.transform.position = transform.position + transform.up / 2;
            mainCameraHorizontalObject.transform.forward = transform.forward;
            return;
        }else{
            animator.SetBool("Clear", false);
        }

        // キー入力による動作など、ボックスユニティちゃんと共通の部分をおこなう
        base.Move();

        // gボタンでカメラリセットを行う
        // TODO: キーコンフィグ専用クラスを使う
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

    /**
     * 何かに触れた時
     */
    protected void OnCollisionEnter(Collision collision){
        base.OnCollisionEnter (collision);
        GameObject obj = collision.gameObject;
        string name = collision.gameObject.name;

        // ゴールに触れた時はゴール
        if(name == "Goal"){
            cleared = true;
            gameClearObject.SendMessage ("Cleared");
            Destroy(collision.gameObject);
        }

    }
}
