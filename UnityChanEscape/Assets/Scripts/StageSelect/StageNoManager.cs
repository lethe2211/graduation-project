using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージセレクトの具体的なロジックを記述したクラス
public static class StageNoManager{

		public static int selectedStage = 1;
		public static int selectedWorld = 1;
	public static List<string> worldTitleList = new List<string> {"エスケープ１", "エスケープ２", "エスケープ３", "エスケープ４", "エスケープ５"};
		
		public static StageInfoList stageInfoList = SaveDataAnalyzer.GetInstance().stageInfoList;
		public static List<int> stageNumPerWorld = stageInfoList.CountStageNumPerWorld();
		
		// ステージリストの最後のステージのワールド数を最大ワールド数とする
		public static int maxWorldNum = StageNoManager.stageInfoList[StageNoManager.stageInfoList.Count() - 1].world;

		public static int selectedStageInc () {
//				Debugger.QuickLog (selectedStage);
//				Debugger.QuickLog (selectedWorld);
				if (StageNoManager.selectedStage < StageNoManager.stageNumPerWorld[StageNoManager.selectedWorld]) {
						if(StageNoManager.stageInfoList[StageNoManager.stageNo()].isAppeared){
								StageNoManager.selectedStage += 1;	
								return 1;
						}
				}
				return 0;
		}

		public static int selectedStageDec () {
				if (StageNoManager.selectedStage > 1) {
						StageNoManager.selectedStage -= 1;
						return 1;
				}
				return 0;
		}

		public static int selectedWorldInc () {
				if (StageNoManager.selectedWorld < StageNoManager.maxWorldNum) {
						if(StageNoManager.stageInfoList[StageNoManager.stageNo() - StageNoManager.selectedStage + StageNoManager.stageNumPerWorld[StageNoManager.selectedWorld]].isAppeared){
								StageNoManager.selectedWorld += 1;
								StageNoManager.selectedStage = 1;
								return 1;
						}
				}
				return 0;
		}

		public static int selectedWorldDec () {
				if (StageNoManager.selectedWorld > 1) {
						StageNoManager.selectedWorld -= 1;
						StageNoManager.selectedStage = 1;
						return 1;
				}
				return 0;
		}
		
		public static int stageNo() {
				int c = 0;
				for (int i = 1; i < StageNoManager.selectedWorld; i++) {
						c += stageNumPerWorld[i];
				}
				// Debug.Log (c + StageNoManager.selectedStage);
				return c + StageNoManager.selectedStage;
		}	
}