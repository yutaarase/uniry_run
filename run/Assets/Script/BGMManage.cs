using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM���Đ�������I�u�W�F�N�g
/// </summary>
public class BGMManage : MonoBehaviour
{
    //�Đ�����BGM
    public AudioClip BGM;
    //AudioSource
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        //�C���X�^���X��������
        //Loop�Đ��ݒ�ɂ���
        //BGM���Đ�������
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
