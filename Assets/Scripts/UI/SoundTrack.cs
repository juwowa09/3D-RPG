using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource soundTrack;
    public AudioClip[] musicClips;
    public AudioClip[] battleMusic;
    public bool sound;
    void Update()
    {
        Scenario();
        Battle();
    }
    void Scenario()
    {
        if (!MusicSensor.musicSensor && !sound)
        {
            sound = true;
            int index = Random.Range(0, musicClips.Length);
            // Debug.Log(index);
            soundTrack.clip = musicClips[index];
            soundTrack.Play();
        }
    }

    void Battle()
    {
        if (MusicSensor.musicSensor && sound)
        {
            sound = false;
            soundTrack.Stop();
            // Debug.Log(index);
            soundTrack.clip = battleMusic[0];
            soundTrack.Play();
        }
    }
}
