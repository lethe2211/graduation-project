﻿using UnityEngine;
using System.Collections;

public class RedButtonScript : MonoBehaviour {

	private Color lightRedColor = new Color(1.0f, 0.5f, 0.5f);
	private Color lightGreenColor = new Color(0.5f, 1.0f, 0.5f);
	private Behaviour halo;
	private GameObject redWall;
	private bool buttonFlag = false;

	// Use this for initialization
	void Start () {
		renderer.material.color = lightRedColor;
		halo = transform.GetComponent("Halo")  as Behaviour;
		redWall = GameObject.Find ("RedWall");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p = redWall.transform.position;
		if(buttonFlag && p.y >= -1.1)
			redWall.transform.position = new Vector3(p.x, p.y - 0.1f, p.z);
		if(!buttonFlag && p.y <= 1.0)
			redWall.transform.position = new Vector3(p.x, p.y + 0.1f, p.z);
	}

	void OnCollisionEnter(Collision collision){
		// オブジェクトが接触した時
		 print("OnCollisionEnter");
		renderer.material.color = lightGreenColor;
		halo.renderer.material.color = lightGreenColor;
		buttonFlag = true;
	}
	
	void OnCollisionExit(Collision collision){
		// オブジェクトが離れた時
		print("OnCollisionExit");
		renderer.material.color = lightRedColor;
		buttonFlag = false;
	}

	void OnCollisionStay(Collision collision){
		// オブジェクトが接触し続けている場合
	}
}
