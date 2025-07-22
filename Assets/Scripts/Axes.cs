using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axes : MonoBehaviour
{
    [SerializeField] protected bool collided;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.SendMessage("Damage", 9, SendMessageOptions.DontRequireReceiver);
        }
    }
}
