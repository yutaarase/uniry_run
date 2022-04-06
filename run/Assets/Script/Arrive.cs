using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����G���A�ɓ���������v�������X�V����N���X
/// </summary>
public class Arrive : MonoBehaviour
{
    //����G���A
    public GameObject area;
    //�X�|�[���|�C���g
    public GameObject point;
    //�Ď��I�u�W�F�N�g
    private GameObject observer;

    private void Start()
    {
        //�I�u�W�F�N�g���擾
        observer = GameObject.Find("Observer");
    }

    private void OnCollisionExit(Collision collision)
    {
        //����G���A����\����������
        if (!area.activeInHierarchy)
        {
            //�G���A��\��
            //�X�|�[���|�C���g���\���ɂ���
            //�Ď��I�u�W�F�N�g�̃J�E���g�������Z
            area.SetActive(true);
            point.SetActive(false);
            observer.GetComponent<Observation>().cnt++;
        }
    }
}
