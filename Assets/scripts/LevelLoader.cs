using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int currentSceneIndex;
    MusicPlayer musicPlayer;
    [SerializeField] int loadingScreenTimeInSeconds = 4;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
          StartCoroutine(LoadMainMenu());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFromStart()
    {
        //yield return new WaitForSeconds(loadingScreenTimeInSeconds);
        SceneManager.LoadScene("Splash Screen");
    }
    public void LoadNextScene()
    {
        Destroy(musicPlayer.gameObject);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public IEnumerator LoadMainMenu()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(loadingScreenTimeInSeconds);
        SceneManager.LoadScene("Main Menu");
    }
    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene("Options Screen");
    }
    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("Lose Screen");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}