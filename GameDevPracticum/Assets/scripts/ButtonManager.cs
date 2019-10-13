using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void ExitButton()
    {
        print("quitting");
        Application.Quit();
    }

    public void StartButton()
    {
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("Level Generation");
    }

    public void NextLevelButton()
    {
        SceneManager.LoadScene("Level Generation");
    }
}
