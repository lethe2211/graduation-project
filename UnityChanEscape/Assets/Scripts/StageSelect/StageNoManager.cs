using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StageNoManager{

		public static int selectedStage = 1;
		public static int selectedWorld = 1;
		public static int maxWorldNum = 2;
		public static List<string> worldTitleList = new List<string> {"escape 1", "escape 2", "escape 3", "escape 4", "escape 5"};
		
		public static StageInfoList stageInfoList = SaveDataAnalyzer.GetInstance().stageInfoList;
		public static List<int> stageNumPerWorld = stageInfoList.CountStageNumPerWorld();
		
		public static void selectedStageInc () {
				if (StageNoManager.selectedStage < StageNoManager.stageNumPerWorld[StageNoManager.selectedWorld]) {
						if(StageNoManager.stageInfoList[StageNoManager.stageNo()].isAppeared){
							StageNoManager.selectedStage += 1;	
						}
				}
		}

		public static void selectedStageDec () {
				if (StageNoManager.selectedStage > 1) {
						StageNoManager.selectedStage -= 1;
				}
		}

		public static void selectedWorldInc () {
				if (StageNoManager.selectedWorld < StageNoManager.maxWorldNum) {
						if(StageNoManager.stageInfoList[StageNoManager.stageNo()].isAppeared){
							StageNoManager.selectedWorld += 1;	
						}
				}
				StageNoManager.selectedStage = 1;
		}

		public static void selectedWorldDec () {
				if (StageNoManager.selectedWorld > 1) {
						StageNoManager.selectedWorld -= 1;
				}
				StageNoManager.selectedStage = 1;
		}
		
		public static int stageNo() {
				int c = 0;
				for (int i = 1; i < StageNoManager.selectedWorld; i++) {
						c += stageNumPerWorld[i];
				}
				return c + StageNoManager.selectedStage;
		}	
}