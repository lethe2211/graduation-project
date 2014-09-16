using UnityEngine;
using System.Collections;

public class StageImageController : MonoBehaviour
{
		SpriteRenderer spriteRenderer; // スプライト
		Color color;
		StageSelect stageSelect; // ステージ選択用のスクリプト
		int imageNum; // アタッチされている画像(スプライト)の番号

		SaveDataReaderWriter saveDataReaderWriter;
		StageInfo stageInfo; // このステージに関する情報

		// Use this for initialization
		void Start ()
		{

				spriteRenderer = GetComponent<SpriteRenderer> ();
				stageSelect = GameObject.Find ("GameManager").GetComponent<StageSelect> ();

				imageNum = int.Parse (this.name.Substring (this.name.Length - 1)); // アタッチされているゲームオブジェクトの名前から番号を取得

				saveDataReaderWriter = new SaveDataReaderWriter();
				stageInfo = saveDataReaderWriter.GetStageInfo (imageNum);

		}
		// Update is called once per frame
		void Update ()
		{
				// ステージが出現しているかどうか
				if (stageInfo.isAppeared) {
						if (imageNum == stageSelect.selectedStage) {
								color = new Color (1.0f, 1.0f, 1.0f, 1.0f); // なぜか元の画像に色が加算される

						} else {
								color = new Color (0.2f, 0.3f, 0.6f, 0.5f);
						}
						spriteRenderer.color = color;
				} else {
						gameObject.SetActive (false);
				}

		}
}
