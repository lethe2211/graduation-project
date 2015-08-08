using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour {

    public int selectedText; // 今選択されているステージのID
    public GUIText[] textList; // 選択できるテキストのリスト
    public GUIText arrow; // 矢印
    public GUIText[] allTexts; // ポーズ中の全テキスト
    public GUITexture manual; // 操作マニュアル
    private int maxTextNum; // ステージIDの最大値
    protected bool isPaused; // ポーズ中かどうかを表すフラグ
    bool verticalPressed = false; // 上下キーが押されたか
    int dv = 0; // 上下キーの変位

    // Use this for initialization
    void Start()
    {
        selectedText = 0;
        maxTextNum = textList.Length - 1;
        isPaused = false;
        foreach (GUIText gt in allTexts)
        {
            gt.enabled = false;
        }
        manual.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ポーズ中でなければPボタンでポーズ
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyInputManager.pauseKeyCode) || Input.GetButtonDown("pauseButton"))
            {
                Time.timeScale = 0f;
                isPaused = true;
                foreach (GUIText gt in allTexts)
                {
                    gt.enabled = true;
                }
                manual.enabled = true;
            }

        }
        else
        {
            // Pボタンでポーズ解除
            if (Input.GetKeyDown(KeyInputManager.pauseKeyCode) || Input.GetButtonDown("pauseButton"))
            {
                Unpause();
            }

            checkYAxis();

            // 上キー
            if (Input.GetKeyDown("up") || dv == 1)
            {
                if (selectedText > 0)
                {
                    selectedText -= 1;
                }
                else
                {
                    selectedText = maxTextNum;
                }
                dv = 0;
            }

            // 下キー
            if (Input.GetKeyDown("down") || dv == -1)
            {
                if (selectedText < maxTextNum)
                {
                    selectedText += 1;
                }
                else
                {
                    selectedText = 0;
                }
                dv = 0;
            }

            arrow.transform.position = new Vector3(0.25f, 0.5f - 0.1f * selectedText, 0f);

            // zキー
            if (Input.GetKeyDown(KeyInputManager.jumpKeyCode) || Input.GetButtonDown("jumpButton"))
            {
                switch (selectedText)
                {
                    case 0:
                        Unpause();
                        break;
                    case 1:
                        Application.LoadLevel(Application.loadedLevel);
                        Time.timeScale = 1f;
                        break;
                    case 2:
                        Application.LoadLevel("StageSelect");
                        Time.timeScale = 1f;
                        break;
                }
            }
        }
    }

    void Unpause()
    {
        foreach (GUIText gt in allTexts)
        {
            gt.enabled = false;
        }
        manual.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    // ゲームパッドでの十字キーのKeyDown時にフラグをtrue
    void checkYAxis()
    {
        float v = Input.GetAxisRaw("Vertical");
        if (v != 0 & !verticalPressed)
        {
            verticalPressed = true;
            dv = (int)Mathf.Sign(v);
        }
        if (v == 0 & verticalPressed) verticalPressed = false;
    }

}
