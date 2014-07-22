using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float timer;
	public GUIText TimerText;

	// Use this for initialization
	void Start () {
		reset();
	}

	private void reset()
	{
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		TimerText.text = ((int)timer).ToString() + "秒";
	}
}
