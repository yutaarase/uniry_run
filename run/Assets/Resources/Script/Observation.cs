using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Observation : MonoBehaviour
{
    public GameObject [] area = new GameObject [2];
    public int cnt;
    Text text;
    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        cnt = 0;
        text = GameObject.FindObjectOfType<Text> ();
        manager = GameObject.Find("GameManager");
    }


    private void FixedUpdate()
    {
        text.text = manager.GetComponent<Manage>().cnt.ToString();
        if(!area[0].activeInHierarchy && !area[1].activeInHierarchy)
        {
            var Script = GameObject.FindGameObjectWithTag("Controller").GetComponent<SceneControl>();
            Script.score = cnt;
            Script.timeUp = true;
        }
    }

}
