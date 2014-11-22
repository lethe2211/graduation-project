using UnityEngine;
using System.Collections;

// ステージセレクト画面の画像のコントローラ
public class StageImageController : MonoBehaviour
{
		SpriteRenderer spriteRenderer; // スプライト
		Color color;
		StageSelect stageSelect; // ステージ選択用のスクリプト
		int imageNum; // アタッチされている画像(スプライト)の番号

		SaveDataAnalyzer saveDataAnalyzer; // セーブデータを扱うクラス
		StageInfo stageInfo; // このステージに関する情報

		// Use this for initialization
		void Start ()
		{
				spriteRenderer = GetComponent<SpriteRenderer> ();
				stageSelect = GameObject.Find ("GameManager").GetComponent<StageSelect> ();
				saveDataAnalyzer = SaveDataAnalyzer.GetInstance ();

				imageNum = int.Parse (this.name.Substring (this.name.Length - 1)); // アタッチされているゲームオブジェクトの名前から番号を取得

				stageInfo = saveDataAnalyzer.GetStageInfo (imageNum);
		}

		// Update is called once per frame
		void Update ()
		{
				// ステージが出現しているかどうか
				if (stageInfo.isAppeared) {
						if (imageNum == StageNoManager.stageNo()) {
								color = new Color (1.0f, 1.0f, 1.0f, 1.0f); // なぜか元の画像に色が加算される

						} else {
								color = new Color (0.2f, 0.3f, 0.6f, 0.5f);
						}
						spriteRenderer.color = color;
				} else {
						gameObject.SetActive (false); // 非表示にする
				}

		}
}
