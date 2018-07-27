using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float moveSpeed = 1f;
    public float moveDelay;
    //public float traceRange;

    [SerializeField] private bool isTracing;
    /*
     * 몬스터 타입 설정
     * 0 : 자유 이동 유형
     * 1 : 추적 이동 유형
     */
    public int enemyType;

    GameObject traceTarget;

    Animator animator;
    Vector3 movement;
    int movementFlag = 0; // 0:Idle , 1: Left, 2: Right

    //-------[Override Function]--------

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine("ChangeMovement");
    }

    void FixedUpdate()
    {
        Move();
        
    }

    //-------[Tracing Trigger Function]-------
    ////Trace Start
    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyType == 0)
            return;

        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            StopCoroutine("ChangeMovement");
        }
    }
    //Trace Maintain
    void OnTriggerStay2D(Collider2D other)
    {
        if (enemyType == 0)
            return;
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            //animator.SetBool("isMoving", true);
        }
    }
    //Trace Over
    void OnTriggerExit2D(Collider2D other)
    {
        if (enemyType == 0)
            return;
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;

            StartCoroutine("ChangeMovement");
        }
    }

    //-------[Movement Function]-------
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        //방향
        string dir = "";

        //Trace or Random
        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                dir = "Left";
            else if (playerPos.x > transform.position.x)
                dir = "Right";
        }
        else
        {
            if (movementFlag == 1)
            {
                dir = "Left";
            }
            else if (movementFlag == 2)
            {
                dir = "Right";
            }
        }

        if(dir == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(dir == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    IEnumerator ChangeMovement()
    {
        //랜덤으로 방향 전환
        movementFlag = Random.Range(0, 3);


        /*
         * 몬스터 IDLE과 MOVE 애니메이션 재생부분
        if (movementFlag == 0)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);
         */
        yield return new WaitForSeconds(moveDelay);

        StartCoroutine("ChangeMovement");
    }
}
