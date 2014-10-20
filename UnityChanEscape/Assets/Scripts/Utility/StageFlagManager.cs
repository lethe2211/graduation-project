using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ステージごとのフラグを管理し，必要に応じてフラグの値の変更，変更の通知を行うクラス
[System.Serializable]
public class StageFlagManager : MonoBehaviour {
		Dictionary<GameObject, bool> currentFlags = new Dictionary<GameObject, bool>(); // 現在のGameObjectとフラグの組．FlagChangedが呼ばれないとaddされないことに注意
		public List<TriggerTarget> triggerTargets; // 

		// Use this for initialization
		void Start () {
				
		}

		// ステージ中の仕掛けのフラグの値が変わる度に呼ばれる
		void FlagChanged(GameObject sender) {
				// Debug.Log ("FlagChanged");
				// string name = sender.name; // 呼び出し元GameObjectの名前

				// フラグ変更
				if (currentFlags.ContainsKey (sender)) {
						currentFlags [sender] = !currentFlags [sender];
				} else {
						currentFlags [sender] = true;
				}

				// トリガーの発火
				// TargetとなるGameObjectにTriggerOnメッセージを送る
				foreach (TriggerTarget item in triggerTargets) {
						List<Trigger> trgrs = item.triggers;
						bool triggerOn = true;
						foreach (Trigger t in trgrs) {
//								Debug.Log (t.button.name);
//								Debug.Log (t.flag);
//								Debug.Log (currentFlags.ContainsKey (t.button));
//								Debug.Log(currentFlags[t.button]);
								if (!currentFlags.ContainsKey(t.button) || currentFlags [t.button] != t.flag) {
										triggerOn = false;
										break;
								}
						}
						if (triggerOn) {
								// Debug.Log ("TriggerOn");
								item.target.SendMessage ("TriggerOn");
						}
				}
		}
}
