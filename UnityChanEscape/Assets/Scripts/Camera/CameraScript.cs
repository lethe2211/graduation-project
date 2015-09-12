using UnityEngine;
using System.Collections;

/**
 * CameraObject にくっついた カメラ自体に関するスクリプト
 */
public class CameraScript : MonoBehaviour {

    static Camera mainCamera; // メインカメラ
    static Camera subCamera; // サブのカメラです
    GameObject unityChan;
    GameObject boxUnityChan;
    bool isFirstPersonCamera; // 主観カメラを使用中かどうか
    bool cameraFirstPersonKeyPressed; // 主観カメラキーが押されたか
    private Vector3 defaultEulerAngles;

    /**
     * 各種変数の初期化
     */
    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
        unityChan = GameObject.Find("unitychan");
        boxUnityChan = GameObject.Find("BoxUnityChan");
        subCamera.enabled = false;
        isFirstPersonCamera = false;
        cameraFirstPersonKeyPressed = false;
        defaultEulerAngles = transform.eulerAngles;
    }

    /**
     * GetKeyDownはUpdateで取得し、処理自体はFixedUpdateで行う
     *
     * GetKeyDown（押された瞬間判定）に関しては、FixedUpdateではなく
     * こちらで取得しなければ、取得漏れが発生する可能性がある
     */
    void Update ()
    {
        // change camera 切り替え
        if (Input.GetKeyDown (KeyInputManager.changeCharacterKeyCode) || Input.GetButtonDown ("changeCharacterButton")) {
            if (mainCamera.enabled && boxUnityChan != null) {
                if (!(CharacterScript.whichPatema == 1)) {
                    mainCamera.enabled = false;
                    subCamera.enabled = true;
                    
                    // キャラクターが変わったことを通知
                    boxUnityChan.SendMessage("NotifyCharacterChanged", CharacterConst.BOX_UNITY_CHAN_ID);
                }
            } else if (subCamera.enabled) {
                if (!(CharacterScript.whichPatema == 2)) {
                    mainCamera.enabled = true;
                    subCamera.enabled = false;
                    
                    // キャラクターが変わったことを通知
                    boxUnityChan.SendMessage("NotifyCharacterChanged", CharacterConst.UNITY_CHAN_ID);
                }
            }
        }
        if (Input.GetKeyDown (KeyInputManager.cameraFirstPersonKeyCode) || Input.GetButtonDown ("cameraFirstPersonButton")) {
            cameraFirstPersonKeyPressed = true;
        }
    }
    
    /**
     * Updateで立てられたフラグを元に処理を行う
     *
     * GetKey に関してはこちらで取得する
     */
    void FixedUpdate ()
    {
        if (!mainCamera.enabled && !subCamera.enabled)
            return;

        float h = Input.GetAxis ("Horizontal"); // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis ("Vertical");   // 入力デバイスの垂直軸をvで定義
                
        if (isFirstPersonCamera) {
            // Wキーの入力がなくなったら元のカメラに戻す
            if (!(Input.GetKey (KeyInputManager.cameraFirstPersonKeyCode) || Input.GetButton ("cameraFirstPersonButton"))) {
                EnabledCamera ().transform.eulerAngles = defaultEulerAngles; // 保存しておいた向きにカメラを戻す
                EnabledCamera ().transform.localPosition = new Vector3 (0.0f, 0.0f, 3.0f);
                isFirstPersonCamera = false;
                // キャラを移動可能に
                unityChan.SendMessage ("SetMoveEnabled", true);
                if (boxUnityChan)
                    boxUnityChan.SendMessage ("SetMoveEnabled", true);
                // インジケーターを削除
                if (GameObject.Find ("Indicator"))
                    Destroy (GameObject.Find ("Indicator"));
            }
                        
            // 上下左右キーの入力分だけカメラの向きを変える
            Vector3 nr = EnabledCamera ().transform.eulerAngles;
            // ボックスユニティちゃんとユニティちゃんでは回転方向が逆転するのでその対応
            if (mainCamera.enabled) {
                EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x - 3.0f * v, nr.y + 3.0f * h, nr.z);
            } else if (subCamera.enabled) {
                EnabledCamera ().transform.eulerAngles = new Vector3 (nr.x + 3.0f * v, nr.y - 3.0f * h, nr.z);

            }
                        
            // ゴールの位置がわかるようにする
            Vector3 goalDirection = (GameObject.Find ("Goal").transform.position - unityChan.transform.position).normalized;
            GameObject indicator;
            if (GameObject.Find ("Indicator")) {
                indicator = GameObject.Find ("Indicator");
            } else {
                indicator = Instantiate ((GameObject)Resources.Load ("Prefab/Indicator")) as GameObject;
                indicator.name = indicator.name.Split ("(" [0]) [0];
            }
            if (mainCamera.enabled) {
                indicator.transform.position = unityChan.transform.position + goalDirection * 3;
            } else {
                indicator.transform.position = boxUnityChan.transform.position + boxUnityChan.transform.up + goalDirection * 3;

            }
            indicator.transform.forward = -goalDirection;
        }
                
        // 主観カメラに切り替える
        // 移動中でない場合にのみ使用可能
        if(cameraFirstPersonKeyPressed && h == 0 && v == 0) {
            EnabledCamera().transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            defaultEulerAngles = EnabledCamera().transform.eulerAngles; // 元々のカメラの向きを保存しておく
            isFirstPersonCamera = true;
            // キャラを両方とも動けないようにする
            unityChan.SendMessage("SetMoveEnabled", false);
            if(boxUnityChan) boxUnityChan.SendMessage("SetMoveEnabled", false);
        }
        cameraFirstPersonKeyPressed = false;            
    }
        
    /**
     * メインカメラとサブカメラで有効になっている方のカメラのオブジェクトを返す
     */
    GameObject EnabledCamera ()
    {
        if(mainCamera.enabled) return GameObject.Find("MainCamera");
        else if(subCamera.enabled) return GameObject.Find("SubCamera");
        else return null;
    }
    
}
