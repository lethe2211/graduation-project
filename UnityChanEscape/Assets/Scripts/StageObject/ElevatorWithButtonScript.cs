using UnityEngine;
using System.Collections;

public class ElevatorWithButtonScript : MonoBehaviour
{

    public float top;
    public float bottom;
    public float speed;

    // Use this for initialization
    void Start()
    {
    }

    void FixedUpdate()
    {
        // if(upFlag && downFlag) return;


        /*
		if(upFlag){
			p = new Vector3(p.x, p.y + speed, p.z);
			transform.position = p;
		}

		if(downFlag){
			p = new Vector3(p.x, p.y - speed, p.z);
			transform.position = p;
		}
		*/
    }

    // Update is called once per frame

    public void AddMoveDirection(float value)
    {
        Vector3 p = transform.position;
        float y = p.y + speed * value;
        if (bottom < y && y < top)
        {
            p = new Vector3(p.x, p.y + speed * value, p.z);
            transform.position = p;
        }
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision c)
    {
        c.gameObject.transform.parent = transform;
    }

    void OnCollisionExit(Collision c)
    {
        c.gameObject.transform.parent = null;
    }
}
