using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource soundTrack;
    public AudioClip[] musicClips;
    void Start()
    {
        int index = Random.Range(0, musicClips.Length);
        Debug.Log(index);
        soundTrack.clip = musicClips[index];
        soundTrack.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
