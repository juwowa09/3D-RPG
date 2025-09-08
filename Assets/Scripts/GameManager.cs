using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioMixer mixer;
    public Slider sliderHp, sliderStamina;
    public GameObject menuPanel, gameOverPanel, optionPanel, inventoryPanel;
    public GameObject saveButton, yesButton, optionButton, invenButton;
    public Scrollbar scrollbar;
    public float velocity = 5f;

    public Animator menuAnimator;

    public bool gamePlay, gameOver, pause, inven;
    // Start is called before the first frame update
    private int fase;

    [Header("Invectory")] 
    public List<GameObject> items;
    public Image[] slot;
    public int[] vidaCollectible;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 또는 return;
            return;
        }

        instance = this;
    }

    void Start()
    {
        pause = false;
        gameOver = false;
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        optionPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
        Menu();
        openInven();
        GameOver();
    }

    void UpdatePlayer()
    {
        sliderHp.value = (float)Player._player.hp/Player._player.maxHp;
        sliderStamina.value = (float)Player._player.stamina / Player._player.maxStamina;
    }

    void Menu()
    {
        if (inven || gameOver) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1.0f && !pause)
            {
                menuAnimator.SetTrigger("dark");
                EventSystem.current.SetSelectedGameObject(saveButton);
                // eventSystem.firstSelectedGameObject = saveButton;
                pause = true;
                Time.timeScale = 0f;
                menuPanel.SetActive(true);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);
            }
            else if (Time.timeScale == 0f && pause)
            {
                pause = false;
                Time.timeScale = 1.0f;
                menuPanel.SetActive(false);
                optionPanel.SetActive(false);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);
            }
        }
        if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal")) && pause)
        {
            AudioController._AC.MenuFX(AudioController._AC.fxButton);
            if (Input.GetButtonDown("Cancel"))
            {
                AudioController._AC.MenuFX(AudioController._AC.fxCancel);
            }
        }
    }

    public void openInven()
    {
        if (pause || gameOver) return;
        ReadInventory();
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inven)
            {
                EventSystem.current.SetSelectedGameObject(invenButton);
                inven = true;
                Time.timeScale = 0f;
                inventoryPanel.SetActive(true);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);
            }
            else if (Time.timeScale == 0f && inven)
            {
                inven = false;
                Time.timeScale = 1.0f;
                inventoryPanel.SetActive(false);
                AudioController._AC.MenuFX(AudioController._AC.fxMenu);
            }
        }
        if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal")) && inven)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                Scroll();
            }
            AudioController._AC.MenuFX(AudioController._AC.fxButton);
            if (Input.GetButtonDown("Cancel"))
            {
                AudioController._AC.MenuFX(AudioController._AC.fxCancel);
            }
        }
    }
    
    void GameOver()
    {
        if (Player._player.death && !gameOver)
        {
            EventSystem.current.SetSelectedGameObject(yesButton);
            // eventSystem.firstSelectedGameObject = yesButton;
            gameOver = true;        
            AudioController._AC.OverFX(AudioController._AC.fxGameOver);
            StartCoroutine(IeGameOver());
        }

        if (gameOver && Input.GetButtonDown("Horizontal"))
        {
            AudioController._AC.MenuFX(AudioController._AC.fxButton);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    IEnumerator IeGameOver()
    {
        yield return new WaitForSeconds(3.0f);
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void ReStart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    
    public void ReMain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void ButtonMenu()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxButton);
        EventSystem.current.SetSelectedGameObject(saveButton);
    }public void ButtonOption()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxButton);
        EventSystem.current.SetSelectedGameObject(optionButton);
    }
    public void GameOverButtonMenu()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxButton);
        EventSystem.current.SetSelectedGameObject(yesButton);
    }
    public void InvenButton()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxButton);
        EventSystem.current.SetSelectedGameObject(invenButton);
    }

    public void Cancel()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxCancel);
    }

    public void Confirm()
    {
        AudioController._AC.MenuFX(AudioController._AC.fxConfirm);
    }

    public void SoundFx(float volume)
    {
        mixer.SetFloat("Fx", volume);
    }
    public void VoiceFx(float volume)
    {
        mixer.SetFloat("Voice", volume);
    }public void MusicFx(float volume)
    {
        mixer.SetFloat("Music", volume);
    }

    public void SaveGame()
    {
        Data data = new Data();
        data.hpData = Player._player.hp;

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath+"save.json", jsonData);
    }

    public void LoadGame()
    {
        string jsonData = File.ReadAllText(Application.persistentDataPath + "save.json");
        Data data = JsonUtility.FromJson<Data>(jsonData);

        Player._player.hp = data.hpData;
    }

    public void Scroll()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float novoValue = scrollbar.value + vertical * velocity;

        novoValue = Mathf.Clamp01(novoValue);
        scrollbar.value = novoValue;

    }

    public void AddItem(GameObject gameObject)
    {
        items.Add(gameObject);
    }
    public void ReadInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            slot[i].sprite = items[i].GetComponent<Collectible>().icon;
            vidaCollectible[i] = items[i].GetComponent<Collectible>().vida;
            slot[i].gameObject.SetActive(true);
        }
    }
}

class Data
{
    public int hpData;
}
