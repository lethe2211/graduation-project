using UnityEngine;
using System.Collections;

/**
 * キャラクターの状態とかを管理するクラス
 */
public class CharacterManager : MonoBehaviour {
    protected static Camera mainCamera;
    protected static Camera subCamera;

    /**
     * カメラオブジェクトを取得する
     */
    protected static void InitCameraObject() {
        if(mainCamera == null) {
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
        }
    }

    /**
     * 操作キャラクターを取得する
     */
    public static int GetEnabledCharacter() {
        InitCameraObject();
        if(mainCamera.enabled) {
            return CharacterConst.UNITY_CHAN;
        }

        if(subCamera.enabled) {
            return CharacterConst.BOX_UNITY_CHAN;
        }
        
        return CharacterConst.INVALID;
    }
}
