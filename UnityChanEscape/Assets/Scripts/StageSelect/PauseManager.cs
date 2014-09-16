﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseManager: MonoBehaviour {

		public int selectedText; // 今選択されているステージのID
		public GUIText[] textList; // 選択できるテキストのリスト
		public GUIText arrow; // 矢印
		public GUIText[] allTexts; // ポーズ中の全テキスト
		public GUITexture manual; // 操作マニュアル
		private int maxTextNum; // ステージIDの最大値
		protected bool isPaused; // ポーズ中かどうかを表すフラグ

		// Use this for initialization
		void Start ()
		{	
				selectedText = 0;
				maxTextNum = textList.Length - 1;
				isPaused = false;
				foreach (GUIText gt in allTexts) {
						gt.enabled = false;
				}
				manual.enabled = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// ポーズ中でなければPボタンでポーズ
				if (!isPaused) {
						if (Input.GetKeyDown ("p")) {
								Time.timeScale = 0f;
								isPaused = true;
								foreach (GUIText gt in allTexts) {
										gt.enabled = true;
								}
								manual.enabled = true;
						}

				} else {
			
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
						
						arrow.transform.position = new Vector3 (0.25f, 0.5f - 0.1f * selectedText, 0f);
						
						// zキー
						if (Input.GetKeyDown ("z")) {
								switch (selectedText) {
									case 0:
										foreach (GUIText gt in allTexts) {
											gt.enabled = false;
										}
										manual.enabled = false;
										Time.timeScale = 1f;
										isPaused = false;
										break;
									case 1:
										Application.LoadLevel(Application.loadedLevel);
										Time.timeScale = 1f;
										break;
									case 2:
										Application.LoadLevel("StageSelect");
										Time.timeScale = 1f;
										break;
								}
						}
				}
		}
}
