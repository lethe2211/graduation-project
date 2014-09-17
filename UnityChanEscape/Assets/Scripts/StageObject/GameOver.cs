﻿using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public GUIText[] allTexts;
	public GUIText arrow;
	public AudioSource gameOverVoice;
	private int selectedText;
	private int maxTextNum;
	private bool is_over = false;

	GameObject TimerObject;

	// Use this for initialization
	void Start ()
	{
			TimerObject = GameObject.Find ("TimeManager");
			foreach (GUIText gt in allTexts) {
						gt.enabled = false;
			}
			selectedText = 0;
			maxTextNum = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
			//クリア時の処理
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
