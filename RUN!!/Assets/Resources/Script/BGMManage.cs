using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManage : MonoBehaviour
{
    public AudioClip BGM;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
