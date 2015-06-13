using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    public float timer;
    public GUIText TimerText;
    private bool is_available;

    // Use this for initialization
    void Start()
    {
        reset();
    }

    private void reset()
    {
        is_available = true;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_available)
        {
            timer += Time.deltaTime;
        }
        TimerText.text = (timer).ToString("f2") + "秒";
    }

    void Stop()
    {
        is_available = false;
    }
}
