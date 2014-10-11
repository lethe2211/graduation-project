using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ステージの情報を持つクラス
public class StageInfo {

		public int stageNo;
		public int world;
		public bool isAppeared;
		public bool isCleared;
		public int deathCount;
		public int score;
		public int clearTime;
		public string stageTitle;
		
		public StageInfo(int stageNo, int world, bool isAppeared, bool isCleared, int deathCount, int score, int clearTime, string stageTitle) {
				this.stageNo = stageNo;
				this.world = world;
				this.isAppeared = isAppeared;
				this.isCleared = isCleared;
				this.deathCount = deathCount;
				this.score = score;
				this.clearTime = clearTime;
				this.stageTitle = stageTitle;
		}
		
		// stringのListを入力として受け取れるようにコンストラクタをオーバーロード
		public StageInfo (List<string> row) {
				this.stageNo = int.Parse (row [0]);
				this.world = int.Parse (row [1]);
				this.isAppeared = (row [2] == "1") ? true : false;
				this.isCleared = (row [3] == "1") ? true : false;
				this.deathCount = int.Parse (row [4]);
				this.score = int.Parse (row [5]);
				this.clearTime = int.Parse (row [6]);
				this.stageTitle = row [7];
		}

		public List<string> ToList() {
				string s_stageNo = stageNo.ToString();
				string s_world = world.ToString();
				string s_isAppeared = (isAppeared) ? "1" : "0";
				string s_isCleared = (isCleared) ? "1" : "0";
				string s_deathCount = deathCount.ToString();
				string s_score = score.ToString();
				string s_clearTime = clearTime.ToString();
				string s_stageTitle = stageTitle;

				return new List<string> {s_stageNo, s_world, s_isAppeared, s_isCleared, s_deathCount, s_score, s_clearTime, s_stageTitle};
		}

}
