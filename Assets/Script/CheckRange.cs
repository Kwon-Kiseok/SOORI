using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRange : MonoBehaviour {

    public Transform sightStart, sightEnd;

    public bool spotted = false;
    private GameObject playerObj = null;

    void Update()
    {
        Raycasting();
        CheckInRange();
    }
    void Raycasting()
    {
        //씬 뷰에서 선으로 영역 라인 범위를 보여주는 부분
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.red);
        //라인캐스트를 통한 spotted의 참/거짓을 정해줌 
        //레이어마스크 부분은 Default 되어 있는 부분도 전부 인식하니 수정 필요
        spotted = Physics2D.Linecast(sightStart.position, sightEnd.position, LayerMask.NameToLayer("Enemy"));
        
        
    }
    void CheckInRange()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");

        if(spotted == true)
            playerObj.gameObject.GetComponent<PlayerController>().canShoot = true;
        else if(spotted == false)
            playerObj.gameObject.GetComponent<PlayerController>().canShoot = false;
    }

}
