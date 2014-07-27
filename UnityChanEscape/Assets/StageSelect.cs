using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour {

	public int stage;
	LineRenderer selectRectangle;
	// Use this for initialization
	void Start () {
	
		stage = 1;

		Vector3 s = new Vector3 (-5f, 5f, 5);
		Vector3 g = new Vector3 (0f, 0f, 5);
		selectRectangle = gameObject.AddComponent<LineRenderer> ();
		selectRectangle.material.color = Color.yellow;
		selectRectangle.SetWidth (0.1f, 0.1f);
		selectRectangle.SetVertexCount (2);
		selectRectangle.SetPosition (0, s);
		selectRectangle.SetPosition (1, g);

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

			if (stage == 1) Application.LoadLevel("DemoScene");

		}


	}
}
