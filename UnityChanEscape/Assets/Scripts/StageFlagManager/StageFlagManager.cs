using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// トリガーの発火に必要なGameObjectの名前と，発火した後に動くGameObjectの組を持つクラス
[System.Serializable]
public class Trigger {
		public List<GameObject> buttons;
		public GameObject target;
}

// ステージごとのフラグを管理し，必要に応じてフラグの値の変更，変更の通知を行うクラス
[System.Serializable]
public class StageFlagManager : MonoBehaviour {
		Dictionary<string, bool> buttonFlags = new Dictionary<string, bool>(); // イベントのトリガーとなるようなGameObjectの名前と，フラグの組を持つディクショナリ
		// Dictionary<List<string>, GameObject> triggerConditions; 
		public Trigger[] triggerConditions; 

		// Use this for initialization
		void Start () {
				
		}

		// ステージ中の仕掛けのフラグの値が変わる度に呼ばれる
		void FlagChanged(GameObject sender) {
				string name = sender.name; // 呼び出し元GameObjectの名前

				// フラグ変更
				if (buttonFlags.ContainsKey (name)) {
						buttonFlags [name] = !buttonFlags [name];
				} else {
						buttonFlags [name] = true;
				}

				// トリガーの発火
				// フラグが立ったGameObjectにTriggerOnメッセージを送る
				foreach (Trigger item in triggerConditions) {
						List<GameObject> flags = item.buttons;
						bool flg = true;
						foreach (GameObject go in flags) { 
								if (!buttonFlags.ContainsKey(go.name) || buttonFlags [go.name] == false) {
										flg = false;
										break;
								}
						}
						if (flg) {
								item.target.SendMessage ("TriggerOn");
						}
				}
		}
}
