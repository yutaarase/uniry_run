using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンをクリックしたときの処理をまとめたクラス
/// </summary>
public class ClickManage : MonoBehaviour
{
    //シーン遷移用コントローラー
    SceneControl controler;
    private void Awake()
    {
        //インスタンスを初期化
        controler = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void play()
    {
        controler.SceneUpdate();
    }

    /// <summary>
    /// セーブ処理
    /// </summary>
    public void Save()
    {
        controler.DataSave();
    }

    /// <summary>
    /// アプリケーション終了
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
