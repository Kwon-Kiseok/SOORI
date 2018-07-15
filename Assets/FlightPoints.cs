using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPoints : MonoBehaviour {

    /*
     * waypointA = 플랫폼이 이동해야 할 위치
     * waypointB = 플랫폼이 이동해야 할 위치
     * speed = 플랫폼이 이동하는 스피드
     * directionAB = waypointA에서 waypointB로 향하고 있는지 아닌지 의미
    */
    public GameObject waypointA;
    public GameObject waypointB;
    public float speed = 1;
    private bool directionAB = true;

    void FixedUpdate()
    {
        /*
         * 플랫폼이 목적지에 도착했는지 아닌지 검사한다. 플랫폼이 목적지에 도착했다면 directionAB값을 반전시켜 방향을 반대로 바꾼다.
        */
        if(this.transform.position == waypointA.transform.position && directionAB == false
            || this.transform.position == waypointB.transform.position && directionAB == true)
        {
            directionAB = !directionAB;
        }
        /*
         * 현재 플랫폼 방향이 A에서 B로가면 B의 방향으로 현재 속도와 마지막으로 FixedUpdate가 호출된 델타 시간을 곱해 이동, 반대도 동일 
         */
        if(directionAB == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                        waypointB.transform.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                        waypointA.transform.position, speed * Time.fixedDeltaTime);
        }
    }
}
