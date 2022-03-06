using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シャトルランの監視を行うクラス
/// </summary>
public class Observation : MonoBehaviour
{
    //判定エリア
    public GameObject [] area = new GameObject [2];
    //計測数
    public int cnt;
    //計測数を表示するテキスト
    Text text;
    //シャトルランの進行を行うオブジェクト
    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        //各変数インスタンス初期化
        //オブジェクト取得
        cnt = 0;
        text = GameObject.FindObjectOfType<Text> ();
        manager = GameObject.Find("GameManager");
    }


    private void FixedUpdate()
    {
        //テキストを更新
        text.text = manager.GetComponent<Manage>().cnt.ToString();

        //エリアが非表示だったら
        //コントローラーのスコアを更新
        //タイムアップをtrueにする
        if(!area[0].activeInHierarchy && !area[1].activeInHierarchy)
        {
            var Script = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
            Script.score = cnt;
            Script.timeUp = true;
        }
    }

}
