using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public Color OnColor = new Color(0.5f, 1.0f, 0.5f);
    public Color OffColor = new Color(1.0f, 0.5f, 0.5f);
    public GameObject[] targetObj = new GameObject[10];
    private int onObjectNum = 0; // 上に乗っているオブジェクトの数

    // Use this for initialization
    void Start()
    {
        renderer.material.color = OffColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // オブジェクトが接触した時
        print("OnTriggerEnter");
        onObjectNum++;
        print(onObjectNum);
        renderer.material.color = OnColor;
        foreach (GameObject tObj in targetObj)
        {
            tObj.SendMessage("ButtonOn");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // オブジェクトが離れた時
        print("OnTriggerExit");
        onObjectNum--;
        print(onObjectNum);

        if (onObjectNum <= 0)
        {
            renderer.material.color = OffColor;
            foreach (GameObject tObj in targetObj)
            {
                tObj.SendMessage("ButtonOff");
            }
        }
    }
}
