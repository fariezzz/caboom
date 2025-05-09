﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
    }

    public void ContinueOrExitGame()
    {
        Time.timeScale = 1;
    }
}
