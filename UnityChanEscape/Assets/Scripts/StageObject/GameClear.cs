﻿using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

	public GUIText[] allTexts;
	public GUIText arrow;
	private int selectedText;
	private int maxTextNum;
	public AudioSource clearVoice;
	private bool isCleared = false;
	string loadedLevelName; // 現在のシーン名
	int currentStageNo; // 現在のステージ番号
	public bool isDebug = false; // デバッグモードかどうか

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
			try {
					currentStageNo = int.Parse (loadedLevelName.Substring (5));
			} catch {
					isDebug = true;
			}
			saveDataAnalyzer = SaveDataAnalyzer.GetInstance ();
	}
	
	// Update is called once per frame
	void Update ()
		{
				//クリア時の処理
				if (isCleared) {
						
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
							if (!isDebug) { //デバッグモードでなければセーブデータを更新
									StageInfo stageInfo = saveDataAnalyzer.GetStageInfo (currentStageNo);
									stageInfo.isCleared = true; // クリアした

									// ここにステージ解放の処理を書く

									saveDataAnalyzer.UpdateStageInfo (currentStageNo, stageInfo);
									saveDataAnalyzer.WriteStageInfo ();
							}
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

	void Cleared ()
	{
			TimerObject.SendMessage ("Stop");
			foreach (GUIText gt in allTexts) {
						gt.enabled = true;
			}
			clearVoice.Play();
			isCleared = true;
	}
}
