using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 電話を模したボタン集合を管理するクラス
public class PhoneButtonManager : MonoBehaviour {
		List<int> expectedSequence; // 入力を期待される数字の配列．この順に押された時のみ有効
		List<int> currentSequence; // 現在押されている数字の配列
		GameObject stageFlagManager;

		// Use this for initialization
		void Start () {
				expectedSequence = new List<int> { 6, 4, 0, 8, 5, 3 };
				currentSequence = new List<int> ();
				stageFlagManager = GameObject.Find ("StageFlagManager");
		}
		
		void FixedUpdate () {

		}

		// ボタンが押される度に呼ばれる
		// senderIdは押されたボタンのID
		void CheckSequence (int senderId) {
				string state; // 現在の状態

				currentSequence.Add (senderId);

				// 配列の値のチェック
				state = "midstream";
				for (int i = 0; i < currentSequence.Count; i++) {
						if (expectedSequence [i] != currentSequence [i]) {
								state = "failed";
								break;
						}
				}

				// 状態をチェックして，クリアしたか，入力途中か，入力失敗かを判定する
				if (state == "midstream") {
						if (expectedSequence.Count == currentSequence.Count) { // クリアした時
								state = "cleared";
								currentSequence.Clear ();
								this.gameObject.BroadcastMessage ("Clear", SendMessageOptions.DontRequireReceiver);
								stageFlagManager.SendMessage ("FlagChanged", this.gameObject);
						} else { // 入力途中

						}
				} else { // 入力失敗
						currentSequence.Clear();
						this.gameObject.BroadcastMessage ("Reset", SendMessageOptions.DontRequireReceiver);
				}

				Debug.Log (state);

		}

}
