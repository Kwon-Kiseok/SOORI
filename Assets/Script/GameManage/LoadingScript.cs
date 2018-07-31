using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//GameMaster 로 부터 상속받아서 최대한 받아올 수 있도록

public class LoadingScript : MonoBehaviour {

    public Slider slider;
    bool isDone = false;
    float fTime = 0f;
    public int stageLevel;
    AsyncOperation async_operation;
    private GameObject gameMaster;

    // 씬 화면 시작 된 후 로딩 되는 장면

	void Start () {

        //gameMaster를 받아와서 stageLevel에 GameMaster에 있는 stageLevel을 받아와 넣어 줌
        //코루틴으로 로딩해줌
        gameMaster = GameObject.FindGameObjectWithTag("GameController");
        stageLevel = gameMaster.GetComponent<GameMaster>().stageLevel;
        StartCoroutine(StartLoad(stageLevel));
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

    public IEnumerator StartLoad(int lvl)
    {
        //async_operation = Application.LoadLevelAsync(strSceneName);
        async_operation = SceneManager.LoadSceneAsync(lvl);
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
