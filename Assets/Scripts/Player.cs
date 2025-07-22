using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player _player;
    [Header("Player")]
    [SerializeField] protected CharacterController Controller;
    [SerializeField] protected Animator _animator;
    private Vector3 direct;
    [SerializeField] protected float moveSpeed;
    [SerializeField] public bool death;
    [SerializeField] public int maxHp;
    [SerializeField] public int hp;
    
    [Header("CAMERA")]
    [SerializeField] protected Transform camera;
    [SerializeField] protected float timeRotation;
    private float targetAngle;
    private float angle;
    private float speedRotation;

    [Header("JUMP")]
    [SerializeField] protected Transform detector;
    public LayerMask groundLayer;
    public bool isGround = false;
    public bool jumpWait;
    public float jumpHeight;
    public float gravity = -19.62f;
    private Vector3 forceY;

    [Header("Magic")] 
    [SerializeField] protected Transform[] transformMagic;
    [SerializeField] protected GameObject[] magics;
    [SerializeField] protected bool waitMagic;
    [SerializeField] protected int idMagic;

    [Header("Custormizing")] 
    [SerializeField] protected GameObject espadaHand;
    [SerializeField] protected GameObject espadaBack;
    [SerializeField] protected GameObject escudoHand;
    [SerializeField] protected GameObject escudoBack;
    
    [Header("Animator Controller")] 
    [SerializeField] protected RuntimeAnimatorController normal; 
    [SerializeField] protected RuntimeAnimatorController espada;
    public bool change;
    
    [Header("Attack")]
    //콤보용 변수
    [SerializeField] protected bool waitEspada;
    //공격용 변수
    [SerializeField] protected bool useEspada;
    [SerializeField] protected bool modeEscudo;
    private int idCombo;

    [Header("Slash")] 
    [SerializeField] protected ParticleSystem slash1;
    [SerializeField] protected ParticleSystem slash2;
    [SerializeField] protected ParticleSystem slash4A;
    [SerializeField] protected ParticleSystem slash4B;
    [SerializeField] protected ParticleSystem slash4C;
    [SerializeField] protected ParticleSystem slash5;

    void Start()
    {
        if (_player != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _player = this;
        hp = maxHp;
        change = true;
    }
    
    private void FixedUpdate()
    {
        isGround = Physics.CheckSphere(detector.position, 0.5f, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        if (death)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direct = new Vector3(horizontal, 0, vertical);
        
        Controller.Move(forceY * Time.deltaTime);
        
        controll();
        run();
        walk();
    }

    void walk()
    {
        if (direct.magnitude >= 0.1f && !useEspada && !waitMagic)
        {
            // 오일러 각은 “회전 각도”, 쿼터니언 ->  각을 벡터로표현
            // Atan2 == 1번 매개변수 -> 2번 매개변수 각도차이 + 카메라의 방향 추가
             targetAngle = Mathf.Atan2( direct.x, direct.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            // 1번 각도 -> 2번 각도로 스무스하게 회전(물리적으로 부드러운 회전)
            // 쿼터니언 -> 오일러 바로 바꾸기
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref speedRotation, timeRotation);
            // 오일러각 -> 쿼터니언으로 변환후 할당
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            // 오일러를 쿼터니언 회전값으로, 그걸 기준으로 앞을보는 벡터를 반환
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            _animator.SetBool("walk", true);
        }
        else
        {
            _animator.SetBool("walk", false);
        }
    }
    void run()
    {
        // 누르는중
        if (Input.GetKey(KeyCode.LeftShift) && direct.magnitude >= 0.1f && !useEspada && !waitMagic)
        {
            // 오일러 각은 “회전 각도”, 쿼터니언 ->  각을 벡터로표현
            // Atan2 == 1번 매개변수 -> 2번 매개변수 각도차이 + 카메라의 방향 추가
             targetAngle = Mathf.Atan2( direct.x, direct.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            // 1번 각도 -> 2번 각도로 스무스하게 회전(물리적으로 부드러운 회전)
            // 쿼터니언 -> 오일러 바로 바꾸기
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref speedRotation, timeRotation);
            // 오일러각 -> 쿼터니언으로 변환후 할당
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            // 오일러를 쿼터니언 회전값으로, 그걸 기준으로 앞을보는 벡터를 반환
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            _animator.SetBool("run", true);
        }
        else
        {
            _animator.SetBool("run", false);
        }
    }
    
    void controll()
    {
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !jumpWait && !change)
        {
            _animator.SetBool("jump",true);
            forceY.y = Mathf.Sqrt(jumpHeight * gravity * -2);
        }
        else
        {
            forceY.y += gravity * Time.deltaTime;
            _animator.SetBool("jump",false);
            if (isGround && forceY.y < 0)
                forceY.y = -1f;
        }

        //magic
        if (Input.GetButtonDown("Fire1") && !waitMagic && !modeEscudo && !jumpWait)
        {
            change = true;
            waitMagic = true;
            jumpWait = true;
            // wait = true;
            _animator.SetTrigger("Magic");
            _animator.SetInteger("IdMagic", idMagic);
            
        }
        if (Input.GetButtonDown("Fire1") && modeEscudo && !waitEspada && idCombo != 5 && !change)
        {
            StartCoroutine(EspadaRotation());
            
            idCombo++;
            _animator.SetInteger("IdCombo",idCombo);
            // direct 벡터 기준 + 카메라 방향으로 회전 각도 계산
            jumpWait = true;
            useEspada = true;
            waitEspada = true;
        }
        
        // switching controller
        if (Input.GetButtonDown("Fire2") && !modeEscudo && !waitMagic && !change)
        {
            jumpWait = true;
            change = true;
            _animator.runtimeAnimatorController = espada;
            modeEscudo = true;
        } 
        
        else if (Input.GetButtonDown("Fire2") && modeEscudo && !useEspada && !change)
        {
            jumpWait = true;
            change = true;
            _animator.runtimeAnimatorController = normal;
            _animator.SetTrigger("Take off");
            modeEscudo = false;
        }
    }

    IEnumerator EspadaRotation()
    {
        float time = 0f;
        float duration = 0.1f; // 부드럽게 돌릴 시간 
        float currentVelocity = 0f;

        float startAngle = transform.eulerAngles.y;
        
        float targetAngle = startAngle;
        
        if(direct.magnitude >= 0.1f)
            targetAngle = Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
        
        while (time < duration)
        {
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref currentVelocity,
                duration
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            time += Time.deltaTime;
            yield return null;
        }

        // 보정 (끝났을 때 정확히 목표각도)
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
    // Event 함수
    // 음성 이벤트
    public void fxTake()
    {
        AudioController._AC.SoundFX(AudioController._AC.fxTakeEspadaFXA);
        AudioController._AC.SoundFX(AudioController._AC.fxTakeEspadaFXB);
    }
    public void fxPut()
    {
        AudioController._AC.SoundFX(AudioController._AC.fxPutEspadaFXA);
        AudioController._AC.SoundFX(AudioController._AC.fxPutEspadaFXB);
    }
    // 콤보 음성
    public void fxCombo1()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxCombo1);
        AudioController._AC.ComboFX(AudioController._AC.fxCombo1Espada);
        slash1.Play();
    }public void fxCombo2()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxCombo2);
        AudioController._AC.ComboFX(AudioController._AC.fxCombo2Espada);
    }public void fxCombo4()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxCombo4);
        AudioController._AC.ComboFX(AudioController._AC.fxCombo4Espada);
    }
    public void fxCombo5()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxCombo5);
        AudioController._AC.ComboFX(AudioController._AC.fxCombo5Espada);
    }
    //보이스 음성
    public void fxVoiceJump()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxVoiceJump);
    }
    public void fxVoiceCombo1()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxVoiceCombo1);
    }
    public void fxVoiceCombo2()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxVoiceCombo2);
    }
    public void fxVoiceCombo4()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxVoiceCombo4);
    }
    public void fxVoiceCombo5()
    {
        AudioController._AC.ComboFX(AudioController._AC.fxVoiceCombo5);
    }
    public void fxDamage()
    {
        AudioController._AC.SoundFX(AudioController._AC.fxDamage);
    }
    public void fxDead()
    {
        AudioController._AC.SoundFX(AudioController._AC.fxDeath);
    }
    
    public void Slash2(){  slash2.Play(); }

    public void Slash4A(){ slash4A.Play(); }
    
    public void Slash4B(){  slash4B.Play(); }

    public void Slash4C()
    {
        slash4C.Play();
        idMagic = 1;
        GameObject tempFire = Instantiate(magics[idMagic], transformMagic[idMagic].position,transformMagic[idMagic].rotation);
        Destroy(tempFire,1.5f);
    }
    
    public void Slash5(){  slash5.Play(); }

    public void StopSlah()
    {
        slash1.Stop();
        slash2.Stop();
        slash4A.Stop();
        slash4B.Stop();slash4C.Stop();
        slash5.Stop();
        idMagic = 0;
    }
    
    // 모드 변경 이벤트
    public void Change()
    {
        // Debug.Log("change flase");
        jumpWait = false;
        change = false;
    }
    public void Escudo()
    {
        if (!modeEscudo)
        {
            escudoHand.SetActive(false);
            escudoBack.SetActive(true);
        }
        else
        {
            escudoHand.SetActive(true);
            escudoBack.SetActive(false);
        }
        waitEspada = false;
        waitMagic = false;
    }
    
    public void Espada()
    {
        if (!modeEscudo)
        {
            espadaBack.SetActive(true);
            espadaHand.SetActive(false);
        }
        else
        {
            espadaBack.SetActive(false);
            espadaHand.SetActive(true);
        }
        waitEspada = false;
        waitMagic = false;
    }

    // 마법 이벤트
    public void FireMagic()
    {
        idMagic = 0;
        GameObject tempFire = Instantiate(magics[idMagic], transformMagic[idMagic].position,transformMagic[idMagic].rotation);
        Destroy(tempFire,3f);
    }
    
    public void ThunderMagic()
    {
        idMagic = 2;
        GameObject temp = Instantiate(magics[idMagic], transformMagic[idMagic].position,transformMagic[idMagic].rotation);
        Destroy(temp,.5f);
    }

    public void stopMagic()
    {
        // Debug.Log("GO");
        idMagic = 0;
        waitMagic = false;
        jumpWait = false;
        change = false;
    }
    
    // 콤보 이벤트
    public void ComboStart()
    {
        waitEspada = false;
    }
    public void ComboCancle()
    {
        // Debug.Log("cancle");
        if (useEspada && waitEspada &&  idCombo != 5)
            return;
        jumpWait = false;
        useEspada = false;
        waitEspada = false;
        idCombo = 0;
        _animator.SetInteger("IdCombo", idCombo);
    }
    
    // 점프 이벤트
    public void JumpTime(bool jump)
    {
        jumpWait = jump;
        change = jump;
    }
    
    public void Damage(int quantity)
    {
        // Debug.Log("hit player");
        if (hp >= 1)
        {
            hp -= quantity;
            if (hp <= 0)
            {  
            }
            else
            {
                _animator.SetTrigger("damage");
            }
        }
    }

    public void Death()
    {
        if (hp <= 0)
        {
            Debug.Log("player death");
            death = true;
            _animator.SetTrigger("death");
        }
    }
}

