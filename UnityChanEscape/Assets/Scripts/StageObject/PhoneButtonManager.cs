using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 電話を模したボタン集合を管理するクラス
public class PhoneButtonManager : MonoBehaviour {

    List<int> expectedSequence; // 入力を期待される数字の配列．この順に押された時のみ有効
    List<int> currentSequence; // 現在押されている数字の配列

    GameObject stageFlagManager;
    GUITexture succeededImage; // 正しい入力だった時に表示する画像
    float timerSucceededImage; // succeededImageを出す時間を測るタイマー
    bool isAppearingSucceededImage; // succeededImageが出現した瞬間のみtrueになるフラグ
    GUITexture failedImage; // 入力が誤りだった時に表示する画像
    float timerFailedImage;
    bool isAppearingFailedImage;

    // Use this for initialization
    void Start()
    {
        expectedSequence = new List<int> { 6, 4, 0, 8, 5, 3 };
        currentSequence = new List<int>();
        stageFlagManager = GameObject.Find("StageFlagManager");
        succeededImage = transform.FindChild("SucceededImage").guiTexture;
        succeededImage.enabled = false;
        isAppearingSucceededImage = false;
        failedImage = transform.FindChild("FailedImage").guiTexture;
        failedImage.enabled = false;
        isAppearingFailedImage = false;
    }

    void FixedUpdate()
    {
        // succeededImageが出現した瞬間
        if (isAppearingSucceededImage == true)
        {
            succeededImage.enabled = true;
            failedImage.enabled = false;
            timerSucceededImage = 3.0f;
            isAppearingSucceededImage = false;
        }

        if (timerSucceededImage < 0.0f)
        {
            succeededImage.enabled = false;
        }
        else
        {
            timerSucceededImage -= Time.deltaTime;
        }

        // FailedImageが出現した瞬間
        if (isAppearingFailedImage == true)
        {
            failedImage.enabled = true;
            succeededImage.enabled = false;
            timerFailedImage = 3.0f;
            isAppearingFailedImage = false;
        }

        if (timerFailedImage < 0.0f)
        {
            failedImage.enabled = false;
        }
        else
        {
            timerFailedImage -= Time.deltaTime;
        }
    }

    // ボタンが押される度に呼ばれる
    // senderIdは押されたボタンのID
    void CheckSequence(int senderId)
    {
        string state; // 現在の状態

        currentSequence.Add(senderId);
        Debugger.List<int>(currentSequence);
        // 配列の値のチェック
        state = "midstream";
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (expectedSequence[i] != currentSequence[i])
            {
                state = "failed";
                break;
            }
        }

        // 状態をチェックして，クリアしたか，入力途中か，入力失敗かを判定する
        if (state == "midstream")
        {
            if (expectedSequence.Count == currentSequence.Count)
            { // クリアした時
                state = "cleared";
                currentSequence.Clear();
                this.gameObject.BroadcastMessage("Clear", SendMessageOptions.DontRequireReceiver);
                stageFlagManager.SendMessage("FlagChanged", this.gameObject);
            }
            else
            { // 入力途中
                isAppearingSucceededImage = true;
            }
        }
        else
        { // 入力失敗
            currentSequence.Clear();
            isAppearingFailedImage = true;
            this.gameObject.BroadcastMessage("Reset", SendMessageOptions.DontRequireReceiver);
        }
    }
}
