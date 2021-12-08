using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
        Debug.Log("quit the game");
    }

    public void nextScene()
    {
        SceneManager.LoadScene("instructionsScene");
    }
}
