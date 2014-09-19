using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class SaveDataAnalyzer {
	
		SaveDataReaderWriter saveDataReaderWriter;
		StageInfoList stageInfoList;

		public SaveDataAnalyzer() {
				saveDataReaderWriter = new SaveDataReaderWriter (@"stageinfo.csv");
				stageInfoList = new StageInfoList ();
				Analyze ();
		}

		public void Analyze() {
				
				for (int i = 0; i < saveDataReaderWriter.CountRow(); i++) {
						List<string> header = saveDataReaderWriter.header;
						List<string> row = saveDataReaderWriter.Read (i);

						int stageNo = int.Parse (row [0]);
						int world = int.Parse (row [1]);
						bool isAppeared = (row [2] == "1") ? true : false;
						bool isCleared = (row [3] == "1") ? true : false;
						int deathCount = int.Parse (row [4]);
						int score = int.Parse (row [5]);
						int clearTime = int.Parse (row [6]);

						Debug.Log ("stageNo: " + stageNo);
						StageInfo stageInfo = new StageInfo (stageNo, world, isAppeared, isCleared, deathCount, score, clearTime);
						stageInfoList.Add(stageInfo);
				}

//				foreach (var item in stageInfoList.GetAll()) {
//						Debug.Log ("stageNo: " + item.stageNo);
//				}

		}

		// 全ステージの情報をStageInfoクラスのリストとして返す
		public List<StageInfo> GetAllStageInfo() {
				return stageInfoList.GetAll ();
		}

		// ステージの情報をStageInfoクラスのインスタンスとして返す
		public StageInfo GetStageInfo(int stageNo) {				
				return stageInfoList [stageNo - 1];
		}


}
