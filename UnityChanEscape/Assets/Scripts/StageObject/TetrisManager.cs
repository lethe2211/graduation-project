﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TetrisManager : MonoBehaviour {

		List<string> minoSequence = new List<string>();
		const int HEIGHT = 20;
		const int WIDTH = 10;
		const int posZ = -50;
		List<Vector3> cubePoints = new List<Vector3>(); // キューブの存在する座標を保持
		GameObject operatedMino; // 操作中のテトリミノ
		int minoCount = 0; // 現在操作中のミノの番号
		int frame = 0;
		
		bool pushedDownKey = false;
		bool pushedZKey = false;
		bool pushedXKey = false;
		
		GameObject unityChan;
		GameObject boxUnityChan;
		Camera mainCamera;
		
		void Start () 
		{
				unityChan = GameObject.Find("unitychan");
				boxUnityChan = GameObject.Find("BoxUnityChan");
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				InitializeStage();
		}
		
		// 回転、落下キーはKeyDownで動作するように
		void Update(){
				if(Input.GetKeyDown("down")) pushedDownKey = true;
				if(Input.GetKeyDown("z")) pushedZKey = true;
				if(Input.GetKeyDown("x")) pushedXKey = true;
		}
		
		void FixedUpdate ()
		{
				frame++;

				if (frame % 5 != 0)
						return; // 10フレームごとに動作
				
				// 左右キーでのミノの移動
				if (Input.GetKey ("right")) {
						MoveMino (-1);
				} else if (Input.GetKey ("left")) {
						MoveMino (1);
				}
				
				//Z, Xキーでミノの回転
				if (pushedZKey) {
						RotateMino (1);
						pushedZKey = false;
				} else if (pushedXKey) {
						RotateMino (-1);
						pushedXKey = false;
				}
				
				// 上キーで加速、下キーで真上に落とす
				if (Input.GetKey ("up")) {
						RaiseMino ();
				}
				if (pushedDownKey) {
						GameObject curMino = operatedMino;
						while (operatedMino == curMino) {
								RaiseMino ();
						}
						pushedDownKey = false;
				}
				
				if (frame % 30 == 0) {
						RaiseMino();
				}
				
				
		}
		
		// ステージの初期化
		void InitializeStage ()
		{
				// ユニティちゃん、ボックスユニティちゃんを操作不能に
				unityChan.SendMessage("SetMoveEnabled", false);
				boxUnityChan.SendMessage("SetMoveEnabled", false);
				
				// メインカメラの位置を固定
				mainCamera.transform.position = new Vector3(0, HEIGHT/2, -30);
				
				// ステージサイズに従って枠を配置
				// 上辺
				for (int i = 0; i < WIDTH + 2; i++) {
						Instantiate(Resources.Load("Prefab/Tetris/Flame"), new Vector3(i - WIDTH / 2, HEIGHT + 1, posZ), Quaternion.identity);
						cubePoints.Add(new Vector3(i - WIDTH / 2, HEIGHT + 1, posZ));
				}
				// 側面
				for (int i = 0; i < HEIGHT + 4; i++) {
						Instantiate(Resources.Load("Prefab/Tetris/Flame"), new Vector3(WIDTH / 2 + 1, HEIGHT - i, posZ), Quaternion.identity);
						Instantiate(Resources.Load("Prefab/Tetris/Flame"), new Vector3(- WIDTH / 2, HEIGHT - i, posZ), Quaternion.identity);
						cubePoints.Add(new Vector3(WIDTH / 2 + 1, HEIGHT - i, posZ));
						cubePoints.Add(new Vector3(- WIDTH / 2, HEIGHT - i, posZ));
				}
				
				// ミノの並びを登録
				AddSequence();
				
				// 最初のテトリミノを生成
				createMino();
		}
		
		// テトリミノの並びを追加
		void AddSequence ()
		{
				string[] minos = new string[]{"I", "O", "T", "J", "L", "S", "Z"};
				
				// 文字列をシャッフル
				for (int i = 0; i < minos.Length; i++) {
						string temp = minos[i];
						int randomIndex = Random.Range(i, minos.Length);
						minos[i] = minos[randomIndex];
						minos[randomIndex] = temp;
				}
				
				// 並び替えたミノの列を追加
				for(int i=0; i < minos.Length; i++){
						minoSequence.Add(minos[i]);
				}
		}
		
		// 操作中のミノの位置を上げる
		void RaiseMino ()
		{
				for (int i = 0; i < 4; i++) {
						// すべてのcubeについて、移動可能かを調べる
						Vector3 pos = operatedMino.transform.FindChild ("Cube" + i.ToString ()).transform.position;
						pos.y++;
						
						// cubeが重ならなければ以下の処理を行う
						if (cubePoints.IndexOf (pos) > -1) {
						
								// ミノを固定し、列がそろっていたら消し、新しいミノを操作可能にする
								FixMino ();
								DeleteLines();
								createMino ();
								return;
						}
				}
				
				// ミノを一段上げる
				Vector3 nowPos = operatedMino.transform.position;
				operatedMino.transform.position = new Vector3(nowPos.x, nowPos.y + 1, nowPos.z);
		}
		
		// 左右にミノを移動する
		void MoveMino (int delta)
		{
				for (int i = 0; i < 4; i++) {
						// すべてのcubeについて、移動可能かを調べる
						Vector3 pos = operatedMino.transform.FindChild("Cube" + i.ToString()).transform.position;
						pos.x += delta;

						// cubeが重なったら移動させない
						if(cubePoints.IndexOf(pos) > -1) return;						
				}
				
				// ミノを左右に動かす
				Vector3 nowPos = operatedMino.transform.position;
				operatedMino.transform.position = new Vector3(nowPos.x + delta, nowPos.y, nowPos.z);
		}
		
		// ミノを回転する
		void RotateMino (int delta)
		{
				//Oミノは回さない
				if(operatedMino.name.IndexOf("TetriminoO") > -1) return;
				
				for (int i = 0; i < 4; i++) {
						// すべてのcubeについて、移動可能かを調べる
						Vector3 pos = RotateCube(operatedMino.transform.FindChild ("Cube" + i.ToString ()).transform.localPosition, delta);
						pos += operatedMino.transform.position;

						// cubeが重なったら回転させない
						if (cubePoints.IndexOf (pos) > -1) return;
				}
				
				//ミノを回転させる
				for (int i = 0; i < 4; i++) {
						// すべてのcubeについて、移動可能かを調べる
						Vector3 pos = RotateCube(operatedMino.transform.FindChild ("Cube" + i.ToString ()).transform.localPosition, delta);
						operatedMino.transform.FindChild("Cube" + i.ToString()).transform.localPosition = pos;
				}
		}
		
		// cubeをZ軸中心で回転させる
		Vector3 RotateCube (Vector3 pos, int delta)
		{
				Vector3 result = new Vector3();
				result.x = - pos.y * delta;
				result.y = pos.x * delta;
				return result;
		}
		
		// 新しくミノを生成する
		void createMino ()
		{
				operatedMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Tetrimino" + minoSequence[minoCount]), new Vector3(0, -2, posZ), Quaternion.identity);
		}
		
		// 操作中のミノを固定する
		void FixMino ()
		{
				// 固定するミノの座標を新しく登録
				for (int i = 0; i < 4; i++) {
						cubePoints.Add(operatedMino.transform.FindChild("Cube" + i.ToString()).transform.position);
				}
				minoCount++;
				
				// ミノの列の残り数が少なくなっていたら新しくミノの列を加える
				if(minoSequence.Count - minoCount < 3) AddSequence();
		}
		
		// 揃っているcubeを消す
		void DeleteLines ()
		{
				bool cubesExist = true; // 見ている列にcubeがあるかどうか
				int currentLine = 0; // 何段目を見ているか
				bool lineExists = true; // 見ている列が揃っているか
				// cubeが存在しない列がくるまで繰り返す
				while (cubesExist) {
						cubesExist = false;
						lineExists = true;
						for (int i = -WIDTH / 2 + 1; i < WIDTH / 2 + 1; i++) {
								Vector3 nowPos = new Vector3 (i, HEIGHT - currentLine, posZ);
								if (cubePoints.IndexOf (nowPos) > -1)
										cubesExist = true;
								else
										lineExists = false;
						}
						if (lineExists) {
								DeleteLine (currentLine);
						} else {
								currentLine++;
						}
				}
		}
		
		// 揃っている1列を消す
		void DeleteLine (int curLine)
		{
//				// cubeの存在する座標リストを更新
//				for (int i = 0; i < cubePoints.Count; i++) {
//						if (cubePoints [i].x <= -WIDTH / 2 || cubePoints [i].x > WIDTH / 2)
//								continue; // 側面の壁は排除
//						
//						if (cubePoints [i].y == HEIGHT - curLine)
//								cubePoints.Remove (cubePoints [i]);
//						else if (cubePoints [i].y < HEIGHT - curLine) {
//								Vector3 pos = cubePoints [i];
//								pos.y++;
//								cubePoints [i] = pos;
//						}
//				}
//				
				// ブロックの位置を動かす
				GameObject[] tetriminos = GameObject.FindGameObjectsWithTag ("Tetrimino");
				int childCount = 0;				
				GameObject cube;
				foreach (GameObject go in tetriminos) {
						childCount = 0;
						for (int i = 0; i < 4; i++) {
								try {
										cube = go.transform.FindChild ("Cube" + i.ToString ()).gameObject;
								} catch {
										continue;
								}
								childCount += 1;
								Vector3 pos = cube.transform.position;
								if (pos.y == HEIGHT - curLine) {
										Destroy (cube);
										childCount -= 1;
								}
								if (pos.y < HEIGHT - curLine) {
										pos.y++;
										cube.transform.position = pos;
								}
								if (childCount==0)
										Destroy (go);
						}
				}
				
				// 移動されたブロックの位置を元にcubesPointsを更新
				UpdatePoints();
		}
		
		// ブロックの存在する位置を示すリストを更新
		void UpdatePoints ()
		{
				List<Vector3> results = new List<Vector3>();
				
				// 上辺
				for (int i = 0; i < WIDTH + 2; i++) {
						results.Add(new Vector3(i - WIDTH / 2, HEIGHT + 1, posZ));
				}
				// 側面
				for (int i = 0; i < HEIGHT + 4; i++) {
						results.Add(new Vector3(WIDTH / 2 + 1, HEIGHT - i, posZ));
						results.Add(new Vector3(- WIDTH / 2, HEIGHT - i, posZ));
				}
				
				// ステージ上のテトリミノの位置をすべて取得
				GameObject[] tetriminos = GameObject.FindGameObjectsWithTag("Tetrimino");
				GameObject cube;
				foreach (GameObject go in tetriminos) {
						for (int i = 0; i < 4; i++) {
								try {
										cube = go.transform.FindChild ("Cube" + i.ToString ()).gameObject;
								} catch {
										continue;
								}
								results.Add(cube.transform.position);
						}
				}
				cubePoints = results;
		}
}
