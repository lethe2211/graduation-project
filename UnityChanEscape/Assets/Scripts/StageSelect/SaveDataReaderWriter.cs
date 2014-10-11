using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// セーブデータを扱うクラス
public class SaveDataReaderWriter {

		public List<string> header { get; set; } // ヘッダ
		List<List<string>> data; // データ
		string filePath; // セーブデータがあるディレクトリのパス

		// コンストラクタ
		public SaveDataReaderWriter(string fileName) {
				header = new List<string> ();
				data = new List<List<string>> ();
				filePath = @"Assets/CSVFile/";

				LoadFile (fileName);
		}

		// "/Assets/CSVFile"内のファイル名を入力として受け取り，そのファイルを読み込む
		// Ex. sdrw.LoadFile(@"stageinfo.csv");
		public void LoadFile(string fileName) {
				try {
						using (StreamReader sr = new StreamReader(filePath + fileName, Encoding. GetEncoding("utf-8"))) {
								// ヘッダの行の読み込み
								string firstLine = sr.ReadLine();
								string[] headerValue = firstLine.Split(',');
								header.AddRange(headerValue);

								// データの行の読み込み
								while (!sr.EndOfStream) {
										List<string> rowData = new List<string>();
										string line = sr.ReadLine();
										string[] values = line.Split(',');

										for (int i = 0; i < values.Length; i++) rowData.Add(values[i]);
										data.Add(rowData);
								}
						}
				}
				catch(Exception e) {
						Debug.Log (e.Message);
				}

		}

		// すべて読み込む
		public List<List<string>> ReadAll() {
				return data;
		}

		// 1行だけ読み込む
		public List<string> Read(int rowNum) {
				return data [rowNum];
		}

		// 1行単位で書き換える
		public void Update(int rowNum, List<string> rowData) {
				data [rowNum] = rowData;
		}

		// 末尾に付け足す
		public void Add(List<string> rowData) {
				data.Add (rowData);
		}

		// ファイル名を入力として受け取り，データを書き込む
		public void WriteFile(string fileName) {
				try {
						using (StreamWriter sw = new StreamWriter(filePath + fileName, false)) {							
								// ヘッダの行を書き込む
								string firstLine = "";

								for (int i = 0; i < header.Count; i++) {
										if (i == 0) firstLine += header[i];
										else firstLine += "," + header[i];
								}
								sw.WriteLine(firstLine);

								// データの行を書き込む
								for (int i = 0; i < data.Count; i++) {
										string line = "";

										for (int j = 0; j < data[i].Count; j++) {
												if (j == 0) line += data[i][j];
												else line += "," + data[i][j];
										}
										sw.WriteLine(line);
								}							
						}
				}
				catch (Exception e) {
						Debug.Log (e.Message);
				}

		}

		// 行数を返す
		public int CountRow() {
				return data.Count;
		}

