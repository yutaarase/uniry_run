using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMを再生させるオブジェクト
/// </summary>
public class BGMManage : MonoBehaviour
{
    //再生するBGM
    public AudioClip BGM;
    //AudioSource
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        //インスタンスを初期化
        //Loop再生設定にする
        //BGMを再生させる
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
