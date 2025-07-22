using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public int faseNumber;

    public void ButtonStart()
    {
        Invoke("LoadGame",0.8f);
    }

    void LoadGame()
    {
        SceneManager.LoadScene(faseNumber);
    }
}
