using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espadas : MonoBehaviour
{
    public bool hitEnemy;
    public bool hitEscudo;
    private void OnTriggerEnter(Collider other)
    {
        if(!Player._player.useEspada) return;
        
        if (other.gameObject.CompareTag("EscudoGB") && !hitEnemy && !hitEscudo)
        {
            hitEscudo = true;
            // Debug.Log(other.gameObject.name);
            Enemy script = other.GetComponentInParent<Enemy>();
            script.gameObject.SendMessage("Block", SendMessageOptions.DontRequireReceiver);
            CancelInvoke("RestartEscudo");
            Invoke("RestartEscudo", 0.7f);
        }
        else if (other.gameObject.CompareTag("Enemy") && !hitEscudo && !hitEnemy)
        {
            hitEnemy = true;
            // Debug.Log(other.gameObject.name);
            other.gameObject.SendMessage("Damage", 9, SendMessageOptions.DontRequireReceiver);
            CancelInvoke("RestartEnemy");
            Invoke("RestartEnemy", 0.7f);
        }
    }
    void RestartEscudo()
    {
        hitEscudo = false;
    }

    void RestartEnemy()
    {
        hitEnemy = false;
    }
}
