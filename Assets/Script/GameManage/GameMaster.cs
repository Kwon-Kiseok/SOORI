﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    static bool isEnded = false;
    public static bool isPaused = false;
    public int stageLevel = 0;
    //로딩씬 화면 1번에 넣어줌
    private int LoadingLevel = 1;

    public GameObject PauseMenuObj;
    public GameObject EventManager;
    public GameObject InGameUI;
    public GameObject CharacterManager;
    public GameObject ClickManager;

    AudioSource audioManager;

    void Awake()
    {
        Screen.SetResolution(1920,1080,true);
        audioManager = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(PauseMenuObj);
        DontDestroyOnLoad(EventManager);
        DontDestroyOnLoad(InGameUI);
        DontDestroyOnLoad(CharacterManager);
        DontDestroyOnLoad(ClickManager);
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
        //첫 스테이지는 두번째 씬에 가있음
        stageLevel = 2;
        //로딩씬을 불러다 줌
        SceneManager.LoadScene(LoadingLevel);      
        audioManager.Play();
        InGameUI.gameObject.SetActive(true);
        CharacterManager.gameObject.SetActive(true);
        ClickManager.gameObject.SetActive(true);
    }

    public void EndLevel()
    {
        //스테이지 레벨을 올림
        stageLevel++; 
        //로딩씬 불러다 줌
        SceneManager.LoadScene(LoadingLevel);
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
