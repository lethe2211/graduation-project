using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージセレクトのロジックを記述したクラス
public class StageSelect : MonoBehaviour {

		public int selectedStage; // 今選択されているステージのID
		public int maxStageNum; // ステージIDの最大値

		SaveDataAnalyzer saveDataAnalyzer;
		List<StageInfo> allStageInfo; // 全ステージの情報

		// Use this for initialization
		void Start () {	
				saveDataAnalyzer = SaveDataAnalyzer.GetInstance ();
				allStageInfo = saveDataAnalyzer.GetAllStageInfo ();

				selectedStage = 1;
				maxStageNum = 5;
				while (allStageInfo [selectedStage - 1].isAppeared == false && selectedStage <= maxStageNum) {
						selectedStage += 1;
				}				
		}
	
		// Update is called once per frame
		void Update () {
	
				int prev = selectedStage;

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


		}
}
