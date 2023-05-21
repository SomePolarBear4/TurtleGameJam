using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_mainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelPrototype");

    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
