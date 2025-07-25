using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    // Start is called before the first frame update
    public int faseNumber;
    private float time;
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 3.0f)
        {
            SceneManager.LoadScene(faseNumber);
        }
    }
}
