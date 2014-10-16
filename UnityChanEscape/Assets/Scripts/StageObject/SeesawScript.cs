using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

// シーソーを動かすスクリプト
// 現在はZ軸方向に並行なシーソーにのみ対応している
public class SeesawScript : MonoBehaviour {

	List<GameObject> onBoardChracters; // シーソー上のオブジェクトのリスト
	public int maxAngle = 15;
	int nowAngle; // 現在のシーソーの角度

	// Use this for initialization
	void Start () {
		onBoardChracters = new List<GameObject> ();
		nowAngle = 15;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
		{
				int massSum = 0; // シーソーにかかる重さの和
				
				// シーソー上からいなくなったオブジェクトをリストから削除　
				for (int i = 0; i < onBoardChracters.Count; i++) {
						if (!IsOnBoard (onBoardChracters [i])) {
								onBoardChracters.Remove(onBoardChracters[i]);
						}
				}

				// シーソー上のオブジェクトによる重さの和を算出
				// 簡単のために中心からの距離では力を変化させないようにしている				
				for (int i = 0; i < onBoardChracters.Count; i++) {
						GameObject go = onBoardChracters [i];

						// オブジェクトのZ座標に従って力のかかる向きを決める
						if (onBoardChracters [i].name.IndexOf ("BoxUnityChan") >= 0) {
								massSum -= (int)(Mathf.Sign (go.transform.position.z - transform.position.z) * go.rigidbody.mass); // ボックスユニティちゃんは重さが逆向き
						} else {
								massSum += (int)(Mathf.Sign (go.transform.position.z - transform.position.z) * go.rigidbody.mass);
						}
				}

				// かかる力が0の時にシーソーが動かないようにする
				if(massSum == 0) return;
				
				// シーソーの角度が変わった際の処理
				if (nowAngle != (int)(Mathf.Sign (massSum) * maxAngle)) {
						// シーソーの角度を変更
						SetEulerAngleX (transform, Mathf.Sign (massSum) * maxAngle);
						// シーソー上のオブジェクトの位置を調整　
						for (int i = 0; i < onBoardChracters.Count; i++) {
								float localZ = onBoardChracters [i].transform.position.z - transform.position.z;
								setY (onBoardChracters [i].transform, transform.position.y - localZ * Mathf.Tan (Mathf.PI / 180.0f * (Mathf.Sign (massSum) * maxAngle)));
						}
						nowAngle = (int)(Mathf.Sign(massSum) * maxAngle);
				}
	}

	// transform.positionのyに値を入れる
	void setY (Transform tf, float y)
	{
		Vector3 pos = tf.position;
		tf.position = new Vector3(pos.x, y, pos.z);
	}
	
	// transform.eulerAnglesのxに値を入れる
	void SetEulerAngleX (Transform tf, float x)
	{
		Vector3 ea = tf.eulerAngles;
		tf.eulerAngles = new Vector3(x, ea.y, ea.z);
	}
	
	// シーソーに新しく乗ったオブジェクトをリストに加える　
	// シーソーの端で乗った判定にするとキャラが落ちる可能性があるので、ある程度中まで移動した後で乗った判定を出す
	void OnCollisionStay (Collision c)
	{		
			if (onBoardChracters.IndexOf (c.gameObject) == -1 && IsOnBoard(c.gameObject)) {
					onBoardChracters.Add (c.gameObject);
			}
	}
	
	// オブジェクトがシーソーの上にあるかどうか
	bool IsOnBoard (GameObject go)
	{
			float localX = go.transform.position.x - transform.position.x;
			float localZ = go.transform.position.z - transform.position.z;
			return (Mathf.Abs(localZ) < transform.localScale.z / 2 - 1) && (Mathf.Abs(localX) < transform.localScale.x / 2); // シーソーの端ではfalseを返すようにする
	}

}
