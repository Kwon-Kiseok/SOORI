using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//GameMaster 로 부터 상속받아서 최대한 받아올 수 있도록

public class LoadingScript : GameMaster {

    public Slider slider;
    bool isDone = false;
    float fTime = 0f;
    AsyncOperation async_operation;


    // 씬 화면 시작 된 후 로딩 되는 장면
	// StartLoad의 매개변수 strSceneName을 받아오는 법 생각해야함
	void Start () {
        StartCoroutine(StartLoad("Demo Map"));
	}
	
	// Update is called once per frame
	void Update () {
        fTime += Time.deltaTime;
        slider.value = fTime;

        if(fTime >= 5)
        {
            async_operation.allowSceneActivation = true;
        }
	}

    public IEnumerator StartLoad(string strSceneName)
    {
        //async_operation = Application.LoadLevelAsync(strSceneName);
        async_operation = SceneManager.LoadSceneAsync(strSceneName);
        async_operation.allowSceneActivation = false;

        if(isDone == false)
        {
            isDone = true;

            while(async_operation.progress < 0.9f)
            {
                slider.value = async_operation.progress;

                yield return true;
            }
        }
    }
}
