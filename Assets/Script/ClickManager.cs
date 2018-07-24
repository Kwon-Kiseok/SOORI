using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    public GameObject target = null;
    private GameObject temp = null;

    //Damage judge by time elements
    private float timeSpan;  //경과 시간을 갖는 변수
    private int checkTime; //특정 시간을 갖는 변수

    //Scaling object over time
    Vector3 originalScale = new Vector3(3.0f,3.0f,3.0f);
    Vector3 destinationScale = new Vector3(1.0f, 1.0f ,1.0f);

    void Start()
    {
        timeSpan = 0.0f;
        checkTime = 3;
    }

    void Update()
    {

        //우클릭 시 조준점이 생김
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.tag);

                //조준점이 생기는 부분 , 객체 하나만 생성되도록 temp가 null일 때만 생성
                if (temp == null && hit.collider.gameObject.tag == "Enemy")
                {
                    //target.transform.position = hit.collider.gameObject.transform.position;
                    temp = Instantiate(target, hit.collider.gameObject.transform);                    
                }
            }

            DamageJudge();

        }

        //우클릭을 뗐을 때 조준점이 사라짐
        else if(Input.GetMouseButtonUp(1))
        {
            Destroy(temp);
            timeSpan = 0;
        }
    }


    //--------[DamageJudge Function]----------
    void DamageJudge()
    {

        timeSpan += Time.deltaTime; //경과 시간 등록

        temp.transform.localScale = Vector3.Lerp(originalScale, destinationScale, timeSpan);

        //0~0.9초 까지는 데미지 1 판정
        if (timeSpan >= 0 && timeSpan < 1.0f)
        {
            Debug.Log("DMG = 1");
            temp.GetComponent<Renderer>().material.color = Color.black;
        }

        //1~1.9초 까지는 데미지 2 판정
        else if(timeSpan >= 1.0f && timeSpan < 2.0f)
        {
            Debug.Log("DMG = 2");
            temp.GetComponent<Renderer>().material.color = Color.yellow;

        }

        //2~2.9초 까지는 데미지 3 판정
        else if(timeSpan >= 2.0f && timeSpan < 3.0f)
        {
            Debug.Log("DMG = 3");
            temp.GetComponent<Renderer>().material.color = Color.red;

        }
        //3이 넘어가면 0부터 루프 시켜 줌
        else
        {
            timeSpan = 0;
            temp.transform.localScale = originalScale;
            temp.GetComponent<Renderer>().material.color = Color.white;
        }
    }

}
