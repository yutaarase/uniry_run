using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定エリアに到着したら計測数を更新するクラス
/// </summary>
public class Arrive : MonoBehaviour
{
    //判定エリア
    public GameObject area;
    //スポーンポイント
    public GameObject point;
    //監視オブジェクト
    private GameObject observer;

    private void Start()
    {
        //オブジェクトを取得
        observer = GameObject.Find("Observer");
    }

    private void OnCollisionExit(Collision collision)
    {
        //判定エリアが非表示だったら
        if (!area.activeInHierarchy)
        {
            //エリアを表示
            //スポーンポイントを非表示にする
            //監視オブジェクトのカウント数を加算
            area.SetActive(true);
            point.SetActive(false);
            observer.GetComponent<Observation>().cnt++;
        }
    }
}
