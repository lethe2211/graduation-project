using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージセレクト用の入力を受け付けるクラス
public class StageSelect : MonoBehaviour {

        public GUIText worldTitle;
        public GUIText stageTitle;

        public GameObject mainCamera;

        public AudioSource audioSource;
        GameObject soundManager;
        
        bool horizontalPressed = false; // 左右キーが押されたか
        bool verticalPressed = false; // 上下キーが押されたか
        int dh = 0; // 左右方向の変位
        int dv = 0; // 上下方向の変位

        // Use this for initialization
        void Start () {    
                mainCamera = GameObject.Find("Main Camera");
                soundManager = GameObject.Find("SoundManager");
        }
    
        // Update is called once per frame
        void Update () {          
                // ゲームパッドでの十字キーの入力をチェック
                checkAxis();

                if (Input.GetKeyDown ("up") || dv == 1) {
                        if (StageNoManager.selectedWorldDec () == 1) {
                                soundManager.SendMessage ("Play", "stage_select");
                        }
                        dv = 0;
                }

                if (Input.GetKeyDown ("down") || dv == -1) {
                        if (StageNoManager.selectedWorldInc () == 1) {
                                soundManager.SendMessage ("Play", "stage_select");
                        }
                        dv = 0;
                }
                        
                if (Input.GetKeyDown ("left") || dh == -1) {
                        if (StageNoManager.selectedStageDec() == 1) {
                                soundManager.SendMessage ("Play", "stage_select");
                        }
                        dh = 0;
                }
        
                if (Input.GetKeyDown ("right") || dh == 1) {
                        if (StageNoManager.selectedStageInc() == 1) {
                                soundManager.SendMessage ("Play", "stage_select");
                        }
                        dh = 0;
                }
                        
                if (Input.GetKeyDown (KeyInputManager.jumpKeyCode) || Input.GetButtonDown ("jumpButton")) {
                        soundManager.SendMessage ("Play", "stage_decide");
                        string stageName = StageNoManager.stageInfoList [StageNoManager.stageNo () - 1].stageFileName;
                        Application.LoadLevel(stageName);
                }

                worldTitle.text = StageNoManager.worldTitleList [StageNoManager.selectedWorld - 1];
                stageTitle.text = StageNoManager.stageInfoList [StageNoManager.stageNo () - 1].stageTitle;
                mainCamera.transform.position = new Vector3 (0f, -30f * (StageNoManager.selectedWorld - 1), -10f);
        }
        
        // ゲームパッドでの十字キーのKeyDown時にフラグをtrue
        void checkAxis ()
        {
                float h = Input.GetAxisRaw ("Horizontal");
                float v = Input.GetAxisRaw ("Vertical");
                
                if (h != 0 & !horizontalPressed) {
                        horizontalPressed = true;
                        dh = (int)Mathf.Sign (h);
                }
                if (v != 0 & !verticalPressed) {
                        verticalPressed = true;
                        dv = (int)Mathf.Sign(v);
                }
                if(h == 0 & horizontalPressed) horizontalPressed = false;
                if(v == 0 & verticalPressed) verticalPressed = false;
        }

}
