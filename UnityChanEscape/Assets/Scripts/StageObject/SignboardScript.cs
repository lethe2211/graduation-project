using UnityEngine;
using System.Collections;
using System.Threading;

public class SignboardScript : MonoBehaviour {

	public GameObject cone;
	private float flame;
	public GUIText text;
	public GUITexture textback;
	private bool isReadable;
	bool subKeyPressed = false;
	
	// Use this for initialization
	void Start () {
		cone = transform.FindChild("Cone").gameObject;
		text = transform.FindChild("Text").guiText;
		textback = transform.FindChild("TextBack").guiTexture;
		cone.renderer.enabled = false;
		text.enabled = false;
		textback.enabled = false;
		flame = 0;
		isReadable = false;
		text.transform.position = new Vector3(0.5f, 0.1f, 2f);
		textback.transform.position = new Vector3(0.5f, 0.1f, 0f);
	}
	
	void Update ()
	{
			if (Input.GetKeyDown (KeyInputManager.subKeyCode) || Input.GetButtonDown ("subButton")) {
					subKeyPressed = true;
			}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		flame += 0.01f;
		float pos = Mathf.Sin (flame * 5);
		cone.transform.Rotate (0, 0, 3);
		cone.transform.position = new Vector3 (cone.transform.position.x, 1.5f + pos / 5, cone.transform.position.z);
		
		if (isReadable && subKeyPressed) {
			text.enabled = !text.enabled;
			textback.enabled = !textback.enabled;
		}
		subKeyPressed = false;
	}
		
	void OnTriggerEnter (Collider c)
	{
		if (c.gameObject.name == "unitychan") {
			cone.renderer.enabled = true;
			isReadable = true;
		}
	}
		
	void OnTriggerExit (Collider c)
	{
		if (c.gameObject.name == "unitychan") {
			cone.renderer.enabled = false;
			isReadable = false;
			text.enabled = false;
			textback.enabled = false;
		}
	}
}
