using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

// ステージに関するセーブデータ解析用のクラス
// シングルトンパターンのクラス
// インスタンスの生成は，"new SaveDataAnalyzer();"ではなく，"SaveDataAnalyzer.GetInstance();"で行う
public class SaveDataAnalyzer {
	
		private static SaveDataAnalyzer _singleInstance = new SaveDataAnalyzer ();

		SaveDataReaderWriter saveDataReaderWriter; // セーブデータ読み込み用のクラス
		StageInfoList stageInfoList; // ステージの情報

		// インスタンスの生成用関数
		public static SaveDataAnalyzer GetInstance() {
				return _singleInstance;
		}

		// コンストラクタ
		private SaveDataAnalyzer() {
				saveDataReaderWriter = new SaveDataReaderWriter (@"stageInfo.csv");
				stageInfoList = new StageInfoList ();
				Analyze ();
		}

		// 読み込んだセーブデータを解析する
		void Analyze() {
				
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

						//Debug.Log ("stageNo: " + stageNo);
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

		// ステージの情報をStageInfoクラスのオブジェクトとして返す
		public StageInfo GetStageInfo(int stageNo) {				
				return stageInfoList [stageNo - 1];
		}

		// ステージ番号とStageInfoクラスのオブジェクトを受け取り，そのステージ番号のセーブデータを書き換える
		public void UpdateStageInfo(int stageNo, StageInfo stageInfo) {
				saveDataReaderWriter.Update (stageNo - 1, stageInfo.ToList ());
		}

		// 変更をファイルに書き込む
		public void WriteStageInfo() {
				saveDataReaderWriter.WriteFile (@"stageInfo.csv");
		}

}
