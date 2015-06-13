using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonOnceScript : MonoBehaviour {

    public Color onColor = new Color(0.5f, 1.0f, 0.5f);
    public Color offColor = new Color(1.0f, 0.5f, 0.5f);

    GameObject stageFlagManager;
    public List<GameObject> targetObject;

    bool flag;

    // Use this for initialization
    void Start()
    {
        renderer.material.color = offColor;
        stageFlagManager = GameObject.Find("StageFlagManager");
        flag = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!flag)
        {
            // ボタンの形状変化
            renderer.material.color = onColor;
            transform.localScale = new Vector3(1.0f, 0.3f, 1.0f);

            // ボタンのフラグが変わったことをStageFlagManagerに通知する
            stageFlagManager.SendMessage("FlagChanged", this.gameObject);
            flag = true;
        }
    }
}
