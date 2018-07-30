using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    static bool isEnded = false;
    public static bool isPaused = false;
    static int stageLevel = 0;

    public GameObject PauseMenuObj;
    public GameObject EventManager;
    AudioSource audioManager;

    void Awake()
    {
        Screen.SetResolution(1920,1080,true);
        audioManager = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(PauseMenuObj);
        DontDestroyOnLoad(EventManager);
    }

    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        PauseGame();
    }

    public void TitleScene()
    {
        Time.timeScale = 1;
        isPaused = false;
        audioManager.Stop();
        PauseMenuObj.SetActive(false);
        stageLevel = 0;
        SceneManager.LoadScene(stageLevel);
        Destroy(gameObject);
    }

    public void PlayGame()
    {       
        stageLevel = 1;
        SceneManager.LoadScene(stageLevel);
        audioManager.Play();
    }

    public void EndLevel()
    {
        stageLevel++;
        SceneManager.LoadScene(stageLevel,LoadSceneMode.Single);
        audioManager.Play();
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        PauseMenuObj.SetActive(false);
        Application.Quit();
    }

    public void PauseGame()
    {
        if (stageLevel != 0 && Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            PauseMenuObj.SetActive(true);
            audioManager.Pause();
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if(isPaused == true)
        {
            PauseMenuObj.SetActive(false);
            audioManager.Play();
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
