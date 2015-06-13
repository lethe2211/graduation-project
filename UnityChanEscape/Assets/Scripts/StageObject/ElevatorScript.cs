using UnityEngine;
using System.Collections;

/**
 * 上下移動する床のスクリプト
 */
public class ElevatorScript : MonoBehaviour {

    // 床の移動に使う情報
    private float frame = 0;
    private float stopFrame = 0;

    // 床の最も高い位置のY座標
    public float upperY;
    // 床の最も低い位置のY座標
    public float downerY;
    // 相対的
    public float relativelySpeed; 

    // upperとdownerのちょうど真ん中のY座標
    private float centerPositionY;
    // 床の中心からupper,downerの距離
    private float moveRangeHalf; 

    /**
     * 値の初期化
     */
    void Start () {
        centerPositionY = (upperY + downerY) / 2;
        moveRangeHalf = upperY - centerPositionY;
    }
    

    /**
     * 1フレームごとに床の位置を再計算させて移動する
     *
     * 基本的にはsin関数にしたがって、upperとdownerの間を床が移動するが、
     * 一番上の位置と一番下の位置では 50 フレーム停止する
     */
    void FixedUpdate () {
        frame += relativelySpeed + 0.01f;
        float sin = Mathf.Sin(frame);

        if(sin == 0) frame = 0.0f;

        if (sin == 1 || sin == -1) {
            stopFrame++;
            if(stopFrame <= 50) return;
        }else{
            stopFrame = 0;
        }

        float y = centerPositionY + sin * moveRangeHalf;
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, y, p.z);
    }

    /**
     * この床に誰かが乗った場合は、そのものも床と一緒に動かす
     */
    void OnCollisionEnter(Collision c){
        c.gameObject.transform.parent = transform;
    }

    /**
     * 床から離れたら一緒には動かない
     */
    void OnCollisionExit(Collision c){
        c.gameObject.transform.parent = null;
    }
}
