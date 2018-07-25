using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    static bool isEnded = false;
    static int stageLevel = 0;
    public AudioSource audioManager;

    void Awake()
    {
        audioManager = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);    
    }

    public void PlayGame()
    {
        stageLevel = 1;
        SceneManager.LoadScene(stageLevel);
        audioManager.Play();
    }

    public void EndLevel()
    {
        audioManager.Pause();
        stageLevel++;
        SceneManager.LoadScene(stageLevel, LoadSceneMode.Single);
        audioManager.Play();
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

}
