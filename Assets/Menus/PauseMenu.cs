using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject _pauseMenu;

    void Start()
    {
        print("pause menu loaded");
        _pauseMenu = GameObject.Find("PauseMenuCanvas");
        _pauseMenu.SetActive(false);
        //GameObject.DontDestroyOnLoad(_pauseMenu);
        //GameObject.DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Escape key was pressed");
            if (GameManager.Instance.IsPaused)
            {
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                print("Resume");
            }
            else
            {
                PauseGame();
                print("Pause");
            }
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        GameManager.Instance.IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        GameManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Stats()
    {
        //TODO : Stats
    }
    
    public void MainMenu() 
    {
        print("Go to Main Menu");;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        //GameObject.DontDestroyOnLoad(_pauseMenu);
        //GameObject.DontDestroyOnLoad(this);
    }

}
