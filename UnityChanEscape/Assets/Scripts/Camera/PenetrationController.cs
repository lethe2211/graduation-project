using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

// カメラとキャラクターの間に存在するオブジェクトを非表示にする
public class PenetrationController : MonoBehaviour {

		Camera mainCamera;
		Camera subCamera;
		GameObject unityChan;
		GameObject boxUnityChan;
		List<GameObject> disabledObjects;
		private Shader nShader;
		private Shader pShader;

		void Start ()
		{
				mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera> ();
				subCamera = GameObject.Find ("SubCamera").GetComponent<Camera> ();
				unityChan = GameObject.Find ("unitychan");
				boxUnityChan = GameObject.Find ("BoxUnityChan");
				disabledObjects = new List<GameObject> ();
				nShader = Shader.Find("Bumped Specular");
				pShader = Shader.Find("Transparent/Bumped Specular");
		}
	
		void FixedUpdate ()
		{
				Vector3 cameraPosition;
				Vector3 characterPosition;
				Vector3 direction;
				float distance;
				
				// 非表示にしていたオブジェクトを元に戻す
				for (int i = 0; i < disabledObjects.Count; i++) {
						disabledObjects [i].renderer.material.shader = nShader;
				}
				
				// 操作中のキャラクターと利用中のカメラの位置を取得
				if (mainCamera.enabled) {
						cameraPosition = mainCamera.transform.position;
						characterPosition = unityChan.transform.position + unityChan.transform.up.normalized;
				} else if (subCamera.enabled) {
						cameraPosition = subCamera.transform.position;
						characterPosition = boxUnityChan.transform.position + boxUnityChan.transform.up.normalized;
				} else
						return;

				// キャラクターとカメラ間の方向ベクトルと距離を算出
				direction = (characterPosition - cameraPosition).normalized;
				cameraPosition -= direction * 2; // カメラ位置を少し後ろに移動
				distance = Vector3.Distance (characterPosition, cameraPosition);
				
				// デバッグ用にRayを赤色でシーンビューに表示
				Ray ray = new Ray (cameraPosition, direction);
				Debug.DrawRay (ray.origin, ray.direction * distance, Color.red);
				
				// カメラとキャラクターの間に存在するオブジェクトを全て取得
				RaycastHit[] hits = Physics.RaycastAll (cameraPosition, direction, distance);
				hits = hits.OrderBy (h => h.distance).ToArray (); // 距離順にソート
				foreach (RaycastHit hit in hits) {
						// レンダラーを非表示に
						if (hit.collider.gameObject.renderer && canBePenetrated(hit.collider.gameObject)) {
								hit.collider.gameObject.renderer.material.shader = pShader;
								if (disabledObjects.IndexOf (hit.collider.gameObject) == -1) {
										disabledObjects.Add (hit.collider.gameObject); // 非表示済みオブジェクトに登録
								}
						}
				}
		}
		
		// オブジェクトが透過可能かどうか
		// 現状はPlateとWallを透過可能に設定している
		bool canBePenetrated (GameObject go)
		{
				return go.name.IndexOf("Plate") > -1 || go.name.IndexOf("Wall") > -1;
		}
		
		// gameObjectのcolorのアルファ値を変更する
		// 今は使ってない
		void SetAlpha (GameObject go, float alpha)
		{
				Color color = go.renderer.material.color;
				color.a = alpha;
				go.renderer.material.color = color;
		}
}
