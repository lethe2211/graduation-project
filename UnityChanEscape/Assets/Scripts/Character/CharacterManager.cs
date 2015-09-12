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
    protected static UnityChanScript unityChanComponent;
    protected static BUnityChanScript boxUnityChanComponent;


    /**
     * カメラオブジェクトを取得する
     */
    protected static void Init() {
        if(mainCamera == null) {
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            // XXX subCameraもnull判定しておいたほうが無難か？
            subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
            unityChan = GameObject.Find ("unitychan"); 
            boxUnityChan = GameObject.Find ("BoxUnityChan");
            unityChanComponent = unityChan.GetComponent<UnityChanScript>();
            if(boxUnityChan != null)
                boxUnityChanComponent = boxUnityChan.GetComponent<BUnityChanScript>();

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
        if(mainCamera != null && mainCamera.enabled) {
            return CharacterConst.UNITY_CHAN_ID;
        }
        if(subCamera != null && subCamera.enabled) {
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

    /**
     * ボックスユニティちゃんが重りを持っているかどうか
     *
     * @return Bool 
     * - 持っていたら true 
     * - 持っていなかったら false
     * - BocUnityChanが存在しない場合も false
     */
    public static bool IsBoxUnityHavingWeight() {
        Init();
        if(boxUnityChanComponent == null) {
            return false;
        }

        if(boxUnityChanComponent.IsHavingWeight()) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * ボックスユニティちゃんの周りに重りがあるかどうか
     *
     * @return Bool 
     * - ある場合 true 
     * - ない場合 false
     * - BocUnityChanが存在しない場合も false
     */
    public static bool IsBoxUnityAroundWeight() {
        Init();
        if(boxUnityChanComponent == null) {
            return false;
        }

        if(boxUnityChanComponent.IsAroundWeight()) {
            return true;
        } else {
            return false;
        }
    }
}
