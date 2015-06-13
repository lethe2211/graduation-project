using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// セーブデータ中のステージ情報を扱うクラス
// StageInfoList[0]でステージ1の情報を得られる
public class StageInfoList {

    List<StageInfo> stageInfoList; // ステージ情報のリスト
    int count; // ステージ情報の数

    // コンストラクタ
    public StageInfoList()
    {
        this.stageInfoList = new List<StageInfo>();
    }

    public StageInfoList(List<StageInfo> stageInfoList)
    {
        this.stageInfoList = stageInfoList;
    }

    // 一回でセットする
    public void SetAll(List<StageInfo> stageInfoList)
    {
        this.stageInfoList = stageInfoList;
    }

    // ステージ情報のリストを受け取る
    public List<StageInfo> GetAll()
    {
        return stageInfoList;
    }

    // 末尾にステージ情報を受け取る
    public void Add(StageInfo stageInfo)
    {
        stageInfoList.Add(stageInfo);
    }

    // インデクサ
    public StageInfo this[int stageNo]
    {
        set
        {
            stageInfoList[stageNo] = value;
        }
        get
        {
            return stageInfoList[stageNo];
        }
    }

    // ステージ情報の数を返す
    public int Count()
    {
        return stageInfoList.Count;
    }

    // ワールドごとのステージ数　
    // CountStageNumPerWorld()[1]でワールド1のステージ数が得られる
    public List<int> CountStageNumPerWorld()
    {
        List<int> stageNumPerWorld = new List<int> { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < this.Count(); i++)
        {
            stageNumPerWorld[this[i].world] += 1;
        }
        // Debugger.List (stageNumPerWorld);
        return stageNumPerWorld;
    }
}
