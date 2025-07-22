using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espadas : MonoBehaviour
{
    [SerializeField] protected bool collided;
    private void OnCollisionEnter(Collision other)
    {
        if (collided) return;
        if(other.gameObject.CompareTag("Enemy"))
            other.gameObject.SendMessage("Damage", 5, SendMessageOptions.DontRequireReceiver);
    }
}
