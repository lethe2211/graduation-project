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

		// Use this for initialization
		void Start () {	
				mainCamera = GameObject.Find("Main Camera");
				soundManager = GameObject.Find("SoundManager");
		}
	
		// Update is called once per frame
		void Update () {

				if (Input.GetKeyDown ("up")) {
						if (StageNoManager.selectedWorldDec () == 1) {
								soundManager.SendMessage ("Play", "stage_select");
						}
				}

				if (Input.GetKeyDown ("down")) {
						if (StageNoManager.selectedWorldInc () == 1) {
								soundManager.SendMessage ("Play", "stage_select");
						}
				}
						
				if (Input.GetKeyDown ("left")) {
						if (StageNoManager.selectedStageDec() == 1) {
								soundManager.SendMessage ("Play", "stage_select");
						}
				}
		
				if (Input.GetKeyDown ("right")) {
						if (StageNoManager.selectedStageInc() == 1) {
								soundManager.SendMessage ("Play", "stage_select");
						}
				}
						
				if (Input.GetKeyDown (KeyInputManager.jumpKeyCode)) {
						soundManager.SendMessage ("Play", "stage_decide");
						// string stageName = "Stage" + StageNoManager.stageNo(); // 選択したステージの名前
						string stageName = StageNoManager.stageInfoList [StageNoManager.stageNo() - 1].stageTitle;
						Application.LoadLevel(stageName);
				}

				worldTitle.text = StageNoManager.worldTitleList [StageNoManager.selectedWorld - 1];
				stageTitle.text = StageNoManager.stageInfoList [StageNoManager.stageNo() - 1].stageTitle;
				mainCamera.transform.position = new Vector3 (0f, -30f * (StageNoManager.selectedWorld - 1), -10f);
		}
}
