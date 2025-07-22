using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerEnemy : MonoBehaviour
{
    public static AudioControllerEnemy _GB;
    public AudioSource sourceFX;
    public AudioSource sourceVoice;
    // Start is called before the first frame update
    
    [Header("EnemyAttack")]
    public AudioClip fxAttack1;
    
    [Header("EnemyVoice")]
    public AudioClip fxEnemy1;
    public AudioClip fxEnemyDamage1;
    public AudioClip fxEnemyDeath1;
    private void Start()
    {
        _GB = this;
    }
    public void SoundFX(AudioClip fx)
    {
        sourceFX.clip = fx;
        sourceFX.PlayOneShot(fx);
    }
    
    public void VoiceFX(AudioClip fx)
    {
        sourceVoice.clip = fx;
        sourceVoice.PlayOneShot(fx);
    }
}
