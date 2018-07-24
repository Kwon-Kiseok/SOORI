using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour {

    public Transform tempPos;
    public float speed = 1f;
    private bool isRight = true;
    private Rigidbody2D rigid = null;
    private PlayerController playerObj = null;


	// Use this for initialization
	void Start () {
        
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        rigid = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);

        if(playerObj.Rdir == true)
        {
            isRight = true;
        }
        else if(playerObj.Rdir == false)
        {
            isRight = false;
        }
	}
    void Update()
    {

        //플레이어가 오른쪽을 보고 있고 타겟된 몬스터 포지션이 플레이어보다 오른쪽에 있어야 함
        if (isRight == true && targeting().position.x > playerObj.transform.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, targeting().position, speed * Time.deltaTime);
        }
        //플레이어가 왼쪽을 보고 있고 타겟된 몬스터 포지션이 플레이어보다 왼쪽에 있어야 함
        else if (isRight == false && targeting().position.x < playerObj.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, targeting().position, speed * Time.deltaTime);
        }
        //화살 역방향 가는거 막음
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //플랫폼에 부딪힐 경우 화살 사라짐
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    public Transform targeting()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    tempPos = hit.collider.gameObject.transform;
                }
            }
            else
                tempPos = null;
        }

        return tempPos;
    }    
}
