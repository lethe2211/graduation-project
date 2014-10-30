﻿using UnityEngine;
using System.Collections;

public class PlateMaterialScript : MonoBehaviour {

	public Material mat;
	
	// Use this for initialization
	void Start ()
	{
			GameObject[] plates = GameObject.FindGameObjectsWithTag ("Plate");
			foreach (GameObject go in plates) {
						Debug.Log(go.name + ":localScale:" + go.transform.localScale.ToString());
						Debug.Log(go.name + "lossyScale:" + go.transform.lossyScale.ToString());
						float x = go.transform.lossyScale.x;
						float z = go.transform.lossyScale.z;
						mat.SetTextureScale("_MainTex", new Vector2(x/10,z/10));
						go.renderer.material = new Material(mat);
			}	
	}
}
