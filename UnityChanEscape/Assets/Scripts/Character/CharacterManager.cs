using UnityEngine;
using System.Collections;

/**
 * キャラクターの状態とかを管理するクラス
 *
 * static っぽいメソッドはこっちに生やして欲しい
 */
public class CharacterManager : MonoBehaviour {
    protected static Camera mainCamera;
    protected static Camera subCamera;
    protected static GameObject boxUnityChan;
    protected static GameObject unityChan;

    /**
     * カメラオブジェクトを取得する
     */
    protected static void Init() {
        if(mainCamera == null) {
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
            unityChan = GameObject.Find ("unitychan"); 
            boxUnityChan = GameObject.Find ("BoxUnityChan");
        }
    }

    /**
     * 操作キャラクターIDを取得する
     *
     * 使用例
     * if(CharacterManager.GetEnabledCharacterId == CharacterConst.UNITY_CHAN_ID) {
     *     // ユニティちゃん専用処理
     * }
     */
    public static int GetEnabledCharacterId() {
        Init();
        if(mainCamera.enabled) {
            return CharacterConst.UNITY_CHAN_ID;
        }
        if(subCamera.enabled) {
            return CharacterConst.BOX_UNITY_CHAN_ID;
        }
        return CharacterConst.INVALID_ID;
    }

    /**
     * 操作キャラクターを返す
     */
    public static GameObject GetEnabledCharacter() {
        Init();
        switch(GetEnabledCharacterId()) {
            case CharacterConst.UNITY_CHAN_ID:
                return unityChan;
            case CharacterConst.BOX_UNITY_CHAN_ID:
                return boxUnityChan;
            default:
                return null;
        }
    }
}
