using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// 全てのScene遷移を統括するクラス
/// </summary>
public class SceneControl : MonoBehaviour 
{
    public static SceneControl instance; //自身のインスタンス
    public int score; //スコア
    public int highScore = 0;　//最大スコア
    public bool timeUp; //タイムアップ
    private int index; //現在のSceneのID

    // 各SceneのID
    private const int TITLE_ID = 0; //タイトルID
    private const int STAGE_ID = 1; //ステージ1ID
    private const int Result_ID = 2; //ステージ２ID
    private bool getText; //テキストを取得したかどうか
    private bool update; //更新したかどうか
    public AudioClip selectClip; //選択音
    public AudioClip[] saveClip = new AudioClip[2]; //セーブ音
    AudioSource audioSource; //AudioSource

    private string fileName = ".dat"; //セーブファイル名

    private void Awake()
    {
        //インスタンスが起こされてなかったら
        //インスタンスの重複を避けるため
        if (instance == null)
        {
            //インスタンス初期化
            instance = this;

            //セーブファイルのロード処理
            if (File.Exists(Application.dataPath + fileName))
            {
                using (StreamReader sr = File.OpenText(Application.dataPath + fileName))
                {
                    // ロード
                    var data = sr.ReadToEnd();
                    highScore = int.Parse(data);
                    Debug.Log(data);
                }
            }
        }
        else
        {
            //インスタンス起こしていたら
            //自身を削除する
            Destroy(this.gameObject);
            return;
        }

        //破壊不可能オブジェクトにする
        //現在のSceneのＩＤ取得
        //インスタンス初期化
        DontDestroyOnLoad(this.gameObject);
        index = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();

    }
    public void Update()
    {
        //現在のSceneのIDによって処理を変える
        switch (index)
        {
            case TITLE_ID:
                //更新されていて音楽が再生されてなかったら
                if(update && !audioSource.isPlaying)
                {
                    //ステージシーンに遷移する
                    NextScene(STAGE_ID);
                    update = false;
                }
                break;
            case STAGE_ID:
                //タイムアップだったら
                if (timeUp)
                {
                    //リザルトシーンに遷移する
                    NextScene(Result_ID);
                    getText = false;
                }

                //Esapeを押されたときの処理
                if (Input.GetKey(KeyCode.Escape))
                {
                    //アプリケーションを終了させる
                    Application.Quit();
                }
                break;
            case Result_ID:
                //スコアが最大スコアを超えたら最大スコアをスコアにする
                if(score > highScore) highScore = score;

                //テキストを取得していなかったら
                if (!getText)
                {
                    //テキスト型のオブジェクトの配列を取得
                    var texts = FindObjectsOfType<Text>();

                    //各オブジェクトのテキストに入力
                    foreach (Text text in texts)
                    {
                        if (text.name == "HighScore") text.text = $"最高記録 : {highScore}回";
                        else if (text.name == "Score") text.text = $"記録 : {score}回";
                    }
                    //テキストを取得したので
                    getText = true;
                }

                //更新されていて再生されてなかったら
                if (update && !audioSource.isPlaying)
                {
                    //タイトルシーンに遷移する
                    NextScene(TITLE_ID);
                    update = false;
                }

                //マウスがクリックされたら
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    //選択音を再生する
                    audioSource.clip = selectClip;
                    audioSource.Play();
                    update = true;
                }

                //Escapeが押されたら
                if (Input.GetKey(KeyCode.Escape))
                {
                    //アプリケーションを終了する
                    Application.Quit();
                }

                break;
        }

        //現在のSceneIDを取得する
        index = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// シーンを更新するときのメソッド
    /// </summary>
    public void SceneUpdate()
    {
        //選択音を再生する
        //変数を初期化する
        audioSource.clip = selectClip;
        audioSource.Play();
        update = true;
        timeUp = false;
        score = 0;
    }

    /// <summary>
    /// セーブ処理メソッド
    /// </summary>
    public void DataSave()
    {
        //Datファイルを取得してファイルに値を書き出す
        //セーブ音を再生する
        using (StreamWriter sw = new StreamWriter(Application.dataPath + fileName, false))
        {
            sw.WriteLine(highScore);
            Debug.Log(highScore);
            sw.Flush();
            sw.Close();
            Debug.Log("書き込み完了");
            audioSource.clip = saveClip[1];
            audioSource.loop = false;
            audioSource.Play();
        }
        Debug.Log("セーブしました");
    }

    /// <summary>
    /// 入力されたindexの値に対応したSceneに遷移するメソッド
    /// </summary>
    /// <param name="index">遷移先SceneID</param>
    public void NextScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// 現在のSceneを再読み込みするメソッド
    /// </summary>
    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
