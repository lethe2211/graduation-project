using UnityEngine;
using System.Collections;

public class GPlateScript : MonoBehaviour {

	public float upsidePosition;
	public float downsidePosition;

	bool unitychanFlag = false;
	bool boxunitychanFlag = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 v = transform.localPosition;
		float y = v.y;

		if (unitychanFlag){
			y += 0.1f;
			//print("unitychan flag " + y);
		}
		if (boxunitychanFlag){
			//print("boxunitychan flag" + y);
			y -= 0.1f;
		}


		if(y >= upsidePosition) y = upsidePosition;
		if(y <= downsidePosition) y = downsidePosition;
		
		transform.localPosition = new Vector3(v.x, y, v.z);
	}

	void OnCollisionEnter(Collision collision){
		//print ("gplate Enter");
		// オブジェクトが接触した時
		if(collision.gameObject.name == "unitychan"){
			//print ("unity chan");
			unitychanFlag = true;
		}

		if(collision.gameObject.name == "BoxUnityChan"){
			//print ("box unity chan");
			boxunitychanFlag = true;
		}
	}

	void OnCollisionExit(Collision collision){
		// オブジェクトが離れた時
	}

}
