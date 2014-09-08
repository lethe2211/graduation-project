using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

	public GUIText clearText;
	public AudioSource clearVoice;
	private bool is_cleared = false;

	GameObject TimerObject;

	// Use this for initialization
	void Start () {
		TimerObject = GameObject.Find ("TimeManager");
		clearText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (is_cleared && Input.GetKeyDown ("z")) {
			Application.LoadLevel("StageSelect");
		}
	}

	void Cleared () {
		TimerObject.SendMessage ("Stop");
		clearText.enabled = true;
		clearVoice.Play();
		is_cleared = true;
	}
}