		// 列数を返す
		public int CountCol() {
				return header.Count;
		}

//		private Hashtable header = new Hashtable (); // ヘッダ
//		private ArrayList SaveData = new ArrayList(); // セーブデータ 
//		public int count;
//
//		// TODO: セーブデータの生データとstageControllerは分けるべき
//		public List<StageInfo> stageControllers; // セーブデータを扱いやすい形にしたstageController
//
//		public SaveDataReaderWriter() {
//				count = 0;
//				Load ();
//				Analyze ();
//		}
//
//		// セーブデータを読み込む
//		public void Load() {
//				try	{
//						// csvファイルを開く
//						using (var sr = new System.IO.StreamReader(@"Assets/CSVFile/stageInfo.csv")) {
//								var firstLine = sr.ReadLine();
//								string[] headerArray = firstLine.Split(',');
//								for (int i = 0; i < headerArray.Length; i++) {
//										header[headerArray[i]] = i;
//								}
//								for (count = 0; !sr.EndOfStream; count++) {
//										// ファイルから一行読み込む
//										var line = sr.ReadLine();
//										// 読み込んだ一行をカンマ毎に分けて配列に格納する
//										string[] values = line.Split(',');
//										SaveData.Add(values);
//								}
//								
//						}
//				}
//				catch (System.Exception e) {
//						// ファイルを開くのに失敗したとき
//						Debug.Log (e.Message);
//				}
//
//		}
//
//		public void Analyze() {
//
//				stageControllers = new List<StageInfo> ();
//				for (int i = 0; i < this.count; i++) {
//						Hashtable row = ReadRow (i);
//
//						int stageNo = int.Parse ((string)row ["stageNo"]);
//						int world = int.Parse ((string)row ["world"]);
//						bool isAppeared = ((string)row ["isAppeared"] == "1") ? true : false;
//						bool isCleared = ((string)row ["isCleared"] == "1") ? true : false;
//						int deathCount = int.Parse ((string)row ["deathCount"]);
//						int score = int.Parse ((string)row ["score"]);
//						int clearTime = int.Parse ((string)row ["clearTime"]);
//						StageInfo si = new StageInfo (stageNo, world, isAppeared, isCleared, deathCount, score, clearTime);
//						stageControllers.Add (si);
//				}
//
////				foreach (var item in stageControllers) {
////						Debug.Log (item.stageNo);
////				}
//
//		}
//
//		public Hashtable ReadHeader () {
//				return header;
//		}
//
//		// 行のインデックス，ヘッダのキーを入力として受け取り，該当するセーブデータの値を返す
//		// 先にLoad()の呼び出しが必要
//		public string Read(int rowNum, string key) {
//				string[] row = (string[])SaveData [rowNum];
//				return row [(int)header [key]];
//		}
//
//
//		public Hashtable ReadRow(int rowNum) {
//				Hashtable result = new Hashtable ();
//				string[] row = (string[])SaveData [rowNum];
//				foreach (DictionaryEntry h in header) {
//						result [h.Key] = row [(int)header[h.Key]];
//				}
//				return result;
//		}
//				
//		// 全ステージの情報をStageInfoクラスのリストとして返す
//		public List<StageInfo> GetAllStageInfo() {
//				return stageControllers;
//		}
//
//		// ステージの情報をStageInfoクラスのインスタンスとして返す
//		public StageInfo GetStageInfo(int stageNo) {
//				StageInfo si = stageControllers [stageNo - 1];
//				return si;
//		}
//
//
//		// 行のインデックス，ヘッダのキー，書き換えたい値を入力として受け取り，セーブデータの値を書き換える
//		// 先にLoad()の呼び出しが必要
//		public string Write(int rowNum, string key, string value) {
//
//				string[] row = (string[])SaveData [rowNum];
//				row [(int)header [key]] = value;
//				SaveData[rowNum] = row;
//
//				try {
//						// appendをtrueにすると，既存のファイルに追記
//						//         falseにすると，ファイルを新規作成する
//						var append = false;
//						// 出力用のファイルを開く
//						using (var sw = new System.IO.StreamWriter(@"Assets/CSVFile/stageinfo.csv", append)) {
//							
//								string firstLine = "";
//								int index = 0;
//								for(int i = 0; i < header.Count; i++) {
//										if (i == 0) {
//												foreach (string k in header.Keys) {
//														if (i == (int)header[k]) firstLine += k;
//												}
//										}
//										else {
//												foreach (string k in header.Keys) {
//														if (i == (int)header[k]) firstLine += "," + k;
//												}
//										}
//								}
//								sw.WriteLine(firstLine);
//
//								foreach (string[] data in SaveData) {
//										string line = "";
//										int i = 0;
//
//										foreach(string s in data) {
//												if (i == 0) line += s;
//												else line += "," + s;
//												i += 1;
//										}
//										sw.WriteLine(line);
//								}
//						}
//				}
//				catch (System.Exception e) {
//						// ファイルを開くのに失敗したときエラーメッセージを表示
//						System.Console.WriteLine(e.Message);
//						return null;
//				}
////				Debug.Log ("Saved!");
//
//				return value;
//		}
				
}
