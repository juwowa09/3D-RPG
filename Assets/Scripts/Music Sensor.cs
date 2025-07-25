using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSensor : MonoBehaviour
{
    public static bool musicSensor;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !musicSensor)
        {
            // Debug.Log("enter");
            musicSensor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy") && musicSensor)
        {
            // Debug.Log("exit");
            musicSensor = false;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
