using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    /*
     * target = 원 UI 오브젝트
     * targetCenter = 원 중앙 표식 UI 오브젝트
     * temp = 원 UI 오브젝트 복사
     * temp2 = 원 중앙 표식 UI 오브젝트 복사
     */

    private GameObject playerObj;
    public GameObject target = null;
    public GameObject targetCenter = null;
    private GameObject temp = null;
    private GameObject temp2 = null;
    

   
    //Damage judge by time elements
    private float timeSpan;  //경과 시간을 갖는 변수

    //Scaling object over time
    Vector2 originalScale = new Vector2(3.0f,3.0f);
    Vector2 destinationScale = new Vector2(1.0f, 1.0f);

    public bool isTarget = false;

    void Start()
    {
        timeSpan = 0.0f;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        playerObj = GameObject.FindGameObjectWithTag("Player");

        //좌클릭 시 조준점이 생김
        if (Input.GetMouseButton(0))
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
                    temp2 = Instantiate(targetCenter, hit.collider.gameObject.transform);
                }
            }

            DamageJudge();

        }

        //좌클릭을 뗐을 때 조준점이 사라짐
        else if(Input.GetMouseButtonUp(0))
        {
            Destroy(temp);
            Destroy(temp2);
            timeSpan = 0;
        }
    }


    //--------[DamageJudge Function]----------
    void DamageJudge()
    {
        if (temp == null)
            return;


        timeSpan += Time.smoothDeltaTime; //경과 시간 등록


        //조준점 줄어드는 부분
        if (timeSpan < 1.25f)
            temp.transform.localScale = Vector2.Lerp(originalScale, destinationScale, timeSpan / 0.75f);
        //조준점 늘어나는 부분
        else if (timeSpan >= 1.25f)
            temp.transform.localScale = Vector2.Lerp(temp.transform.localScale, originalScale, (timeSpan - 1.25f)/ 12.0f);

        //0 ~ 0.4초 까지는 데미지 1 판정
        if (timeSpan >= 0 && timeSpan < 0.5f)
        {
            Debug.Log("DMG = 1");
            playerObj.GetComponent<PlayerController>().AttackDamage = 1;
            temp.GetComponent<Renderer>().material.color = Color.white;
            temp2.GetComponent<Renderer>().material.color = Color.white;
        }

        //0.5 ~ 0.9초 까지는 데미지 2 판정
        else if (timeSpan >= 0.5f && timeSpan < 1.0f)
        {
            Debug.Log("DMG = 2");
            playerObj.GetComponent<PlayerController>().AttackDamage = 2;
            temp.GetComponent<Renderer>().material.color = Color.yellow;
            temp2.GetComponent<Renderer>().material.color = Color.yellow;

        }

        //1.0 ~ 1.24초 까지는 데미지 3 판정
        else if (timeSpan >= 1.0f && timeSpan < 1.25f)
        {
            Debug.Log("DMG = 3");
            playerObj.GetComponent<PlayerController>().AttackDamage = 3;
            temp.GetComponent<Renderer>().material.color = Color.red;
            temp2.GetComponent<Renderer>().material.color = Color.red;
        }

        //1.25 ~ 1.4초 까지는 데미지 3 판정
        else if (timeSpan >= 1.25f && timeSpan < 1.5f)
        {
            Debug.Log("DMG = 3");
            playerObj.GetComponent<PlayerController>().AttackDamage = 3;
            temp.GetComponent<Renderer>().material.color = Color.red;
            temp2.GetComponent<Renderer>().material.color = Color.red;
        }

        //1.5 ~ 1.9초 까지는 데미지 2 판정
        else if (timeSpan >= 1.5f && timeSpan < 2.0f)
        {
            Debug.Log("DMG = 2");
            playerObj.GetComponent<PlayerController>().AttackDamage = 2;
            temp.GetComponent<Renderer>().material.color = Color.yellow;
            temp2.GetComponent<Renderer>().material.color = Color.yellow;
        }

        //2.0 ~ 2.4초 까지는 데미지 1 판정
        else if (timeSpan >= 2.0f && timeSpan < 2.5f)
        {
            Debug.Log("DMG = 1");
            playerObj.GetComponent<PlayerController>().AttackDamage = 1;
            temp.GetComponent<Renderer>().material.color = Color.white;
            temp2.GetComponent<Renderer>().material.color = Color.white;
        }

        //2.5이 넘어가면 0부터 루프 시켜 줌
        else
        {
            timeSpan = 0;
            playerObj.GetComponent<PlayerController>().AttackDamage = 1;
        }
    }

}
