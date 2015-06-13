using UnityEngine;
using System.Collections;

public class MoveDownOnceWallScript : MonoBehaviour {

    Vector3 initPosition;
    Vector3 pos;
    bool buttonFlag;
    AudioSource audioSource;
    float wallHeight;

    // Use this for initialization
    void Start()
    {
        initPosition = transform.position;
        buttonFlag = false;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        wallHeight = transform.localScale.y;
    }

    void FixedUpdate()
    {
        pos = transform.position;

        if (buttonFlag)
        {
            if (pos.y >= initPosition.y - wallHeight + 0.3)
            {
                transform.Translate(0, -0.1f, 0);
            }
        }
    }

    void TriggerOn()
    {
        audioSource.PlayOneShot(audioSource.clip);
        buttonFlag = true;
    }
}
