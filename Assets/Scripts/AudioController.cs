using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static AudioController _AC;
    public AudioSource sourceFX;
    public AudioSource sourceVoice;
    public AudioSource sourceHudFX;


    [Header("Hud")] 
    public AudioClip fxMenu;
    public AudioClip fxGameOver;
    public AudioClip fxConfirm;
    public AudioClip fxCancel;
    public AudioClip fxButton;
    
    [Header("Equip")]
    public AudioClip fxPutEspadaFXA;
    public AudioClip fxPutEspadaFXB;
    public AudioClip fxTakeEspadaFXA;
    public AudioClip fxTakeEspadaFXB;
    
    [Header("Combo")]
    public AudioClip fxCombo1;
    public AudioClip fxCombo2;
    public AudioClip fxCombo4;
    public AudioClip fxCombo5;
    public AudioClip fxCombo1Espada;
    public AudioClip fxCombo2Espada;
    public AudioClip fxCombo4Espada;
    public AudioClip fxCombo5Espada;

    [Header("Voice")] 
    public AudioClip fxVoiceJump;
    public AudioClip fxVoiceCombo1;
    public AudioClip fxVoiceCombo2;
    public AudioClip fxVoiceCombo4;
    public AudioClip fxVoiceCombo5;
    public AudioClip fxDeath;
    public AudioClip fxDamage;
    private void Start()
    {
        _AC = this;
    }

    public void SoundFX(AudioClip fx)
    {
        sourceFX.clip = fx;
        sourceFX.PlayOneShot(fx);
    }
    public void ComboFX(AudioClip fx)
    {
        sourceFX.clip = fx;
        sourceFX.PlayOneShot(fx);
    }
    
    public void VoiceFX(AudioClip fx)
    {
        sourceVoice.clip = fx;
        sourceVoice.PlayOneShot(fx);
    }
    
    public void MenuFX(AudioClip fx)
    {
        sourceHudFX.clip = fx;
        sourceHudFX.PlayOneShot(fx);
    }public void OverFX(AudioClip fx)
    {
        sourceHudFX.clip = fx;
        sourceHudFX.PlayOneShot(fx, 0.3f);
    }
}
