using UnityEngine;
using System.Collections;

// キー入力を抽象化し，設定によってキー割り当てを変更できるようにするためのクラス
public class KeyInputManager : MonoBehaviour {

		// 必要に応じてこれらのクラス変数を呼び出し，キー割り当てを変更する
		// 十字キーは基本的に割り当てを変更しない
		public static KeyCode jumpKeyCode = KeyCode.Z; // ジャンプキー
		public static KeyCode subKeyCode = KeyCode.X; // サブキー
		public static KeyCode changeCharacterKeyCode = KeyCode.C; // キャラクターチェンジキー
		public static KeyCode cameraLeftRotateKeyCode = KeyCode.Alpha1; // カメラを左回転させるキー
		public static KeyCode cameraRightRotateKeyCode = KeyCode.Alpha2; // カメラを右回転させるキー
		public static KeyCode cameraFirstPersonKeyCode = KeyCode.W; // 主眼カメラに切り替えるキー
		public static KeyCode cameraBackKeyCode = KeyCode.A; // カメラ背面に移動させるキー
		public static KeyCode pauseKeyCode = KeyCode.P; // ポーズキー
		
		// Use this for initialization
		void Start () {
	
		}

}
