using UnityEngine;
using System.Collections;

public class CameraObjectScript : MonoBehaviour {

	GameObject horizontalObject;
	GameObject verticalObject;
	Camera mainCamera;//メインカメラ
	Camera subCamera;//サブのカメラです

	public string horizontalObjectName;
	public string verticalObjectName;
	public bool isMainCamera;


	// Use this for initialization
	void Start () {
		horizontalObject = GameObject.Find (horizontalObjectName);
		verticalObject = GameObject.Find (verticalObjectName);
		
		mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		subCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isMainCamera && !mainCamera.enabled) return;
		if(!isMainCamera && !subCamera.enabled) return;

		// up-down
		if(Input.GetKey ("q")) verticalObject.transform.Rotate(3, 0, 0);
		if(Input.GetKey ("a")) verticalObject.transform.Rotate (-3, 0, 0);

		// right-left
		if(Input.GetKey ("1")) horizontalObject.transform.Rotate (0, -3, 0);
		if(Input.GetKey ("2")) horizontalObject.transform.Rotate (0, 3, 0);

		if(Input.GetKey ("g")) {
			//Physics.gravity  *= -1;
		}
	}
}
