using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider sliderHp;
    public GameObject menuPanel;
    public GameObject gameOverPanel;

    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
        Menu();
        GameOver();
    }

    void UpdateHP()
    {
        sliderHp.value = (float)Player._player.hp/Player._player.maxHp;
    }

    void GameOver()
    {
        if (Player._player.death && !gameOver)
        {
            gameOver = true;
            gameOverPanel.SetActive(true);
            AudioController._AC.OverFX(AudioController._AC.fxGameOver);
            Time.timeScale = 0f;
        }
    }

    void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0f;
                menuPanel.SetActive(true);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);
            }
            else if (Time.timeScale == 0f)
            {
                Time.timeScale = 1.0f;
                menuPanel.SetActive(false);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);

            }
        }
    }
}
