using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeScript : MonoBehaviour {

		int placedNum; // 壁が配置されている座標数
		bool[,] isWall; // 壁が配置されているかどうかを示すbool
		List<Vector2> startpoints; // 壁生成の開始点
		int wayWidth = 5; // 迷路の幅
		int HEIGHT = 20; // 天井の高さ
		int WALLHEIGHT = 10; // 壁の高さ
		
		// Use this for initialization
		void Start () {
			int size = 10; // 迷路の1列のマス数(偶数)
			isWall = new bool[size+1, size+1];
			startpoints = new List<Vector2>();
			GenerateMaze(size);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		// 迷路を作成する
		void GenerateMaze (int size)
		{
				bool isVertical = false; // 縦横どちら向きの壁を配置するか
				
				InitializeOuter(size); // 外壁の場所や床の大きさを調整 
				
				// 壁の初期化
				for (int i = 0; i < size + 1; i++) {
						for (int j = 0; j < size + 1; j++) {
								if (i == 0 || i == size || j == 0 || j == size) {
										isWall [j, i] = true; // 外壁のみtrue
										if (i > 0 || i < size || j > 0 || j < size) {
												startpoints.Add (new Vector2 (j, i));
										}
								} else {
										isWall [j, i] = false;
								}
						}
				}
				
				// 簡単にならないように最初の壁の開始点は指定する
				SetWall(new Vector2(0, size/2), 0);
				
				// 壁の生成
				while (startpoints.Count > 0) {
						Vector2 start = startpoints [Random.Range (0, startpoints.Count - 1)];
						List<int> dirs = GetDirections (start);
						if (dirs.Count > 0) {
								int dir = dirs [Random.Range (0, dirs.Count - 1)];
								SetWall (start, dir);
						} else {
								startpoints.Remove (start);
						}
				}
		}
		
		// 座標(x,y)から壁を伸ばす
		// 壁のばし法を採用
		void SetWall (Vector2 start, int d)
		{
				Vector2[] delta = { new Vector2 (1, 0), new Vector2 (-1, 0), new Vector2 (0, 1), new Vector2 (0, -1) };
				Vector2 end = start + delta [d];

				while (!isWall [(int)end.x, (int)end.y]) {
						isWall [(int)end.x, (int)end.y] = true;
						switch (d) {
							case 0:
								GenerateWall(start, end, true);
								break;
							case 1:
								GenerateWall(end, start, true);
								break;
							case 2:
								GenerateWall(start, end, false);
								break;
							case 3:
								GenerateWall(end, start, false);
								break;								
						}
						startpoints.Add(end);
						List<int> candidates = new List<int>{0,1,2,3};
						candidates.Remove((d%2==0) ? d+1 : d-1);
						d = candidates[Random.Range(0,candidates.Count)];
						start = end;
						end += delta [d];
				}
		}
		
		// 壁Prefabをシーンに生成する
		void GenerateWall (Vector2 s, Vector2 e, bool ish)
		{
				GameObject wall = Instantiate ((GameObject)Resources.Load ("Prefab/MazeWall")) as GameObject;
				wall.gameObject.transform.parent = transform;
				if (ish) {
						// 横長の壁
						wall.transform.position = new Vector3(-((e.x + s.x)/2)*wayWidth, WALLHEIGHT/2, -s.y * wayWidth);
						wall.transform.localScale = new Vector3((e.x - s.x)*wayWidth, WALLHEIGHT, 0.2f);
				} else {
						// 縦長の壁
						wall.transform.position = new Vector3(-s.x * wayWidth, WALLHEIGHT/2, -((e.y + s.y)/2)*wayWidth);	
						wall.transform.localScale = new Vector3(0.2f, WALLHEIGHT, (e.y - s.y)*wayWidth);
				}
				AddLine(wall, s, e);
				wall.name = wall.name.Split("("[0])[0]; // GameObject名に(Clone)がつかないようにする
		}
		
		List<int> GetDirections (Vector2 point)
		{
				List<int> result = new List<int> ();
				Vector2[] delta = { new Vector2 (1, 0), new Vector2 (-1, 0), new Vector2 (0, 1), new Vector2 (0, -1) };
				for (int i = 0; i < delta.Length; i++) {
						Vector2 cur = point + delta[i];
						if(cur.x > -1 && cur.x < isWall.GetLength(1) && cur.y > -1 && cur.y < isWall.GetLength(0) && !isWall[(int)cur.x, (int)cur.y]) result.Add(i);
				}
				return result;
		}
		
		void InitializeOuter (int size)
		{
				int stageSize = wayWidth * size; // ステージの大きさ
				
				// 床と天井の位置の調節
				foreach (GameObject p in GameObject.Find("Plates").GetChildren()) {
						Vector3 curPos = p.transform.position;
						p.transform.position = new Vector3 (-stageSize / 2, curPos.y, -(stageSize) / 2);
						p.transform.localScale = new Vector3 (stageSize, 0.2f, stageSize);
				}
				
				// 外壁の生成
				int[] owx = {0, -stageSize/2, -stageSize, -stageSize/2};
				int[] owz = {-stageSize/2, 0, -stageSize/2, -stageSize};
				for(int i=0; i<4; i++){
						GameObject wall = Instantiate ((GameObject)Resources.Load ("Prefab/MazeWall")) as GameObject;
						wall.gameObject.transform.parent = transform;
						wall.gameObject.name = "OuterWall";
						wall.transform.position = new Vector3(owx[i], HEIGHT/2, owz[i]);
						if(i%2==0) wall.transform.rotation = Quaternion.Euler(0, 90, 0);
						wall.transform.localScale = new Vector3(stageSize, HEIGHT, 0.2f);
				}
				
				// ゴールオブジェクトの位置調整
				GameObject.Find("Goal").transform.position = new Vector3(-wayWidth/2, 1, -(stageSize-wayWidth/2));
		}
		
		// 壁の上部に線を追加する
		void AddLine (GameObject wall, Vector2 s, Vector2 e)
		{
				LineRenderer line = wall.AddComponent<LineRenderer>();
				line.material.color = Color.green;
				line.SetWidth(0.2f, 0.2f);
				line.SetVertexCount(2);
				line.SetPosition(0, new Vector3(-s.x*wayWidth, WALLHEIGHT, -s.y*wayWidth));
				line.SetPosition(1, new Vector3(-e.x*wayWidth, WALLHEIGHT, -e.y*wayWidth));
		}
}
