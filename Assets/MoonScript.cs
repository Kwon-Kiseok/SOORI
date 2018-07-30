using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour {


    private float playerObjX;
    //달과 플레이어 사이의 X좌표 떨어진 거리
    public float moonOffset;

    private GameObject playerObj;

	// Use this for initialization
	void Start () {
        
	}
    void Update()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void LateUpdate () {

        //달이 390에 도착하면 더이상 움직이지 않음 -> 보스전 
        if (transform.position.x >= 390)
        {
            return;
        }
        //그 외의 경우에 플레이어를 따라 다님
        else
        {
            playerObjX = playerObj.transform.position.x;
            transform.position = new Vector3(playerObjX + moonOffset, transform.position.y);
        }
	}
}
