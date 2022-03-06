using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// シャトルランの進行を行うクラス
/// </summary>
public class Manage : MonoBehaviour
{
    public int cnt; //現在のカウント数
    [SerializeField] private int m_Index; //音楽配列要素数用変数
    [SerializeField] private float Interval; //再生間隔
    [SerializeField] private AudioClip[] audioClips = new AudioClip[3]; //通常の音楽
    [SerializeField] private AudioClip[] reverseClips = new AudioClip[3]; //折り返しの音楽
    [SerializeField] private AudioClip startClip; //往復後スタート時の音楽
    [SerializeField] private AudioClip cntDownClip; //スタート時のカウントダウン
    [SerializeField] private AudioSource audioSource; //AudioSource
    [SerializeField] private GameObject[] area = new GameObject[2]; //判定エリア
    [SerializeField] private GameObject[] point = new GameObject[2]; //スポーンポイント
    [SerializeField] private int floorMax; //足場の最大数(レベルが上がる数)
    [SerializeField] int floorInterval; //足場の最大数を超えてからの処理用変数
    private bool start; //StartClipを再生出来るかどうか
    private bool gameStart; //ゲームが開始したかどうか
    [SerializeField]private bool isPlay; //再生できるかどうか
    private float currentTime; //現在の経過時間

    // Start is called before the first frame update
    
    private void Awake()
    {
        //各変数初期化
        cnt = 0;
        m_Index = 0;
        floorInterval = -1;
        gameStart = false;
        //既存の足場を削除して新しい足場のインスタンスを起こす
        Destroy(GameObject.FindGameObjectWithTag("Floor"));
        var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)(cnt / 2 + 1)}") as GameObject) as GameObject;
        floor.transform.position = new Vector3(10, 6, 32);
        
    }

    private void Start()
    {
        //音楽を再生する
        audioSource.clip = cntDownClip;
        audioSource.Play();
    }
    private void FixedUpdate()
    {
        //片道分再生し終わったら
        if (!audioSource.isPlaying && m_Index >= 3)
        {
            //再生できないようにする
            isPlay = false;
            
            if (cnt % 2 == 0)
            {
                //カウント数が偶数だったら
                //既存の足場を削除して新しい足場のインスタンスを起こす
                floorInterval = cnt / floorMax;
                Destroy(GameObject.FindGameObjectWithTag("Floor"));
                var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)((cnt - floorInterval * 30) / 2 + 1)}") as GameObject) as GameObject;
                floor.transform.position = new Vector3(10, 6, 32);
            }
            else
            {
                //奇数だったら足場を逆向きに回転させる
                GameObject.FindGameObjectWithTag("Floor").transform.rotation = Quaternion.Euler(0f,180f,0f);
            }
            
        }

        //ゲームがスタートしていなかったら
        if (!gameStart)
        {
            //再生していなっかったら
            //ゲームをスタートさせる
            if (!audioSource.isPlaying)
            {
                gameStart = true;
            }
        }

        if (isPlay)
        {

            //再生出来るなら
            //再生させる
            Play();
        }
        else
        {
            //再生出来なかったら
            //経過時間が再生間隔以上だったら
            if(currentTime >= Interval)
            {
                //各変数を初期化し
                //カウント数を加算
                //カウント数に対応したエリアを非表示に
                //カウント数に対応したポイントを表示させる
                isPlay = true;
                currentTime = 0;
                start = false;
                m_Index = 0;
                ++cnt;
                area[cnt % 2].SetActive(false);
                point[cnt % 2].SetActive(true);
            }

            //ゲームがスタートしていたら
            if (gameStart)
            {
                //再生を止める
                //経過時間を加算
                Stop();
                currentTime += Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// シャトルランの音楽を再生させるメソッド
    /// </summary>
    void Play()
    {
        //再生されてなかったら
        if (!audioSource.isPlaying)
        {
            
            if (cnt % 2 == 1)
            {
                //カウント数が奇数だったら
                //カウント数が足場の最大数を超えたら
                //何度も行わせないために n * floorMax + 1 をとるようにする
                if (cnt % floorMax == 1 && !start)
                {
                    //ピッチを上げる
                    audioSource.pitch = 1 + 0.1f * (floorInterval+1);
                    audioSource.clip = startClip;
                    audioSource.Play();
                    start = true;
                }

                //再生されてなかったら
                //上の処理が行われたら再生されているので
                //ここにも同じ条件を付ける
                if (!audioSource.isPlaying)
                {
                    //通常の音楽を再生する
                    //要素数を加算
                    audioSource.clip = audioClips[m_Index];
                    audioSource.Play();
                    ++m_Index;
                }
            }
            else
            {
                //偶数だったら
                //折り返しの音楽を再生する
                //要素数を加算
                audioSource.clip = reverseClips[m_Index];
                audioSource.Play();
                ++m_Index;
            }
            
        }
    }

    /// <summary>
    /// 再生を止めるメソッド
    /// </summary>
    void Stop()
    {
        audioSource.Stop();
    }
}
