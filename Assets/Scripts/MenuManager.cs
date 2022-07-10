#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum State
{
    Playing = 0,
    Loading = 1,
    Paused = 2,
    MainMenu = 3
}

public struct SaveData
{
    public int PlayerHealth;
    public Vector3 PlayerPosition;
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager;
    [SerializeField] private GameObject m_pausePanel;
    private PPeffects m_effects;

    public HP_Display m_hpD;

    public State m_state = State.Playing;

    private void Awake()
    {
        if(!menuManager)
        {
            menuManager = this;
        }
        SceneManager.sceneLoaded += LevelLoaded;
        m_effects = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<PPeffects>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        switch(m_state)
        {
            case State.Playing:
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    m_state = State.Paused;
                    PauseGame();
                }
                break;
            case State.Loading:
                break;
            case State.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    m_state = State.Playing;
                    UnPauseGame(true);
                }
                break;
            case State.MainMenu:

                break;
        }
    }
    public void MainMenu()
    {
        UnPauseGame(false);
        m_state = State.MainMenu;
        SceneManager.LoadScene(0);
    }

    void PauseGame()
    {
        m_pausePanel.SetActive(true);
        m_hpD.gameObject.SetActive(false);
        Time.timeScale = 0;
        m_effects.BlurEffect = true;
    }
    public void UnPauseGame(bool showHp)
    {
        m_pausePanel.SetActive(false);
        m_hpD.gameObject.SetActive(showHp);
        Time.timeScale = 1;
        m_effects.BlurEffect = false;
    }

    public void SaveProgress(SaveData data)
    {
        PlayerPrefs.SetInt("PlayerHealth", data.PlayerHealth);
        PlayerPrefs.SetFloat("PlayerPositionX", data.PlayerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", data.PlayerPosition.y);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Health>().m_HP = PlayerPrefs.GetInt("PlayerHealth");
        Vector3 newPositon = new Vector3 
        {
            x = PlayerPrefs.GetFloat("PlayerPositionX"), 
            y = PlayerPrefs.GetFloat("PlayerPositionY"),
            z = 0
        };
    }

    void LevelLoaded(Scene argv0,LoadSceneMode argv1)
    {
        if(argv0.buildIndex>0)
        {
            m_state = State.Playing;
            m_effects = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<PPeffects>();
        }
    }
}