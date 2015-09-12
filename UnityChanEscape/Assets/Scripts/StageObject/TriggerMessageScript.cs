using UnityEngine;
using System;
using System.Collections;

// トリガーに対応して，メッセージウィンドウを表示するスクリプト
public class TriggerMessageScript : MonoBehaviour
{

    private GUIText guiText;

    // Use this for initialization
    void Start()
    {
        guiText = transform.FindChild("Message").GetComponent<GUIText>();
        guiText.enabled = false;
    }

    void Update()
    {

    }

    // メッセージ表示のトリガー
    void DisplayMessage(String message)
    {
        guiText.enabled = true;
        guiText.text = message;
    }

    // メッセージ非表示のトリガー
    void HideMessage()
    {
        guiText.enabled = false;
    }

}
