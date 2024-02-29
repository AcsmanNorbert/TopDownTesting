using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject quitMenu;
    //public GameObject gameSettings;
    //public GameObject cutscene;
    //public VideoPlayer videoPlayer;

    private void Start()
    {
        mainMenu.GetComponent<Animator>().SetBool("showMenu", true);

        //cutscene.SetActive(false);
        //videoPlayer = cutscene.GetComponent<VideoPlayer>();
    }
    /*
    public void PlayGame()
    {
        //cutscene.SetActive(true);
        mainMenu.GetComponent<Animator>().SetTrigger("StartCutscene");
        StartCoroutine(StartCutscene());
    }*/
    
    /*IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
        time = (float)videoPlayer.clip.length - 1f;
    }*/

    public void QuitMenu()
    {
        mainMenu.GetComponent<Animator>().SetBool("showMenu", false);
        quitMenu.GetComponent<Animator>().SetBool("showMenu", true);
    }
    /*
    public void SettingsMenu()
    {
        mainMenu.GetComponent<Animator>().SetBool("showMenu", false);
        gameSettings.GetComponent<Animator>().SetBool("showMenu", true);
    }*/

    /*public void MainMenuOpen()
    {
        gameSettings.GetComponent<Animator>().SetBool("showMenu", false);
        mainMenu.GetComponent<Animator>().SetBool("showMenu", true);
    }*/

    public void QuitGame(bool choice)
    {
        if (choice)
            Application.Quit();/*
        else
        {
            mainMenu.GetComponent<Animator>().SetBool("showMenu", true);
            quitMenu.GetComponent<Animator>().SetBool("showMenu", false);
        }*/
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    /*float currentTime;
    float time;

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            currentTime = (float)videoPlayer.time;
            if (currentTime >= time)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }*/
}
