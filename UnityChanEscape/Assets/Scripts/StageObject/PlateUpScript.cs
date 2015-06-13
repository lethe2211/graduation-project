using UnityEngine;
using System.Collections;

// OnOffButtonに応じて，アタッチしたオブジェクトのy軸方向へ移動させるスクリプト
public class PlateUpScript : MonoBehaviour {

    private Vector3 initPosition;
    private Vector3 pos;

    private bool buttonFlag;
    public double upDistance;
    private bool direction;

    void Start()
    {
        initPosition = transform.localPosition;
        buttonFlag = false;
        if (upDistance >= 0)
        {
            direction = true;
        }
        else
        {
            direction = false;
        }
    }

    void FixedUpdate()
    {
        pos = transform.localPosition;
        if (buttonFlag)
        {
            if (direction == true && pos.y <= initPosition.y + upDistance)
            {
                transform.Translate(0, 0.1f, 0);
            }
            else if (direction == false && pos.y >= initPosition.y + upDistance)
            {
                transform.Translate(0, -0.1f, 0);
            }
        }
        else
        {
            if (direction == true && pos.y >= initPosition.y)
            {
                transform.Translate(0, -0.1f, 0);
            }
            else if (direction == false && pos.y <= initPosition.y)
            {
                transform.Translate(0, 0.1f, 0);
            }
        }
    }

    void TriggerOn()
    {
        buttonFlag = !buttonFlag;
    }
}