using UnityEngine;
using System.Collections;

public class ElevatorButtonScript : MonoBehaviour {

	public Color trueColor;
	private Color falseColor;
	private Behaviour halo;
	public GameObject targetObj;
	public float moveDirection;

	private ElevatorWithButtonScript targetComponent;
	private int objectCount = 0;

	// Use this for initialization
	void Start () {
		falseColor = renderer.material.color;
		halo = (Behaviour)gameObject.GetComponent("Halo");

		targetComponent = targetObj.GetComponent<ElevatorWithButtonScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(objectCount > 0){
			renderer.material.color = trueColor;
			halo.renderer.material.color = trueColor;
			targetComponent.AddMoveDirection(moveDirection);
			//halo.enabled = false;
		}else{	
			renderer.material.color = falseColor;
		}
	}

	void OnCollisionEnter(Collision collision){
		string name = collision.gameObject.name;
		if(name.IndexOf("Plage") < 0){
			objectCount++;
		}
	}

	
	void OnCollisionExit(Collision collision){
		string name = collision.gameObject.name;
		if(name.IndexOf("Plage") < 0){
			objectCount--;
		}
	}
}
