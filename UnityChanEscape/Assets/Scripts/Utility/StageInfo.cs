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
		
		public StageInfo(int stageNo, int world, bool isAppeared, bool isCleared, int deathCount, int score, int clearTime) {
				this.stageNo = stageNo;
				this.world = world;
				this.isAppeared = isAppeared;
				this.isCleared = isCleared;
				this.deathCount = deathCount;
				this.score = score;
				this.clearTime = clearTime;
		}

		public List<string> ToList() {
				string s_stageNo = stageNo.ToString();
				string s_world = world.ToString();
				string s_isAppeared = (isAppeared) ? "1" : "0";
				string s_isCleared = (isCleared) ? "1" : "0";
				string s_deathCount = deathCount.ToString();
				string s_score = score.ToString();
				string s_clearTime = clearTime.ToString();

				return new List<string> {s_stageNo, s_world, s_isAppeared, s_isCleared, s_deathCount, s_score, s_clearTime};
		}

}
