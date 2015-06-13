using UnityEngine;
using System.Collections;

// プレート型のボタンの動作スクリプト
// PhoneButtonManagerと合わせて使う
public class PlateButtonScript : MonoBehaviour {
		int id; 
		bool isPushed;
		bool isCleared;

		Color lightRedColor = new Color(1.0f, 0.5f, 0.5f);
		Color lightGreenColor = new Color(0.5f, 1.0f, 0.5f);
		Color gray = new Color (0.3f, 0.3f, 0.3f);
		GameObject phoneButtonManager;

		// Use this for initialization
		void Start () {
				string name = this.name;
				id = int.Parse (name.Substring (name.Length - 1)); // IDはボタンの名前の末尾1文字から取得する
				isPushed = false;
				isCleared = false;
				renderer.material.color = lightRedColor;
				phoneButtonManager = transform.parent.gameObject;
		}

		void OnTriggerEnter (Collider other) {
				if (isCleared == false) {
						if (isPushed == false) {
								renderer.material.color = lightGreenColor;
								isPushed = true;
								phoneButtonManager.SendMessage ("CheckSequence", id);
						}
				}
		}

		// ボタンの状態をリセットする時に呼ばれる
		void Reset() {
				renderer.material.color = lightRedColor;
				isPushed = false;
		}

		// 仕掛けがクリアされた時に呼ばれる
		void Clear() {
				renderer.material.color = gray;
				isCleared = true;
		}
}
