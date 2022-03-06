using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�^�����N���b�N�����Ƃ��̏������܂Ƃ߂��N���X
/// </summary>
public class ClickManage : MonoBehaviour
{
    //�V�[���J�ڗp�R���g���[���[
    SceneControl controler;
    private void Awake()
    {
        //�C���X�^���X��������
        controler = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void play()
    {
        controler.SceneUpdate();
    }

    /// <summary>
    /// �Z�[�u����
    /// </summary>
    public void Save()
    {
        controler.DataSave();
    }

    /// <summary>
    /// �A�v���P�[�V�����I��
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
