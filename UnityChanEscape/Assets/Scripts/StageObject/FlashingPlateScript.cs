using UnityEngine;
using System.Collections;

public class FlashingPlateScript : MonoBehaviour {

    public int AppearTime, IntervalTime, DyingTime;
    int time = 0;
    Color FixedColor;

    // Use this for initialization
    void Start()
    {
        FixedColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        time++;
        if (time <= AppearTime)
        {
            renderer.material.color = FixedColor;
            // rigidbody.collider.enabled = true;
            collider.enabled = true;
        }
        else if (time <= AppearTime + IntervalTime)
        {
            int dt = time - AppearTime;
            float a = (float)(IntervalTime - dt) / (float)IntervalTime;
            //print(a);
            renderer.material.color = new Color(FixedColor.r, FixedColor.g, FixedColor.b, a);
        }
        else if (time <= AppearTime + IntervalTime + DyingTime)
        {
            // rigidbody.collider.enabled = false;
            collider.enabled = false;
        }
        else
        {
            time = 0;
        }
    }
}
