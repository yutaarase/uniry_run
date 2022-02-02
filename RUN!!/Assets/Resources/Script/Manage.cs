using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Manage : MonoBehaviour
{
    [SerializeField] public int cnt;
    [SerializeField] private int m_Index;
    [SerializeField] private float Interval;
    [SerializeField] private AudioClip[] audioClips = new AudioClip[3];
    [SerializeField] private AudioClip[] reverseClips = new AudioClip[3];
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip cntDownClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject[] area = new GameObject[2];
    [SerializeField] private GameObject[] point = new GameObject[2];
    [SerializeField] private int floorMax;
    [SerializeField] int floorInterval;
    private bool start;
    private bool gameStart;
    [SerializeField]private bool isPlay;
    private float currentTime;

    // Start is called before the first frame update
    
    private void Awake()
    {
        cnt = 0;
        m_Index = 0;
        floorInterval = -1;
        gameStart = false;
        Destroy(GameObject.FindGameObjectWithTag("Floor"));
        var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)(cnt / 2 + 1)}") as GameObject) as GameObject;
        floor.transform.position = new Vector3(10, 6, 32);
        
    }

    private void Start()
    {
        audioSource.clip = cntDownClip;
        audioSource.Play();
    }
    private void FixedUpdate()
    {
        if (!audioSource.isPlaying && m_Index >= 3)
        {
            isPlay = false;
            if (cnt % 2 == 0)
            {
                floorInterval = cnt / floorMax;
                Destroy(GameObject.FindGameObjectWithTag("Floor"));
                var floor = (GameObject)Instantiate(Resources.Load($"Floors/Floor{(int)((cnt - floorInterval * 30) / 2 + 1)}") as GameObject) as GameObject;
                floor.transform.position = new Vector3(10, 6, 32);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Floor").transform.rotation = Quaternion.Euler(0f,180f,0f);
            }
            
        }

        if (!gameStart)
        {
            if (!audioSource.isPlaying)
            {
                gameStart = true;
            }
            
        }

        if (isPlay)
        {
            Play();
            
        }
        else
        {
            if(currentTime >= Interval)
            {
                isPlay = true;
                currentTime = 0;
                start = false;
                m_Index = 0;
                ++cnt;
                area[cnt % 2].SetActive(false);
                point[cnt % 2].SetActive(true);
            }
            if (gameStart)
            {
                Stop();
                currentTime += Time.fixedDeltaTime;
            }
        }
    }

    void Play()
    {
        if (!audioSource.isPlaying)
        {
            
            if (cnt % 2 == 1)
            {
                if (cnt % floorMax == 1 && !start)
                {
                    audioSource.pitch = 1 + 0.1f * (floorInterval+1);
                    audioSource.clip = startClip;
                    audioSource.Play();
                    start = true;
                    
                }
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioClips[m_Index];
                    audioSource.Play();
                    ++m_Index;
                }
            }
            else
            {
                audioSource.clip = reverseClips[m_Index];
                audioSource.Play();
                ++m_Index;
            }
            
        }
    }

    void Stop()
    {
        audioSource.Stop();
    }
}
