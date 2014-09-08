using UnityEngine;
using System.Collections;
using System.Threading;

public class SignboardScript : MonoBehaviour {

	public GameObject cone;
	private float flame;
	public GUIText text;
	public GUITexture textback;
	private bool isReadable;
	
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
	}
	
	// Update is called once per frame
	void Update ()
	{
		flame += 0.01f;
		float pos = Mathf.Sin (flame * 5);
		cone.transform.Rotate (0, 0, 3);
		cone.transform.position = new Vector3 (cone.transform.position.x, 1.5f + pos / 5, cone.transform.position.z);
		
		if (isReadable && Input.GetKeyDown ("z")) {
			text.enabled = !text.enabled;
			textback.enabled = !textback.enabled;
		}
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
