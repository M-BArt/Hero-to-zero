using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool _gameIsPause = false;
    [SerializeField] private GameObject _pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverMenu._gameEnds)
        {
            if(_gameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _gameIsPause = false;
    }
    void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPause=true;
    }

    public void LoadMenu()
    {
        _gameIsPause = false;
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
