using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour {

	public int stage;
	LineRenderer selectRectangle;
	private string[] stageList = { "DemoScene", "Stage2", "stage3"};
	// Use this for initialization
	void Start () {
	
		stage = 1;

	}
	
	// Update is called once per frame
	void Update () {
	
		
		if (Input.GetKeyDown ("left")) {
			
			if (stage > 1) stage -= 1;
			
		}
		
		if (Input.GetKeyDown ("right")) {
			
			if (stage < 5) stage += 1;
			
			
		}

		if (Input.GetKeyDown ("z")) {

			Application.LoadLevel(stageList[stage-1]);

		}


	}
}
