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
    public static SceneControl instance;
    public int score;
    public int highScore = 0;
    public bool timeUp;
    private int index;

    // 各SceneのID
    private const int TITLE_ID = 0;
    private const int STAGE_ID = 1;
    private const int Result_ID = 2;
    private bool getText;
    private bool update;
    public AudioClip selectClip;
    public AudioClip [] saveClip = new AudioClip[2];
    AudioSource audioSource;

    private string fileName = ".dat";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        index = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();

    }
    public void Update()
    {
        switch (index)
        {
            case TITLE_ID:
                if(update && !audioSource.isPlaying)
                {
                    NextScene(STAGE_ID);
                    update = false;
                }
                break;
            case STAGE_ID:
                if (timeUp)
                {
                    NextScene(Result_ID);
                    getText = false;
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }
                break;
            case Result_ID:
                if(score > highScore) highScore = score;
                if (!getText)
                {
                    var texts = FindObjectsOfType<Text>();
                    foreach (Text text in texts)
                    {
                        if (text.name == "HighScore") text.text = $"最高記録 : {highScore}回";
                        else if (text.name == "Score") text.text = $"記録 : {score}回";
                    }
                    getText = true;
                }
                if (update && !audioSource.isPlaying)
                {
                    NextScene(TITLE_ID);
                    update = false;
                }
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    audioSource.clip = selectClip;
                    audioSource.Play();
                    update = true;
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }

                break;
        }
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void SceneUpdate()
    {
        audioSource.clip = selectClip;
        audioSource.Play();
        update = true;
        timeUp = false;
        score = 0;
    }

    public void DataSave()
    {

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
    public void NextScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
