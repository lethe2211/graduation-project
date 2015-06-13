using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ステージごとのフラグを管理し，必要に応じてフラグの値の変更，変更の通知を行うクラス
// シングルトンパターンのクラス
// インスタンスの生成は，"new StageFlagManager();"ではなく，"StageFlagManager.GetInstance();"で行う
[System.Serializable]
public class StageFlagManager : MonoBehaviour {
		private static StageFlagManager _singleInstance = new StageFlagManager ();

		// コンストラクタ
		private StageFlagManager() {

		}

		// インスタンスの生成用関数
		public static StageFlagManager GetInstance() {
				return _singleInstance;
		}
				
		Dictionary<GameObject, bool> currentFlags = new Dictionary<GameObject, bool>(); // 現在のGameObjectとフラグの組．FlagChangedが呼ばれないとaddされないことに注意
		public List<TriggerTarget> triggerTargets; // トリガーとターゲットの組のリスト

		// ステージ中の仕掛けのフラグの値が変わる度に呼ばれる
		void FlagChanged(GameObject sender) {
				// Debug.Log ("FlagChanged");

				// フラグ変更
				if (currentFlags.ContainsKey (sender)) {
						currentFlags [sender] = !currentFlags [sender];
				} else {
						currentFlags [sender] = true;
				}

				// トリガーの発火
				// TargetとなるGameObjectにTriggerOnメッセージを送る
				foreach (TriggerTarget item in triggerTargets) {
						List<GameObject> trgrs = item.triggers;
						if (trgrs.Contains (sender)) {
								if (CheckTriggerChange (trgrs, sender)) {
										item.target.SendMessage ("TriggerOn");	
								}					
						}							
				}
		}

		// フラグのチェック
		// トリガーを構成するブール値がすべてtrueの時のみtrueを返す
		bool CheckTriggerChange(List<GameObject> trgrs, GameObject sender) {
				foreach (GameObject t in trgrs) {
						if (t != sender) {
								if (!(currentFlags.ContainsKey (t) && currentFlags [t] == true)) {
										return false;
								}
						}
				}
				return true;
		}
}
