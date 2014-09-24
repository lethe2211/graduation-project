using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public GUIText[] allTexts;
	public GUIText arrow;
	public AudioSource gameOverVoice;
	private int selectedText;
	private int maxTextNum;
	private bool is_over = false;
	string loadedLevelName; // 現在のシーン名
	int currentStageNo; // 現在のステージID

	GameObject TimerObject;
	SaveDataAnalyzer saveDataAnalyzer;

	// Use this for initialization
	void Start ()
	{
			TimerObject = GameObject.Find ("TimeManager");
			foreach (GUIText gt in allTexts) {
						gt.enabled = false;
			}
			selectedText = 0;
			maxTextNum = 1;
			loadedLevelName = Application.loadedLevelName;
			currentStageNo = int.Parse (loadedLevelName.Substring (5));
			saveDataAnalyzer = SaveDataAnalyzer.GetInstance ();
	}
	
	// Update is called once per frame
	void Update ()
	{
			//ゲームオーバー時の処理
			if (is_over) {
						
					// 上キー
					if (Input.GetKeyDown ("up")) {
							if (selectedText > 0) {
									selectedText -= 1;
							} else {
									selectedText = maxTextNum;
							}

					}
	
					// 下キー
					if (Input.GetKeyDown ("down")) {
							if (selectedText < maxTextNum) {
									selectedText += 1;
							} else {
									selectedText = 0;
							}

					}
					
					// 矢印の位置を変更
					arrow.transform.position = new Vector3 (0.25f, 0.5f - 0.1f * selectedText, 0f);
					
					// zキー
					if (Input.GetKeyDown ("z")) {
							StageInfo stageInfo = saveDataAnalyzer.GetStageInfo (currentStageNo);
							stageInfo.deathCount += 1; // 死亡カウントを1増やす
							saveDataAnalyzer.UpdateStageInfo (currentStageNo, stageInfo);
							saveDataAnalyzer.WriteStageInfo ();

							switch (selectedText) {
							case 0:
									Application.LoadLevel (Application.loadedLevel);
									Time.timeScale = 1f;
									break;
							case 1:
									Application.LoadLevel ("StageSelect");
									Time.timeScale = 1f;
									break;
							}
					}
			}
	}

	void Over ()
	{
			TimerObject.SendMessage ("Stop");
			foreach (GUIText gt in allTexts) {
						gt.enabled = true;
			}
			gameOverVoice.Play();
			is_over = true;
	}
}
