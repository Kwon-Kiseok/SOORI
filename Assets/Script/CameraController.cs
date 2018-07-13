using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class CameraController : MonoBehaviour {

    //카메라가 쫓아갈 타겟
    public Transform target;

    //캐릭터의 방향
    [SerializeField]private bool Rdir = true;

    //카메라 오프셋
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = -10f;
    public float followSpeed = 1f;

    //아래로 볼 때 시야확보 되는 거리값
    public float downviewY = 0f;

    //카메라 가두기 위한 좌표값
    public float xMin = 8f;
    public float xMax = 100f;
    public float yMin = 0f;
    public float yMax = 10f;

    Vector3 cameraPosition;

    public float smoothTime = .15f;

    //카메라 방향전환 쿨타임
    //[SerializeField] private float rotateCoolTime;


    void Update()
    {

        //캐릭터가 보는 방향 체크
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Rdir = true;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Rdir = false;
        }
    }

    void FixedUpdate()
    {
        //캐릭터가 오른쪽을 보고 있을 경우의 카메라 무빙
        if (Rdir == true)
        {

            if (cameraPosition.x > xMin && cameraPosition.x < xMax && cameraPosition.y > yMin && cameraPosition.y < yMax)
            {
                cameraPosition.x = target.transform.position.x + offsetX;
                cameraPosition.y = target.transform.position.y + offsetY;
                cameraPosition.z = target.transform.position.z + offsetZ;

                transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
                if (cameraPosition.x > xMax)
                    return;
            }
            else
            {
                cameraPosition.x = target.transform.position.x + offsetX;
                cameraPosition.y = target.transform.position.y + offsetY;
                cameraPosition.z = target.transform.position.z + offsetZ;

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax),
                    Mathf.Clamp(transform.position.y, yMin, yMax),
                    Mathf.Clamp(transform.position.z, -10, -10));
            }

        }
        //캐릭터가 왼쪽을 보고 있을 경우의 카메라 무빙
        else if (Rdir == false)
        {

            if (cameraPosition.x > xMin && cameraPosition.x < xMax && cameraPosition.y > yMin && cameraPosition.y < yMax)
            {
                cameraPosition.x = target.transform.position.x - offsetX;
                cameraPosition.y = target.transform.position.y + offsetY;
                cameraPosition.z = target.transform.position.z + offsetZ;


                transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
                if (cameraPosition.x < xMin)
                    return;
            }
            else
            {
                cameraPosition.x = target.transform.position.x - offsetX;
                cameraPosition.y = target.transform.position.y + offsetY;
                cameraPosition.z = target.transform.position.z + offsetZ;


                transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax),
                    Mathf.Clamp(transform.position.y, yMin, yMax),
                    Mathf.Clamp(transform.position.z, -10, -10));
            }
        };      

        //아래키 누를 때 카메라 무빙
        if(Input.GetKey(KeyCode.DownArrow) && cameraPosition.y > yMin)
        {
            cameraPosition.y = target.transform.position.y - downviewY;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
        }
        //위키 누를 때 카메라 무빙
        else if (Input.GetKey(KeyCode.UpArrow) && cameraPosition.y < yMax)
        {
            cameraPosition.y = target.transform.position.y + downviewY;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
        }
    }




}
