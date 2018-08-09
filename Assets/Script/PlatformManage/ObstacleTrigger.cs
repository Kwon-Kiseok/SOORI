using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject trigger = GetNearestActiveCheckPoint();
            if (trigger != null)
            {
                other.transform.position = trigger.transform.position;
            }
            else
            {
                Debug.Log("No Valid checkpoint was found!");
            }
        }
        else if(other.tag == "Enemy")
        {
            return;
        }
        else if(other.tag == "Arrow")
        {
            return;
        }
        else if(other.tag == "FallingPlatform")
        {
            return;
        }
        else
        {
            Destroy(other.gameObject);
        }
        
    }

    /* 
     * -------[GetNearestActiveCheckPoint Function]----------
     * checkpoint 태그를 가진 게임오브젝트들을 모두 찾아내 게임오브젝트 배열에 담아둔다.
     * 활성화 되어있는 가장 가까운 체크 포인트를 저장하고 있는 변수를 만든다.
     * 체크포인트와 장애물 사이의 거리를 계산한다. 
     * GetComponent로 트리거가 활성화 되어 있는지 검사한다.
     */

    GameObject GetNearestActiveCheckPoint()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        GameObject nearestCheckpoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach(GameObject checkpoint in checkpoints)
        {
            Vector3 checkpointPosition = checkpoint.transform.position;
            float distance = (checkpointPosition - transform.position).sqrMagnitude;
            CheckPoint trigger = checkpoint.GetComponent<CheckPoint>();
            if(distance < shortestDistance && trigger.isTriggered == true)
            {
                nearestCheckpoint = checkpoint;
                shortestDistance = distance;
            }
        }
        return nearestCheckpoint;
    }

}
