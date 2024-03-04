using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject tutorialUI;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.i.currentGameState != GameManager.GameState.Dead)
            {
                if (GameManager.i.isPaused)
                    Resume();
                else
                    Paused();
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        GameManager.i.PauseGame(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        tutorialUI.SetActive(false);
        GameManager.i.PauseGame(false);
    }

    void Paused()
    {
        pauseMenuUI.SetActive(true);
        GameManager.i.PauseGame(true);
    }

    public void Restart()
    {
        GameManager.i.PauseGame(false);
        SceneManager.LoadScene(0);
    }
    public void ShowTutorial(bool tutorial)
    {
        pauseMenuUI.SetActive(!tutorial);
        tutorialUI.SetActive(tutorial);
    }
}
