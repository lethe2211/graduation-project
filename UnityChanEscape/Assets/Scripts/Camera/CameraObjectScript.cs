using UnityEngine;
using System.Collections;


/**
 * カメラ自体ではなく、カメラを回転させたり上下させたりするためのオブジェクトのクラス
 */
public class CameraObjectScript : MonoBehaviour {

    GameObject horizontalObject; // カメラを水平方向に回転させるためのオブジェクト
    GameObject verticalObject;   // カメラを垂直方向に回転させるためのオブジェクトです
    Camera mainCamera; //メインカメラ
    Camera subCamera;  //サブのカメラです
    
    public string horizontalObjectName;
    public string verticalObjectName;
    public bool isMainCamera;
    
    GameObject unityChan;
    GameObject boxUnityChan;
    bool isRotateToBack;
    Quaternion characterRotation;
    int rotateFlame;
	
    bool cameraBackKeyPressed;
    
    /**
     * 各種変数の初期化
     */
    void Start ()
    {
        horizontalObject = GameObject.Find (horizontalObjectName);
        verticalObject = GameObject.Find (verticalObjectName);

        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();

        unityChan = GameObject.Find ("unitychan");
        boxUnityChan = GameObject.Find ("BoxUnityChan");
        isRotateToBack = false;
				
        cameraBackKeyPressed = false;
        rotateFlame = 0;
    }
		
    /**
     * UpdateではGetKeyDownを取得してフラグを立てる
     *
     * Fixed Update でGetKeyDownを取得するとキーの取得漏れが起こる可能性がある
     * Updateでキー入力を取得しフラグを立て、FixedUpdateで処理を行う
     */
    void Update ()
    {
        if (Input.GetKeyDown (KeyInputManager.cameraBackKeyCode) || Input.GetButtonDown ("cameraBackButton")) {
            cameraBackKeyPressed = true;
        }
    }
		

    /**
     * Updateで書き換えられたフラグを元にこちらで実際の処理をする
     *
     * GetKey（押しっぱなし判定）はこちらで取得しても問題ない
     */
    void FixedUpdate ()
    {
        Vector3 cameraPosition;
        Vector3 characterPosition;

        if (isMainCamera && !mainCamera.enabled)
            return;
        if (!isMainCamera && !subCamera.enabled)
            return;


        // 主観カメラでなければカメラ操作ができる
        if (Input.GetKey (KeyInputManager.cameraFirstPersonKeyCode) || Input.GetButton ("cameraFirstPersonButton")) {
            isRotateToBack = false;
            cameraBackKeyPressed = false;
            return;
        }
        // right-left
        if (Input.GetKey (KeyInputManager.cameraLeftRotateKeyCode) || Input.GetButton ("cameraLeftRotationButton"))
            horizontalObject.transform.Rotate (0, -3, 0);
        if (Input.GetKey (KeyInputManager.cameraRightRotateKeyCode) || Input.GetButton ("cameraRightRotationButton"))
            horizontalObject.transform.Rotate (0, 3, 0);


        // 背面カメラ
        if (cameraBackKeyPressed && !isRotateToBack) {
            isRotateToBack = true;
            if (mainCamera.enabled)
                characterRotation = unityChan.transform.rotation;
            else if (subCamera.enabled)
                characterRotation = boxUnityChan.transform.rotation;
        }
        cameraBackKeyPressed = false;

        
        // TODO: キーを押しっぱなしにすると回転し続けてしまうので直す
        if (isRotateToBack) {
            rotateFlame++;
            // 操作中のキャラクターと利用中のカメラの位置を取得
            if (mainCamera.enabled) {
                cameraPosition = mainCamera.transform.position;
                characterPosition = unityChan.transform.position + unityChan.transform.up.normalized;
            } else if (subCamera.enabled) {
                cameraPosition = subCamera.transform.position;
                characterPosition = boxUnityChan.transform.position + boxUnityChan.transform.up.normalized;
            } else
                return;
				
            // カメラの角度を背面に来るように変更
            cameraPosition = characterPosition;
            Quaternion from = mainCamera.transform.rotation;
            if (subCamera.enabled)
                from = subCamera.transform.rotation;
            Quaternion to = characterRotation;

            if (Quaternion.Dot (from, to) > 0.999f || rotateFlame > 300) {
                Debug.Log ("finished");
                isRotateToBack = false;
                rotateFlame = 0;
            } 

            horizontalObject.transform.rotation = Quaternion.Slerp (from, to, 0.1f);
            horizontalObject.transform.Rotate (0f, 180f, 0f);
        }
    }
}
