using UnityEngine;
using System.Collections;

public class stageImageController : MonoBehaviour
{
		SpriteRenderer spriteRenderer; // スプライト
		Color color;
		StageSelect stageSelect; // ステージ選択用のスクリプト
		int imageNum; // アタッチされている画像(スプライト)の番号

		// Use this for initialization
		void Start ()
		{
	
				spriteRenderer = GetComponent<SpriteRenderer> ();
				stageSelect = GameObject.Find ("GameManager").GetComponent<StageSelect> ();

				imageNum = int.Parse (this.name.Substring (this.name.Length - 1)); // アタッチされているゲームオブジェクトの名前から番号を取得

		}
		// Update is called once per frame
		void Update ()
		{

				Debug.Log (stageSelect.stage);
				print (imageNum);

				if (imageNum == stageSelect.stage) {

						color = new Color (1.0f, 1.0f, 1.0f, 1.0f); // なぜか元の画像に色が加算される

				} else {

						color = new Color (0.2f, 0.3f, 0.6f, 0.5f);

				}

				spriteRenderer.color = color;
	
		}
}
