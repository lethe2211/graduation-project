using UnityEngine;
using System.Collections;

public class OnOffButtonScript : MonoBehaviour {

    private Color lightRedColor = new Color(1.0f, 0.5f, 0.5f);
    private Color lightGreenColor = new Color(0.5f, 1.0f, 0.5f);
    private Behaviour halo;
    private GameObject redWall;
    private bool buttonFlag = false;
    GameObject stageFlagManager;

    // Use this for initialization
    void Start()
    {
        renderer.material.color = lightRedColor;
        halo = transform.GetComponent("Halo") as Behaviour;
        redWall = GameObject.Find("RedWall");
        stageFlagManager = GameObject.Find("StageFlagManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // オブジェクトが接触した時
        // print("OnCollisionEnter");
        renderer.material.color = lightGreenColor;
        halo.renderer.material.color = lightGreenColor;
        stageFlagManager.SendMessage("FlagChanged", this.gameObject);
        buttonFlag = true;
    }

    void OnTriggerEnter(Collider col)
    {
        // オブジェクトが接触した時
        // print("OnCollisionEnter");
        renderer.material.color = lightGreenColor;
        halo.renderer.material.color = lightGreenColor;
        stageFlagManager.SendMessage("FlagChanged", this.gameObject);
        buttonFlag = true;
    }

    void OnCollisionExit(Collision collision)
    {
        // オブジェクトが離れた時
        // print("OnCollisionExit");
        renderer.material.color = lightRedColor;
        stageFlagManager.SendMessage("FlagChanged", this.gameObject);
        buttonFlag = false;
    }

    void OnTriggerExit(Collider col)
    {
        // オブジェクトが離れた時
        // print("OnCollisionExit");
        renderer.material.color = lightRedColor;
        stageFlagManager.SendMessage("FlagChanged", this.gameObject);
        buttonFlag = false;
    }

    void OnCollisionStay(Collision collision)
    {
        // オブジェクトが接触し続けている場合
    }
}
