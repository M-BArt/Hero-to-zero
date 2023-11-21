using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public static bool _gameEnds = false;
    [SerializeField] private GameObject _GameOverMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (_gameEnds)
        {
            TheEnd();
        }
    }

    public void NewGame()
    {
        _GameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _gameEnds = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<AudioManager>().Stop("GameOver");
        FindObjectOfType<AudioManager>().Play("Theme");
    }

    void TheEnd()
    {
        _GameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        _gameEnds = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        FindObjectOfType<AudioManager>().Stop("GameOver");
        FindObjectOfType<AudioManager>().Play("Theme");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
