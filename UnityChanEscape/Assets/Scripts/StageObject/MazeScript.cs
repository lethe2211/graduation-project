using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeScript : MonoBehaviour {

		int placedNum; // 壁が配置されている座標数
		bool[,] isWall; // 壁が配置されているかどうかを示すbool
		List<Vector2> startpoints; // 壁生成の開始点
		int wayWidth = 5;
		
		// Use this for initialization
		void Start () {
			int size = 11;
			isWall = new bool[size, size];
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
				
				// 壁の初期化
				for (int i = 0; i < size; i++) {
						for (int j = 0; j < size; j++) {
								if (i == 0 || i == size - 1 || j == 0 || j == size - 1) {
										isWall [j, i] = true; // 外壁のみtrue
										if (i > 0 || i < size - 1 || j > 0 || j < size - 1) {
												startpoints.Add (new Vector2 (j, i));
										}
								} else {
										isWall [j, i] = false;
								}
						}
				}
				
				// 簡単にならないように最初の壁の開始点は指定する
				SetWall(new Vector2(0, (size-1)/2), 0);
				
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
						wall.transform.position = new Vector3(-((e.x + s.x)/2)*wayWidth, 5, -s.y * wayWidth);
						wall.transform.localScale = new Vector3((e.x - s.x)*wayWidth, 10, 0.2f);
				} else {
						// 縦長の壁
						wall.transform.position = new Vector3(-s.x * wayWidth, 5, -((e.y + s.y)/2)*wayWidth);	
						wall.transform.localScale = new Vector3(0.2f, 10, (e.y - s.y)*wayWidth);
				}
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
}
