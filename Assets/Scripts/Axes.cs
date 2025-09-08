using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axes : MonoBehaviour
{
    public static bool hitPlayer, hitEscudo;
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Escudo") && enemy.isAttack && ControllerEscudo.activeParry)
        {
            // ControllerEscudo.activeParry = false;
            // Debug.Log("parrying");
            StopCoroutine(enemy.IeAttack());
            StopCoroutine(enemy.IeAttackB());
            enemy._animator.SetTrigger("damage");
            enemy.isAttack = false;
            enemy.escudoGB.enabled = false;
            MatrixEffects._instance.ActiveSlowCamera();
            return;
        }
        
        if (ControllerEscudo.defend)
        {
            if (other.gameObject.CompareTag("Escudo") && enemy.isAttack && !hitPlayer && !hitEscudo)
            {
                if (ControllerEscudo.defend)
                {
                    hitEscudo = true;
                    // Debug.Log(other.gameObject.name);
                    Player script = other.GetComponentInParent<Player>();
                    script.gameObject.SendMessage("Block", 9, SendMessageOptions.DontRequireReceiver);
                    CancelInvoke("RestartEscudo");
                    Invoke("RestartEscudo", 0.7f);
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player") && enemy.isAttack && !hitEscudo && !hitPlayer)
            {
                hitPlayer = true;
                // Debug.Log(other.gameObject.name);
                other.gameObject.SendMessage("Damage", 9, SendMessageOptions.DontRequireReceiver);
                CancelInvoke("RestartPlayer");
                Invoke("RestartPlayer", 0.7f);
            }
        }
    }

    void RestartEscudo()
    {
        hitEscudo = false;
    }

    void RestartPlayer()
    {
        hitPlayer = false;
    }
}
