using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour
{
    public GameObject area;
    public GameObject point;
    private GameObject observer;

    private void Start()
    {
        observer = GameObject.Find("Observer");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!area.active)
        {
            area.SetActive(true);
            point.SetActive(false);
            observer.GetComponent<Observation>().cnt++;
        }
    }
}
