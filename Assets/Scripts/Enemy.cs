using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Space]
    [Header("MODE")]
    public bool isHit, isVision, isPatrol, isdeath;

    public float moveFront;
    public bool isAttack;
    public BoxCollider escudoGB;

    [Header("I.A")]
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp = 100;
    [SerializeField] public Animator _animator;
    public Slider sliderHp;

    public NavMeshAgent navi;
    public Transform player;
    public bool updatelock;

    [Header("Patrol")] 
    private float tempDistance;
    public Vector3 rangePatrol;
    public float rangeAttack;
    public float rangeAlert;
    public bool isAlert;

    [Header("Coroutine")]
    protected Coroutine alertHandler;
    
    [Header("Audio")] 
    public AudioSource voiceFx;
    public AudioSource SoundFx;

    [Space] public int randomInt;
    

    void Start()
    {
        hp = maxHp;
        isPatrol = true;
    }
    void Update()
    {
        UpdateHP();
        if (isdeath)
        {
            return;
        }

        if (isVision || isHit)
        {
            Rotation();
        }
        if((isVision || isHit) && !updatelock)
        {
            isPatrol = false;
            isAlert = false;
            StopCoroutine(IePatrol());
            StopCoroutine(IeAlert());
            
            _animator.SetBool("alert", false);       
            _animator.SetBool("walk",false);
            _animator.SetBool("walkBattle",true);
            
            //갱신하면 is stop 무시됨(갱신)
            if (isAttack)
            {
                 navi.isStopped = true;
            }
            else
            {
                navi.destination = player.position;   
            }
            
            if (navi.remainingDistance <= 3.0f)
            {
                _animator.SetBool("walkBattle", false);
                _animator.SetBool("idleBattle", true);
                tempDistance = Vector3.Distance(transform.position, player.transform.position);
        
                if (tempDistance <= rangeAttack)
                {
                    Choice();
                }
            }
            else
            {
                _animator.SetBool("walkBattle", true);
                _animator.SetBool("idleBattle", false);  
                
                StopCoroutine(IeAttack());
                StopCoroutine(IeDefense());
            }
        }

        if (!isVision)
        {
            _animator.SetBool("idleBattle", false);
        }
        
        if(isPatrol && navi.remainingDistance <= 3.0f && !isVision && !isAlert && !isHit)
        {
            _animator.SetBool("walk", false);
            StartCoroutine(IePatrol());
        }
        
        if (!isVision && isAlert && !isHit)
        {
            Alert();
        }
        Distance();
    }
    
    void UpdateHP()
    {
        sliderHp.value = (float)hp/maxHp;
    }
    
    // 순찰함수
    public void Patrol()
    {
        Vector3 temp = transform.position + new Vector3(Random.Range(-rangePatrol.x, rangePatrol.x),
            Random.Range(-rangePatrol.y, rangePatrol.y), Random.Range(-rangePatrol.z, rangePatrol.z));
        
        navi.destination = temp;
        _animator.SetBool("walk", true);
    }
    

    public void Distance()
    {
        tempDistance = Vector3.Distance(transform.position, player.transform.position);
        if (tempDistance <= rangeAlert && !isHit && !isVision)
        {
            isAlert = true;
            navi.isStopped = true;
            _animator.SetBool("idleBattle", false);
        }
        else
        {
            isAlert = false;
            isPatrol = true;
            _animator.SetBool("alert", false);
            navi.isStopped = false;
        }
    }

    void Choice()
    {
        randomInt = Random.Range(0, 100);
        if (randomInt <= 20)
        {
            updatelock = true;
            StartCoroutine(IeAttack());
        }
        else if (randomInt > 20 && randomInt <= 60)
        {
            updatelock = true;
            StartCoroutine(IeAttackB());
        }
        else
        {
            updatelock = true;
            StartCoroutine(IeDefense());
        }
    }

    void Alert()
    {
        navi.isStopped = true;
        alertHandler = StartCoroutine(IeAlert());
    }
    
    public void Rotation()
    {
     
        Vector3 direct = player.transform.position - transform.position;
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation, 
            Quaternion.LookRotation(new Vector3(direct.x,0,direct.z)), 
            Time.deltaTime * 1.4f);


        // if(transform.localRotation.y < 0f && !isVision && !isHit && isAlert && !isRotation)
        // {
        //     _animator.SetBool("left", true);
        // }
        // else if(transform.localRotation.y > 0f && !isVision && !isHit && isAlert && !isRotation)
        // {
        //     _animator.SetBool("right", true);
        // }
    }

    public IEnumerator IeAttack()
    {
        escudoGB.enabled = false;
        _animator.SetTrigger("Attack");
        MoveFront();
        isAttack = true;
        yield return new WaitForSeconds(4.0f);
        updatelock = false;
        isAttack = false;
    }

    public IEnumerator IeAttackB()
    {
        escudoGB.enabled = false;
        _animator.SetTrigger("AttackB");
        MoveFront();
        yield return new WaitForSeconds(1.0f);
        isAttack = true;
        yield return new WaitForSeconds(0.9f);
        isAttack = false;
        yield return new WaitForSeconds(5.0f);
        isAttack = false;
        updatelock = false;
    }

    IEnumerator IeDefense()
    {
        escudoGB.enabled = true;
        _animator.SetBool("defense",true);
        yield return new WaitForSeconds(3.0f);
        _animator.SetBool("defense",false);
        // escudoGB.enabled = false;
        updatelock = false;
    }

    // 순찰 or 정지
    IEnumerator IePatrol()
    {
        yield return new WaitForSeconds(3.0f);  
        // Debug.Log("hhh");
        randomInt = Random.Range(0, 2);
        if (randomInt == 0 && navi.remainingDistance <= 3.0f && isPatrol)
        {
            Patrol();
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
        }
    }
    
    IEnumerator IeAlert()
    {
        _animator.SetBool("alert", true);
        yield return new WaitForSeconds(3.0f);
        
        randomInt = Random.Range(0, 2);
        if (randomInt == 0)
        {        
            // _animator.SetBool("alert", false);
            Rotation();
            yield return new WaitForSeconds(2.0f);
        }
        else
        {           
            yield return new WaitForSeconds(2.0f);
            // 코루틴 중첩실행, 다 제거
        }
        if(alertHandler !=null)
            StopCoroutine(alertHandler);
        alertHandler = null;
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isVision = true;
            isHit = true;
        }
    }private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isVision = false;
        }
    }
    
    public void Damage(int quantity)
    {
        StopCoroutine(IeAttack());
        StopCoroutine(IeAttackB());
        isAttack = false;
        escudoGB.enabled = false;
        if (hp >= 1)
        {
            isHit = true;        
            hp -= quantity;
            if (hp <= 0)
            {
                Death();
            }
            else
            {
                _animator.SetTrigger("damage");
            }
        }
        
    }
    
    
    public void Block()
    {
        isAttack = false;
        _animator.SetTrigger("block");
        AudioControllerEnemy._GB.SoundFX(AudioControllerEnemy._GB.fxEscudo);
    }

    public void Death()
    {
        isHit = false;
        isVision = false;
        isAlert = false;
        updatelock = true;
        isdeath = true;
        _animator.SetBool("defense",false);
        _animator.SetTrigger("death");   
        // _animator.enabled = false;
        navi.isStopped = true;
    }

    public void resetAttack()
    {
        // Debug.Log("resetAttack");
        isAttack = false;
    }
    
    //FX

    public void DeathFX()
    {            
        AudioControllerEnemy._GB.VoiceFX(AudioControllerEnemy._GB.fxEnemyDeath1);
    }public void DamageFX()
    {            
        AudioControllerEnemy._GB.VoiceFX(AudioControllerEnemy._GB.fxEnemyDamage1);
    }public void IdleFX()
    {            
        AudioControllerEnemy._GB.VoiceFX(AudioControllerEnemy._GB.fxEnemy1);
    }public void AttackFX()
    {            
        AudioControllerEnemy._GB.SoundFX(AudioControllerEnemy._GB.fxAttack1);
    }

    public void MoveFront()
    {
        if (navi.remainingDistance < moveFront)
        {
            Vector3 Dis = transform.position + moveFront * transform.forward;
            // 이동하고싶은 내비메쉬를 지정
            if(NavMesh.SamplePosition(Dis, out NavMeshHit hit,moveFront, NavMesh.AllAreas))
            {
                navi.SetDestination(hit.position);
            }
        }
    }
}
