using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

	private float frame = 0;
	private float stopFrame = 0;

	private float centerPositionY;
	private float moveRangeHalf; 

	// Use this for initialization
	void Start () {
		centerPositionY = transform.position.y;
		moveRangeHalf = 2.7f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		frame += 0.01f;
		float sin = Mathf.Sin(frame);

		if(sin == 0) frame = 0.0f;

		if (sin == 1 || sin == -1) {
			stopFrame++;
			if(stopFrame <= 50) return;
		}else{
			stopFrame = 0;
		}

		float y = centerPositionY + sin * moveRangeHalf;
		Vector3 p = transform.position;
		transform.position = new Vector3(p.x, y, p.z);
	}

	void OnCollisionEnter(Collision c){
		c.gameObject.transform.parent = transform;
	}

	void OnCollisionExit(Collision c){
		c.gameObject.transform.parent = null;
	}
}
