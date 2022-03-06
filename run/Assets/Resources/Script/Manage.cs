using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �V���g�������̐i�s���s���N���X
/// </summary>
public class Manage : MonoBehaviour
{
    public int cnt; //���݂̃J�E���g��
    [SerializeField] private int m_Index; //���y�z��v�f���p�ϐ�
    [SerializeField] private float Interval; //�Đ��Ԋu
    [SerializeField] private AudioClip[] audioClips = new AudioClip[3]; //�ʏ�̉��y
    [SerializeField] private AudioClip[] reverseClips = new AudioClip[3]; //�܂�Ԃ��̉��y
    [SerializeField] private AudioClip startClip; //������X�^�[�g���̉��y
    [SerializeField] private AudioClip cntDownClip; //�X�^�[�g���̃J�E���g�_�E��
    [SerializeField] private AudioSource audioSource; //AudioSource
    [SerializeField] private GameObject[] area = new GameObject[2]; //����G���A
    [SerializeField] private GameObject[] point = new GameObject[2]; //�X�|�[���|�C���g
    [SerializeField] private int floorMax; //����̍ő吔(���x�����オ�鐔)
    [SerializeField] int floorInterval; //����̍ő吔�𒴂��Ă���̏����p�ϐ�
    private bool start; //StartClip���Đ��o���邩�ǂ���
    private bool gameStart; //�Q�[�����J�n�������ǂ���
    [SerializeField]private bool isPlay; //�Đ��ł��邩�ǂ���
    private float currentTime; //���݂̌o�ߎ���

    // Start is called before the first frame update
    
    private void Awake()
    {
        //�e�ϐ�������
        cnt = 0;
        m_Index = 0;
        floorInterval = -1;
        gameStart = false;
        //�����̑�����폜���ĐV��������̃C���X�^���X���N����
        Destroy(GameObject.FindGameObjectWithTag("Floor"));
        var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)(cnt / 2 + 1)}") as GameObject) as GameObject;
        floor.transform.position = new Vector3(10, 6, 32);
        
    }

    private void Start()
    {
        //���y���Đ�����
        audioSource.clip = cntDownClip;
        audioSource.Play();
    }
    private void FixedUpdate()
    {
        //�Г����Đ����I�������
        if (!audioSource.isPlaying && m_Index >= 3)
        {
            //�Đ��ł��Ȃ��悤�ɂ���
            isPlay = false;
            
            if (cnt % 2 == 0)
            {
                //�J�E���g����������������
                //�����̑�����폜���ĐV��������̃C���X�^���X���N����
                floorInterval = cnt / floorMax;
                Destroy(GameObject.FindGameObjectWithTag("Floor"));
                var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)((cnt - floorInterval * 30) / 2 + 1)}") as GameObject) as GameObject;
                floor.transform.position = new Vector3(10, 6, 32);
            }
            else
            {
                //��������瑫����t�����ɉ�]������
                GameObject.FindGameObjectWithTag("Floor").transform.rotation = Quaternion.Euler(0f,180f,0f);
            }
            
        }

        //�Q�[�����X�^�[�g���Ă��Ȃ�������
        if (!gameStart)
        {
            //�Đ����Ă��Ȃ���������
            //�Q�[�����X�^�[�g������
            if (!audioSource.isPlaying)
            {
                gameStart = true;
            }
        }

        if (isPlay)
        {

            //�Đ��o����Ȃ�
            //�Đ�������
            Play();
        }
        else
        {
            //�Đ��o���Ȃ�������
            //�o�ߎ��Ԃ��Đ��Ԋu�ȏゾ������
            if(currentTime >= Interval)
            {
                //�e�ϐ�����������
                //�J�E���g�������Z
                //�J�E���g���ɑΉ������G���A���\����
                //�J�E���g���ɑΉ������|�C���g��\��������
                isPlay = true;
                currentTime = 0;
                start = false;
                m_Index = 0;
                ++cnt;
                area[cnt % 2].SetActive(false);
                point[cnt % 2].SetActive(true);
            }

            //�Q�[�����X�^�[�g���Ă�����
            if (gameStart)
            {
                //�Đ����~�߂�
                //�o�ߎ��Ԃ����Z
                Stop();
                currentTime += Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// �V���g�������̉��y���Đ������郁�\�b�h
    /// </summary>
    void Play()
    {
        //�Đ�����ĂȂ�������
        if (!audioSource.isPlaying)
        {
            
            if (cnt % 2 == 1)
            {
                //�J�E���g�������������
                //�J�E���g��������̍ő吔�𒴂�����
                //���x���s�킹�Ȃ����߂� n * floorMax + 1 ���Ƃ�悤�ɂ���
                if (cnt % floorMax == 1 && !start)
                {
                    //�s�b�`���グ��
                    audioSource.pitch = 1 + 0.1f * (floorInterval+1);
                    audioSource.clip = startClip;
                    audioSource.Play();
                    start = true;
                }

                //�Đ�����ĂȂ�������
                //��̏������s��ꂽ��Đ�����Ă���̂�
                //�����ɂ�����������t����
                if (!audioSource.isPlaying)
                {
                    //�ʏ�̉��y���Đ�����
                    //�v�f�������Z
                    audioSource.clip = audioClips[m_Index];
                    audioSource.Play();
                    ++m_Index;
                }
            }
            else
            {
                //������������
                //�܂�Ԃ��̉��y���Đ�����
                //�v�f�������Z
                audioSource.clip = reverseClips[m_Index];
                audioSource.Play();
                ++m_Index;
            }
            
        }
    }

    /// <summary>
    /// �Đ����~�߂郁�\�b�h
    /// </summary>
    void Stop()
    {
        audioSource.Stop();
    }
}
