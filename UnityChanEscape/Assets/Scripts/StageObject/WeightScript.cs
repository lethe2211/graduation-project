using UnityEngine;
using System.Collections;

public class WeightScript : MonoBehaviour {

    public float g = 9.8f;
    private Vector3 fixedPosition;
    private GameObject triggerMessageObject;

    // Use this for initialization
    void Start()
    {
        fixedPosition = new Vector3(0.1f, 0.6f, 0.1f);
        triggerMessageObject = GameObject.Find("TriggerMessageObject");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rigidbody.mass > 0.1)
        {
            Vector3 v = new Vector3(0, -g, 0);
            rigidbody.AddForce(v * 50);

            /*
			if(transform.parent != null) return;
			if(transform.parent.name.Equals("Character1_Hips")){
				print("omori soubi sareteiru");
				// 以下、重りが装備されている場合の処理
				// transform.localPosition = fixedPosition;
			}
			*/
        }
    }
    
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "unitychan" || c.gameObject.name == "BoxUnityChan")
        {
            // FIXME: 現時点では，例えば，ユニティちゃんでおもりに近づいたままキャラチェンジしたり，ポーズしたりするとメッセージが出たままになる
            // 現在の実装では，操作キャラがユニティちゃんかボックスユニティちゃんかを取得することが難しいため放置している
            
            triggerMessageObject.SendMessage("DisplayMessage", "サブキーでおもりを拾う");
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "unitychan" || c.gameObject.name == "BoxUnityChan")
        {
            triggerMessageObject.SendMessage("HideMessage");
        }
    }
}
