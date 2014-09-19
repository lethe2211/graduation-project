﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// セーブデータ中のステージ情報を扱うクラス
public class StageInfoList {

		List<StageInfo> stageInfoList; // ステージ情報のリスト
		int count; // ステージ情報の数

		// コンストラクタ
		public StageInfoList() {
				this.stageInfoList = new List<StageInfo> ();
		}

		public StageInfoList(List<StageInfo> stageInfoList) {
				this.stageInfoList = stageInfoList;
		}

		// 一回でセットする
		public void SetAll(List<StageInfo> stageInfoList) {
				this.stageInfoList = stageInfoList;
		}

		// ステージ情報のリストを受け取る
		public List<StageInfo> GetAll() {
				return stageInfoList;
		}

		// 末尾にステージ情報を受け取る
		public void Add(StageInfo stageInfo) {
				stageInfoList.Add (stageInfo);
		}

		// インデクサ
		public StageInfo this [int stageNo] {
				set {
						stageInfoList [stageNo] = value;
				}
				get {
						return stageInfoList [stageNo];
				}
		}

		// ステージ情報の数を返す
		public int Count() {
				return stageInfoList.Count;
		}

}
