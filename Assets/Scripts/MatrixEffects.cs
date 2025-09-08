using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixEffects : MonoBehaviour
{
    public static MatrixEffects _instance;

    public float normalTime = 1.0f;

    public float matrixTime = 0.5f;

    public float timeToNormal = 1.0f;
    private bool activeMatrix = false;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    public void ActiveSlowCamera()
    {
        if (!activeMatrix)
        {
            activeMatrix = true;
            StartCoroutine(Normalize());
        }
    }

    public IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = matrixTime;
        yield return new WaitForSecondsRealtime(timeToNormal);
        Time.timeScale = normalTime;
        activeMatrix = false;
    }
}
