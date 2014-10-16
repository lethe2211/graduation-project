using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SeesawScript : MonoBehaviour {

	List<GameObject> onBoardChracters;
	public int maxAngle = 15;
	int nowAngle;

	// Use this for initialization
	void Start () {
		onBoardChracters = new List<GameObject> ();
		nowAngle = 15;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
		{
				int massSum = 0;
				for (int i = 0; i < onBoardChracters.Count; i++) {
						GameObject go = onBoardChracters [i];

						if (onBoardChracters [i].name.IndexOf ("BoxUnityChan") >= 0) {
								massSum -= (int)(Mathf.Sign (go.transform.position.z - transform.position.z) * go.rigidbody.mass);
						} else {
								massSum += (int)(Mathf.Sign (go.transform.position.z - transform.position.z) * go.rigidbody.mass);
						}
				}
				if (massSum == 0)
						return;
				if (nowAngle != (int)(Mathf.Sign (massSum) * maxAngle)) {
						SetEulerAngleX (transform, Mathf.Sign (massSum) * maxAngle);
						for (int i = 0; i < onBoardChracters.Count; i++) {
								float localZ = onBoardChracters [i].transform.position.z - transform.position.z;
								setY (onBoardChracters [i].transform, transform.position.y - localZ * Mathf.Tan (Mathf.PI / 180.0f * (Mathf.Sign (massSum) * maxAngle)));
						}
						nowAngle = (int)(Mathf.Sign(massSum) * maxAngle);
				}
	}

	void setY (Transform tf, float y)
	{
		Vector3 pos = tf.position;
		tf.position = new Vector3(pos.x, y, pos.z);
	}
	
	void SetEulerAngleX (Transform tf, float x)
	{
		Vector3 ea = tf.eulerAngles;
		tf.eulerAngles = new Vector3(x, ea.y, ea.z);
	}
	
	void OnCollisionEnter (Collision c)
	{
			if (onBoardChracters.IndexOf (c.gameObject) == -1) {
					onBoardChracters.Add (c.gameObject);
			}
	}

	void OnCollisionExit (Collision c)
	{
			float localZ = c.gameObject.transform.position.z - transform.position.z;
			if (Mathf.Abs (localZ) > c.gameObject.transform.localScale.z / 2) {
					onBoardChracters.Remove (c.gameObject);
			}
	}

}
