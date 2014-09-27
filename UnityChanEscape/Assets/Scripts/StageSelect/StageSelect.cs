using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージセレクトのロジックを記述したクラス
public class StageSelect : MonoBehaviour {

		public int selectedStage; // 今選択されているステージのID
		public int selectedWorld; // 
		public int maxStageNum; // ステージIDの最大値
		public int maxWorldNum; // 

		public GUIText worldTitle;
		List<string> worldTitleList = new List<string> {"escape 1", "escape 2", "escape 3", "escape 4", "escape 5"};
	List<int> stageNumPerWorld;
		SaveDataAnalyzer saveDataAnalyzer;
		StageInfoList allStageInfo; // 全ステージの情報

	public GameObject mainCamera;

		// Use this for initialization
		void Start () {	
				saveDataAnalyzer = SaveDataAnalyzer.GetInstance ();
				allStageInfo = saveDataAnalyzer.stageInfoList;

				selectedStage = 1;
				selectedWorld = 1;

				while (allStageInfo [selectedStage - 1].isAppeared == false && selectedStage <= maxStageNum) {
						selectedStage += 1;
				}		
				maxWorldNum = 5;
				stageNumPerWorld = allStageInfo.CountStageNumPerWorld ();
				maxStageNum = stageNumPerWorld[selectedWorld];

				mainCamera = GameObject.Find("Main Camera");

		}
	
		// Update is called once per frame
		void Update () {
	
				int prev = selectedStage;


				if (Input.GetKeyDown ("up")) {
						if (selectedWorld > 1){
								selectedWorld -= 1;
								selectedStage = 1;
								maxStageNum = stageNumPerWorld[selectedWorld];
						}
				}

				if (Input.GetKeyDown ("down")) {
						if (selectedWorld < maxWorldNum){
								selectedWorld += 1;
								selectedStage = 1;
								maxStageNum = stageNumPerWorld[selectedWorld];
						}
				}

				// 左キー
				if (Input.GetKeyDown ("left")) {
						if (selectedStage > 1) {
								selectedStage -= 1;
								while (allStageInfo [selectedStage - 1].isAppeared == false) {
										if (selectedStage == 1) {
												selectedStage = prev;
												break;
										}
										selectedStage -= 1;
								}
						}
				}
		
				// 右キー
				if (Input.GetKeyDown ("right")) {
						if (selectedStage < maxStageNum) {
								selectedStage += 1;
								while (allStageInfo [selectedStage - 1].isAppeared == false) {
										if (selectedStage == maxStageNum) {
												selectedStage = prev;
												break;	
										}
										selectedStage += 1;
								}
						}
				}
						
				// zキー
				if (Input.GetKeyDown ("z")) {
						string stageName = "Stage" + selectedStage; // 選択したステージの名前
						Application.LoadLevel(stageName);
				}

				worldTitle.text = worldTitleList [selectedWorld - 1];
		mainCamera.transform.position = new Vector3 (0f, -20f * (selectedWorld - 1), -10f);
		}

	public int stageNo() {
		int c = 0;
		for (int i = 1; i < selectedWorld; i++) {
			c += stageNumPerWorld[i];
		}
		return c + selectedStage;
	}
}
