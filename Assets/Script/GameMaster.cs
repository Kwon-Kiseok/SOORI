using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    static bool isEnded = false;
    public static bool isPaused = false;
    static int stageLevel = 0;
    AudioSource audioManager;

    void Awake()
    {
        audioManager = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        PauseGame();
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
        SceneManager.LoadScene(stageLevel, LoadSceneMode.Single);
        audioManager.Play();
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            audioManager.Pause();
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if (isPaused == true)
        {

            if(GUILayout.Button("Continue?",GUILayout.Width(400), GUILayout.Height(80)))
            {
                Time.timeScale = 1;
                isPaused = false;
                audioManager.Play();
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
