using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ColorPlateManager : MonoBehaviour {
		
	string[] plateColors; // プレートの色全体
	int linePlates; // 1行に含まれるプレートの数
	float iniZ; // プレートの初期Z座標
	int frame; // フレーム数
	int curCid; // 現在透明になっている色のID

	// Use this for initialization
	void Start () {
		plateColors = new string[]{"Red", "Blue", "Green"};
		linePlates = 10;
		iniZ = -7.5f;
		frame = 0;
		curCid = 0;
				
		SetPlates();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
		{
				if (frame % 90 == 0) {
						// 透明になっているPlateを元に戻す
						GameObject[] plates = GameObject.FindGameObjectsWithTag (plateColors [curCid] + "Plate");
						foreach (GameObject p in plates) {
								p.renderer.material.shader = Shader.Find ("Diffuse");
								p.collider.enabled = true;
						}
						curCid = (curCid + 1) % plateColors.Length;
						// Plateを透明にする
						plates = GameObject.FindGameObjectsWithTag (plateColors [curCid] + "Plate");
						foreach (GameObject p in plates) {
								p.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
						}
				} else if ((frame - 10) % 90 == 0) {
						GameObject[] plates = GameObject.FindGameObjectsWithTag (plateColors [curCid] + "Plate");
						foreach (GameObject p in plates) {
								p.collider.enabled = false;
						}
				}
				frame += 1;	
	}
	
	// ステージ呼び出し時にプレートを配置する
	void SetPlates ()
		{
				for (int i = 0; i < linePlates; i++) {
						string[] colors = plateColors.OrderBy(k => Random.value).ToArray();
						for (int j = 0; j < colors.Length; j++) {
								string plateName = "Prefab/ColorPlate/" + colors[j] + "Plate";
								GameObject plate = Instantiate ((GameObject)Resources.Load (plateName)) as GameObject;
								plate.transform.position = new Vector3(3 * (j - 1), 0, -6 * i + iniZ);
								plate.name = plate.name.Split("("[0])[0];
						}
				}
		}
}
