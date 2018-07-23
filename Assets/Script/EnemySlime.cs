using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController {

    private Rigidbody2D rigid;
    private GameObject playerObj;

    //유닛의 타입 결정 0 = 기본 1 = 추적형
    public int EnemyType;
    //몬스터와 캐릭터 사이의 거리
    private float Distance;
    //몬스터의 감지 범위
    public float TraceRange = 1f;
    //피격 시 체크
    [SerializeField] private bool isHit = false;
    //경직 시간
    public float stunTime;

    //돌진 공격 
    public bool rushAttack = false;
    public float rushX = 1f;
    public float rushY = 1f;
    public float rushRate;
    private float nextRush;

    public int Health = 3;

    [SerializeField] private bool isTracing = false;

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();       
    }

    void Update()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Distance = Vector2.Distance(transform.position, playerObj.transform.position);

        Raycasting();

        if (isHit == true)
        {
            StartCoroutine("isHitting");
        }
    }

    void FixedUpdate()
    {
        //기본형 이동
        if (EnemyType == 0 && isHit == false)
        {
            Move();
        }
        //추적형 이동
        else if(EnemyType == 1 && isHit == false)
        {

            //추적 범위 내 일 경우
            if(Distance < TraceRange)
            {
                //대상이 보다 오른쪽에 있을 경우
                if(playerObj.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                //대상이 보다 왼쪽에 있을 경우
                else if(playerObj.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                isTracing = true;
                transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, maxSpeed * Time.deltaTime);
                
            }
            //추적 범위 밖 일 경우
            else
            {
                isTracing = false;
                Move();
            }
        }

        //날아다니는 추적형
        else if(EnemyType == 2 && isHit == false)
        {
            //추적 범위 내 일 경우
            if(Distance < TraceRange)
            {
                //대상이 보다 오른쪽에 있을 경우
                if(playerObj.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if(playerObj.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                isTracing = true;
                transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, maxSpeed * Time.deltaTime);
            }
            else
            {
                isTracing = false;                
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow" && Health > 0)
        {
            isHit = true;
            Destroy(other.gameObject);
            Health -= playerObj.GetComponent<PlayerController>().AttackDamage;           
        }
        if (Health == 0)
        {
            Destroy(gameObject);
        }

        //플레이어 부딪히면 돌려줌
        if(other.tag == "Player")
        {
            Flip();
        }
        //방해물과 부딪히면 돌려줌
        else if(other.tag == "ObstaclePlatform")
        {
            Flip();
        }       
    }

    void Move()
    {
        if (this.isFacingRight == true)
        {
            rigid.velocity = new Vector2(maxSpeed, this.rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(maxSpeed * -1, this.rigid.velocity.y);
        }
    }

    public void RushAttack()
    {
        if(rushAttack == true && Time.time > nextRush)
        {
            if(this.isFacingRight == true)
            {
                Vector2 rushForce = new Vector2(rushX, rushY);
                rigid.AddForce(rushForce, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 rushForce = new Vector2(-rushX, rushY);
                rigid.AddForce(rushForce, ForceMode2D.Impulse);
            }
            nextRush = Time.time + rushRate;
        }        
    }
    IEnumerator isHitting()
    {
        yield return new WaitForSeconds(stunTime);
        isHit = false;
    }
}
