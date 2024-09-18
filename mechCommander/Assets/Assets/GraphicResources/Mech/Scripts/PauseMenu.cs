using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool ControlsStatus = false;

    public GameObject pauseMenuUI;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject pauseButtons;
    void Start()
    {
        
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                if (GamePaused)
                {
                    if (ControlsStatus)
                    {
                        Pause();
                    }
                    else
                    {
                        Resume();   
                    }
                }
                else if (!GamePaused)
                {
                    Pause();
                }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        controls.SetActive(false);
        Time.timeScale = 1f;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            a.Play();
        }

        GamePaused = false;
        ControlsStatus = false;

        Cursor.visible = false; //makes cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButtons.SetActive(true);
        controls.SetActive(false);
        Time.timeScale = 0f;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            a.Pause();
        }

        GamePaused = true;
        ControlsStatus = false;

        Cursor.visible = true; //makes cursor visible
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Controls()
    {
        ControlsStatus = true;
        pauseButtons.SetActive(false);
        controls.SetActive(true);
    }

    public void LoadMenu()
    {
        Debug.Log("LoadMenu(pauseMenu)");
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit(pauseMenu)");
        Application.Quit();
    }
}
