using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManage : MonoBehaviour
{
    SceneControl controler;
    private void Awake()
    {
        controler = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
    }
    public void play()
    {
        controler.SceneUpdate();
    }
    public void Save()
    {
        controler.DataSave();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
