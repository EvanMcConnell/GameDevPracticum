using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    bool ResumeButtonPressed = false;
    public GameObject PauseMenuUI, QuitConfirmationUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || ResumeButtonPressed == true)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    void setResumeButtonPressed()
    {
        ResumeButtonPressed = true;
    }


    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void ShowQuitConfirmationMenu()
    {
        QuitConfirmationUI.SetActive(true);
    }

    public void HideQuitConfirmationMenu()
    {
        QuitConfirmationUI.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
