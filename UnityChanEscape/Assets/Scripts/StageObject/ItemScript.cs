using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

    private float frame = 0;
    private float defaultPositionY;

    // Use this for initialization
    void Start()
    {
        defaultPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        frame += 0.02f;
        float sin = Mathf.Sin(frame);
        if (sin == 0) frame = 0.0f;
        float y = defaultPositionY + sin * 0.1f;
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, y, p.z);
    }
}
