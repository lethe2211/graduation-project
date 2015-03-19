using UnityEngine;
using System.Collections;

public class FallingPlateScript : MonoBehaviour {

	bool CollisionFlag = false;
	int time = 0;
	Vector3 Default;

	// Use this for initialization
	void Start () {
		Default = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		if (CollisionFlag) {
			time++;
			if(time <= 40 && time % 3 == 0){
				float rnd = Random.value;
				print (time + " rnd:" + rnd);
				Vector3 v = new Vector3(Default.x, Default.y - rnd / 5.0f, Default.z);
				transform.position = v;
			} else if (time <= 400){
				Vector3 v = new Vector3(Default.x, Default.y - (time/100.0f)*(time/100.0f), Default.z);
				transform.position = v;
			} else {
				transform.position = Default;
			}

		}
	}

	void OnCollisionEnter(Collision c){
		// c.gameObject.transform.parent = transform;
		CollisionFlag = true;
	}
	
	void OnCollisionExit(Collision c){
		// c.gameObject.transform.parent = null;
		// CollisionFlag = false;
		//if(time > 400) time = 0;
	}
}
