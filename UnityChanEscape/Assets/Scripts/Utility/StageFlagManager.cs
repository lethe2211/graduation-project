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
//				Debug.Log (sender.name);
//				Debug.Log (currentFlags[sender]);

				// トリガーの発火
				// TargetとなるGameObjectにTriggerOnメッセージを送る
				foreach (TriggerTarget item in triggerTargets) {
						List<Trigger> trgrs = item.triggers;
						bool triggerOn = false; // 最終的にフラグの変更をターゲットに通知するかどうか

						bool triggerMatched = true; // トリガーの発火に必要な条件をすべて満たしているかどうか
						bool containedSenderInTrigger = false; // トリガーの発火に必要な条件にFlagChangedメッセージを送ってきたGameobjectが含まれているかどうか
						// これを考慮しないと，複数のボタンがあるときに，片方のボタンで設定したトリガーが，もう片方のボタンのフラグ変更によって再度呼ばれてしまう

						foreach (Trigger t in trgrs) {
//								Debug.Log (t.button.name);
//								Debug.Log (t.flag);
//								Debug.Log (currentFlags.ContainsKey (t.button));
//								Debug.Log(currentFlags[t.button]);
								if (t.button == sender)
										containedSenderInTrigger = true;
								if (!currentFlags.ContainsKey(t.button) || currentFlags [t.button] != t.flag) {
										triggerMatched = false;
										break;
								}
						}

						// 上の両方が満たされたときにフラグの変更を通知する
						if (triggerMatched && containedSenderInTrigger)
								triggerOn = true;
						if (triggerOn) {
								// Debug.Log ("TriggerOn");
								item.target.SendMessage ("TriggerOn");
						}
				}
		}
}
