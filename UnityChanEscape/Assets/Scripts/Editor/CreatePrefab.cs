using UnityEngine;
using UnityEditor;
using System.Collections;

// Resourcesフォルダ内のprefabのインスタンスを生成するエディタ拡張
public class CreatePrefab : ScriptableWizard {

		private static string [] prefabs; // Resourcesフォルダ内のprefab名のリスト
		private static int selectIndex = 0;
		
		[MenuItem ("GameObject/Create Other/Prefab")]
		static void Init ()
		{
				// Resoursesフォルダ内のprefabファイル名をすべて取得
				prefabs = System.IO.Directory.GetFiles (@"Assets/Resources/Prefab", "*.prefab", System.IO.SearchOption.TopDirectoryOnly);
				for (int i = 0; i < prefabs.Length; i++) {
						prefabs[i] = prefabs[i].Split("/"[0])[3].Split("."[0])[0];
				}
				EditorWindow.GetWindow<CreatePrefab>(true, "Create Prefab");
    	}

		void OnGUI() {
				try {
						selectIndex = EditorGUILayout.Popup(selectIndex, prefabs);
				       	if (GUILayout.Button("Create")) Create();
    				} catch (System.FormatException) {}
		}
		
		static void Create ()
		{
				// 選択されたprefabのインスタンスを生成
				GameObject go = Instantiate((GameObject)Resources.Load ("Prefab/" + prefabs[selectIndex])) as GameObject;
				go.transform.position = new Vector3(0, 0, 0);
		}
}
