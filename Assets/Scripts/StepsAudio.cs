using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StepsAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected AudioSource steps;
    private AudioClip clips;
    [SerializeField] protected List<AudioClip> groundStep;
    [SerializeField] protected List<AudioClip> aquaStep;
    [SerializeField] protected List<AudioClip> curList;

    public void Step()
    {
        clips = curList[Random.Range(0, curList.Count)];
        steps.PlayOneShot(clips);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            curList = groundStep;
        }else if (hit.gameObject.CompareTag("Aqua"))
        {
            curList = aquaStep;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default")) return;
        if (other.gameObject.CompareTag("Ground"))
        {
            curList = groundStep;
        }else if (other.gameObject.CompareTag("Aqua"))
        {
            curList = aquaStep;
        }
    }
}
