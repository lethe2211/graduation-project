using UnityEngine;
using System.Collections;

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



}
