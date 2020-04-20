using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject help, options, back;


    void Start()
    {
        help.SetActive(false);
        options.SetActive(false);
        back.SetActive(false);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Help()
    {
        help.SetActive(true);
        back.SetActive(true);
    }

    public void Options()
    {
        options.SetActive(true);
        back.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        Start();
    }
}
