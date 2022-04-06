using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �V���g�������̊Ď����s���N���X
/// </summary>
public class Observation : MonoBehaviour
{
    //����G���A
    public GameObject [] area = new GameObject [2];
    //�v����
    public int cnt;
    //�v������\������e�L�X�g
    Text text;
    //�V���g�������̐i�s���s���I�u�W�F�N�g
    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        //�e�ϐ��C���X�^���X������
        //�I�u�W�F�N�g�擾
        cnt = 0;
        text = GameObject.FindObjectOfType<Text> ();
        manager = GameObject.Find("GameManager");
    }


    private void FixedUpdate()
    {
        //�e�L�X�g���X�V
        text.text = manager.GetComponent<Manage>().cnt.ToString();

        //�G���A����\����������
        //�R���g���[���[�̃X�R�A���X�V
        //�^�C���A�b�v��true�ɂ���
        if(!area[0].activeInHierarchy && !area[1].activeInHierarchy)
        {
            var Script = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
            Script.score = cnt;
            Script.timeUp = true;
        }
    }

}
