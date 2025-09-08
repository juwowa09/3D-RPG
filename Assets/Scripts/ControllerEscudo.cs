using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEscudo : MonoBehaviour
{
    private BoxCollider boxEscudo;
    public static bool defend;
    public static bool activeParry;
    void Start()
    {
        boxEscudo = this.GetComponent<BoxCollider>();
        boxEscudo.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 패링하는 코드
        if (Input.GetKeyDown(KeyCode.R) && ControllParrying.enableParry && !activeParry)
        {
            activeParry = true;
            Player._player._animator.SetTrigger("parry");
            // boxEscudo.enabled = true;
        }
        // 패링 타이밍이 아니면서 방어도(Q) 하지 않는다면
        if (!ControllParrying.enableParry)
        {
            // boxEscudo.enabled = false;
            activeParry = false;
        }
        
        //
        if (Input.GetKeyDown(KeyCode.Q) && !defend)
        {
            defend = true;
            // boxEscudo.enabled = true;
            Player._player._animator.SetBool("defense",true);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && defend)
        {
            defend = false;
            // boxEscudo.enabled = false;
            Player._player._animator.SetBool("defense",false);
        }
        else if (Player._player.useEspada || !defend && !activeParry)
        {
            // boxEscudo.enabled = false;
        }
    }
}
