using UnityEngine;
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
		GameObject nextMino; // 次のテトリミノ
		GameObject holdedMino; // ホールド中のテトリミノ
		int minoCount = 0; // 現在操作中のミノの番号
		int minoState = 0; // ミノの回転状態
		int frame = 0;
		bool isPlayingTetris = true;
		bool tspinDone = false;
		
		bool pushedDownKey = false;
		bool pushedZKey = false;
		bool pushedXKey = false;
		bool pushedCKey = false;
		bool holdEnable = true;
		bool downKeyPressed = false;
		int dv = 0; // 上下の変位
		
		GameObject unityChan;
		GameObject boxUnityChan;
		Camera mainCamera;
		Camera subCamera;
		Camera tetrisCamera;
		GameObject gameOverObject;
		
		// スーパーローテーション用の辞書
		Dictionary<string, List<List<Vector2>>> SRS = new Dictionary<string, List<List<Vector2>>>();
		
		GUIText scoreText;
		int score = 0;
		
		void Start () 
		{
				unityChan = GameObject.Find("unitychan");
				boxUnityChan = GameObject.Find("BoxUnityChan");
				mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
				subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
				tetrisCamera = GameObject.Find("TetrisCamera").GetComponent<Camera>();
				scoreText = GameObject.Find("ScoreText").guiText;
				gameOverObject = GameObject.Find("GameOverObject");
				registerSRS();
				InitializeStage();
		}
		
		// 回転、落下、ホールドキーはKeyDownで動作するように
		void Update(){
				
				if(!isPlayingTetris) return;
				
				DownPressed();
//				if(Input.GetKeyDown("down") || downKeyPressed) pushedDownKey = true;
				if(Input.GetKeyDown(KeyInputManager.jumpKeyCode) || Input.GetButtonDown("subButton")) pushedZKey = true;
				if(Input.GetKeyDown(KeyInputManager.subKeyCode) || Input.GetButtonDown("jumpButton")) pushedXKey = true;
				if(Input.GetKeyDown(KeyInputManager.changeCharacterKeyCode) || Input.GetButtonDown("changeCharacterButton")) pushedCKey = true;
		}
		
		void FixedUpdate ()
		{
				if(!isPlayingTetris) return;
				
				frame++;

				if (frame % 5 != 0)
						return; // 10フレームごとに動作
				
				// 左右キーでのミノの移動
				if (Input.GetAxis("Horizontal") > 0) {
						MoveMino (-1);
				} else if (Input.GetAxis("Horizontal") < 0) {
						MoveMino (1);
				}
				
				// Z, Xキーでミノの回転
				if (pushedZKey) {
						RotateMino (-1);
						pushedZKey = false;
				} else if (pushedXKey) {
						RotateMino (1);
						pushedXKey = false;
				}
				
				// 上キーで加速、下キーで真上に落とす
				if (Input.GetAxis("Vertical") > 0) {
						RaiseMino ();
				}
				if (pushedDownKey) {
						GameObject curMino = operatedMino;
						while (operatedMino == curMino) {
								RaiseMino ();
						}
						pushedDownKey = false;
				}
				
				// Cキーでホールド
				if (pushedCKey) {
						holdMino();
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
				
				// メイン、サブカメラを使用不能に
				mainCamera.enabled = false;
				subCamera.enabled = false;
				
				// テトリスカメラの位置を固定
				tetrisCamera.transform.position = new Vector3(0, HEIGHT/2, -30);
				
				// ステージサイズに従って枠を配置
				// 上辺
				GameObject frame;
				for (int i = 0; i < WIDTH + 2; i++) {
						frame = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Frame"), new Vector3(i - WIDTH / 2, HEIGHT + 1, posZ), Quaternion.identity);
						frame.transform.parent = transform;
						cubePoints.Add(new Vector3(i - WIDTH / 2, HEIGHT + 1, posZ));
				}
				// 側面
				for (int i = 0; i < HEIGHT + 4; i++) {
						frame = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Frame"), new Vector3(WIDTH / 2 + 1, HEIGHT - i, posZ), Quaternion.identity);
						frame.transform.parent = transform;
						frame = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Frame"), new Vector3(- WIDTH / 2, HEIGHT - i, posZ), Quaternion.identity);
						frame.transform.parent = transform;
						cubePoints.Add(new Vector3(WIDTH / 2 + 1, HEIGHT - i, posZ));
						cubePoints.Add(new Vector3(- WIDTH / 2, HEIGHT - i, posZ));
				}
				
				// ゲームオーバーになるラインを描画
				DrawGameOverLine();
				
				// ミノの並びを登録
				AddSequence();
				
				// 最初のテトリミノを生成
				createMino();
		}
		
		//ゲームオーバーラインの描画
		void DrawGameOverLine ()
		{
				LineRenderer line = this.gameObject.AddComponent<LineRenderer>();
				line.SetPosition(0, new Vector3(- WIDTH / 2, 0.5f, posZ));
				line.SetPosition(1, new Vector3(WIDTH / 2 + 1, 0.5f, posZ));
				line.SetWidth(0.1f, 0.1f);
				line.material.color = Color.red;
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
						
						// cubeが重なれば以下の処理を行う
						if (cubePoints.IndexOf (pos) > -1) {
								
								// ミノを固定し、列がそろっていたら消し、新しいミノを操作可能にする
								FixMino ();
								DeleteLines ();
								createMino ();
								tspinDone = false;
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
				if (operatedMino.name.IndexOf ("TetriminoO") > -1)
						return;
				
				int srsIndex = 0;
				
				string srsType;
				if (operatedMino.name.IndexOf ("TetriminoI") > -1) srsType = "I" + delta.ToString();
				else srsType = "T";
				
				while (srsIndex < 5) {
						bool rotateEnable = true;
						float tmpx;
						for (int i = 0; i < 4; i++) {
								// すべてのcubeについて、移動可能かを調べる
								Vector3 pos = RotateCube (operatedMino.transform.FindChild ("Cube" + i.ToString ()).transform.localPosition, delta);
								tmpx = SRS[srsType][minoState][srsIndex].x;
								if(minoState % 2 == 0) tmpx *= delta;
								pos.x += tmpx;
								pos.y += SRS[srsType][minoState][srsIndex].y;
								if(srsType == "I1" || srsType == "I-1") pos += rotateForI(delta);
								pos += operatedMino.transform.position;

								// cubeが重なったら回転させない
								if (cubePoints.IndexOf (pos) > -1) {
										rotateEnable = false;
										break;
								}
						}
						if (rotateEnable) {
								// ミノを回転させる
								for (int i = 0; i < 4; i++) {
										Vector3 pos = RotateCube(operatedMino.transform.FindChild ("Cube" + i.ToString ()).transform.localPosition, delta);
										operatedMino.transform.FindChild("Cube" + i.ToString()).transform.localPosition = pos;
								}
								
								// SRSの分だけミノを移動させる
								Vector3 mpos = operatedMino.transform.position;
								tmpx = SRS[srsType][minoState][srsIndex].x;
								if(minoState % 2 == 0) tmpx *= delta;
								mpos.x += tmpx;
								mpos.y += SRS[srsType][minoState][srsIndex].y;
								if(srsType == "I1" || srsType == "I-1") mpos += rotateForI(delta);
								operatedMino.transform.position = mpos;
								
								// ミノの状態を更新
								if(minoState == 3 && delta == 1) minoState = 0;
								else if(minoState == 0 && delta == -1) minoState = 3;
								else minoState += delta;
								
								// Tスピンが行われた場合にはフラグをtrueに
								if(operatedMino.name.IndexOf("TetriminoT") > -1 && CheckTspin()) tspinDone = true;
								if(tspinDone) Debug.Log("TSpin!");
								
								return;

						} else {
								srsIndex += 1;
						}
				}
				Debug.Log("State:" + minoState.ToString() + ",key:" + delta.ToString());				
		}
		
		// cubeをZ軸中心で回転させる
		Vector3 RotateCube (Vector3 pos, int delta)
		{
				Vector3 result = new Vector3 ();
				result.x = -pos.y * delta;
				result.y = pos.x * delta;
				return result;
		}
		
		// Iミノのみ回転の仕様が異なるため、その修正
		Vector3 rotateForI (int delta)
		{
				int[] patterns = new int[]{0,2,0,1,1,0,2,0};
				switch (patterns[(delta+1)*2 + minoState]) {
				case 0:
						return new Vector3(0, 0, 0);
				case 1:
						return new Vector3(-1, 1, 0);
				case 2:
						return new Vector3(1, -1, 0);
				default:
						return new Vector3(0, 0, 0);
				}
		}
		
		// Tspin判定
		bool CheckTspin ()
		{
				int cubeNum = 0;
				Vector3 pos = operatedMino.transform.position;
				for (int i = -1; i < 2; i += 2) {
						for (int j = -1; j < 2; j += 2) {
								if(cubePoints.IndexOf(new Vector3(pos.x + j, pos.y + i, posZ)) > -1) cubeNum++;
						}
				}
				return cubeNum > 2;
		}
		
		// 新しくミノを生成する
		void createMino ()
		{
				if(nextMino) Destroy(nextMino);
				
				operatedMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Tetrimino" + minoSequence[minoCount]), new Vector3(1, -2, posZ), Quaternion.identity);
				operatedMino.transform.parent = transform;
				
				nextMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/Tetrimino" + minoSequence[minoCount+1]), new Vector3(- WIDTH / 2 - 7, 0, posZ), Quaternion.identity);
				nextMino.transform.parent = transform;
				
				holdEnable = true;
				pushedCKey = false;
				minoState = 0;
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
				int deletedLineNum = 0; // 消えたライン数
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
								deletedLineNum++;
						} else {
								currentLine++;
						}
				}
				if (deletedLineNum > 0)
						AddScore (deletedLineNum);
				// cubeがラインを超えるとゲームを終了
				if (currentLine - deletedLineNum - 1 > HEIGHT) {
						isPlayingTetris = false;
						gameOverObject.SendMessage("Over");
				}
		} 
		
		// 揃っている1列を消す
		void DeleteLine (int curLine)
		{
				// cubeの存在する座標リストを更新
				for (int i = cubePoints.Count - 1; i > -1; i--) {
						if (cubePoints [i].x <= -WIDTH / 2 || cubePoints [i].x > WIDTH / 2)
								continue; // 側面の壁は排除
						
						if (cubePoints [i].y == HEIGHT - curLine) {
								cubePoints.Remove (cubePoints [i]);
						} else if (cubePoints [i].y < HEIGHT - curLine) {
								Vector3 pos = cubePoints [i];
								pos.y++;
								cubePoints [i] = pos;
						}
				}

				// ブロックの位置を動かす
				GameObject[] tetriminos = GameObject.FindGameObjectsWithTag ("Tetrimino");
				int childCount = 0;				
				GameObject cube;
				for (int j = 0; j < tetriminos.Length; j++) {
						
						if (tetriminos[j].transform.position.x <= -WIDTH / 2 || tetriminos [j].transform.position.x > WIDTH / 2)
								continue; // ホールド、ネクストは対象外

						childCount = 0;
						for (int i = 0; i < 4; i++) {
								try {
										cube = tetriminos[j].transform.FindChild ("Cube" + i.ToString ()).gameObject;
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
						}
						if (childCount == 0) {
										Destroy (tetriminos[j]);
						}
				}
		}
		
		// 消したライン数だけスコアを加算
		void AddScore (int n)
		{
				if(tspinDone) n *= 2;
				int scoreDelta = 50;
				while (n > 0) {
						scoreDelta *= n--;
				}
				score += scoreDelta;
				scoreText.text = score.ToString();			
				tspinDone = false;
		}
		
		// ミノをホールド
		void holdMino ()
		{
				if(!holdEnable) return;
				
				if (!holdedMino) {
						holdedMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/" + operatedMino.name.Split("("[0])[0]), new Vector3(WIDTH / 2 + 7, 0, posZ), Quaternion.identity);
						Destroy(operatedMino);
						minoCount++;
						createMino ();
				} else {
						GameObject tmp = holdedMino;
						holdedMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/" + operatedMino.name.Split("("[0])[0]), new Vector3(WIDTH / 2 + 7, 0, posZ), Quaternion.identity);
						Destroy(operatedMino);
						operatedMino = (GameObject)Instantiate(Resources.Load("Prefab/Tetris/" + tmp.name.Split("("[0])[0]), new Vector3(1, -2, posZ), Quaternion.identity);
						Destroy(tmp);
				}
				pushedCKey = false;
				holdEnable = false;
				minoState = 0;
		}
		
		// SRSでの移動位置を登録
		void registerSRS ()
		{
				string[] minos = new string[]{"I1", "I-1", "T"};
				// 各ミノの回転ごとのSRSの移動先
				// 第一要素がミノ名、第二要素がミノ状態、第三要素が移動先の座標 
				// 1->4->3->2->1の順
				// 回転キーによる移動先の差はない
				// 登録されているのはZキーによる回転の際のSRS
				// Iミノは回転方向ごとに登録
				// あとのミノは状態0,2のときの逆回転はマイナス
				int[][,] moves = new int[][,] {
						new int[,]{                                   // Iミノ(z回転)
								{0, 0, 2, 0, -1, 0, -1, -2, 2, 1},								
								{0, 0, 2, 0, -1, 0, 2, -1, -1, 2},
								{0, 0, -2, 0, 1, 0, -2, -1, 1, 1},
								{0, 0, -1, 0, 2, 0, -1, -2, 2, 1}
						},
						new int[,]{                                   // Iミノ(x回転)
								{0, 0, 2, 0, -1, 0, -1, -2, 2, 1},								
								{0, 0, 1, 0, -2, 0, 1, -2, -2, 1},
								{0, 0, -2, 0, 1, 0, -2, -1, 1, 1},
								{0, 0, 2, 0, -1, 0, 2, -2, -1, 2}
						},
						new int[,]{                                   // Iミノ以外
								{0, 0, -1, 0, -1, -1, 0, 2, -1, 2},
								{0, 0, 1, 0, 1, 1, 0, -2, 1, -2},
								{0, 0, 1, 0, 1, -1, 0, 2, 1, 2},
								{0, 0, -1, 0, -1, 1, 0, -2, -1, -2}
						}
				};
				for (int i = 0; i < minos.Length; i++) {
						setSRS(minos[i], moves[i]);
				}
		}
		
		// 入力されたミノのSRS異動先を登録
		void setSRS (string mino, int[,] moves)
		{
				SRS [mino] = new List<List<Vector2>> ();
				for (int i = 0; i < 4; i++) {
						List<Vector2> tmp = new List<Vector2>();
						for(int j=0; j<5; j++){
								Vector2 tmpvec = new Vector2(moves[i,j*2], moves[i,j*2+1]);
								tmp.Add(tmpvec);
						}
						SRS[mino].Add(tmp);
				}
		}
		
	// ゲームパッドでの下キーのKeyDown時にフラグをtrue
	void DownPressed ()
	{
			float v = Input.GetAxisRaw ("Vertical");
			if ((v < 0 & !downKeyPressed)) {
						downKeyPressed = true;
						pushedDownKey = true;
			}
			if(v == 0 & downKeyPressed) downKeyPressed = false;
	}
}