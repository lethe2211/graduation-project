using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージセレクトのロジックを記述したクラス
public class StageSelect : MonoBehaviour {

		public GUIText worldTitle;
		public GUIText stageTitle;
		

		public GameObject mainCamera;

		// Use this for initialization
		void Start () {	
				mainCamera = GameObject.Find("Main Camera");
		}
	
		// Update is called once per frame
		void Update () {

				if (Input.GetKeyDown ("up")) {
						StageNoManager.selectedWorldDec();
				}

				if (Input.GetKeyDown ("down")) {
						StageNoManager.selectedWorldInc();
				}

				// 左キー
				if (Input.GetKeyDown ("left")) {
						StageNoManager.selectedStageDec();
				}
		
				// 右キー
				if (Input.GetKeyDown ("right")) {
						StageNoManager.selectedStageInc();
				}
						
				// zキー
				if (Input.GetKeyDown ("z")) {
						string stageName = "Stage" + StageNoManager.stageNo(); // 選択したステージの名前
						Application.LoadLevel(stageName);
				}

				worldTitle.text = StageNoManager.worldTitleList [StageNoManager.selectedWorld - 1];
				stageTitle.text = StageNoManager.stageInfoList [StageNoManager.stageNo() - 1].stageTitle;
				mainCamera.transform.position = new Vector3 (0f, -20f * (StageNoManager.selectedWorld - 1), -10f);
		}
}
