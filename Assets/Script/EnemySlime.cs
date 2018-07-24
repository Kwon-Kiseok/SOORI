using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController {

    private Rigidbody2D rigid;
    private GameObject playerObj;
    Animator animator;
    AnimatorStateInfo animatorState;

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

    //몬스터 원래 위치
    private Vector2 originTransform;

    //화면에 들어오는지 
    public bool isVisible = false;

    public int Health = 3;

    [SerializeField] private bool isTracing = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        originTransform = transform.position;
    }

    void Update()
    {
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
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
        //--------------------------------------------------01 기본형 이동-----------------------------------------------
        if (EnemyType == 0 && isHit == false)
        {
            Move();
        }




        //--------------------------------------------------02 추적형 이동-----------------------------------------------
        else if (EnemyType == 1 && isHit == false)
        {

            //추적 범위 내 일 경우
            if(Distance < TraceRange)
            {
                //대상이 보다 오른쪽에 있을 경우
                if(playerObj.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = true;
                }
                //대상이 보다 왼쪽에 있을 경우
                else if(playerObj.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = false;
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



        //--------------------------------------------------03 날아다니는 추적형-----------------------------------------------
        else if (EnemyType == 2 && isHit == false)
        {
            //추적 범위 내 일 경우
            if(Distance < TraceRange)
            {
                //대상이 보다 오른쪽에 있을 경우
                if(playerObj.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = true;
                }
                else if(playerObj.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = false;
                }
                isTracing = true;
                transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, maxSpeed * Time.deltaTime);
            }
            //추적 범위 밖 일 경우
            //대쉬 이동 속도 초기화 및 몬스터 원래 위치로 돌아감
            else if(Distance > TraceRange)
            {
                isTracing = false;
                rigid.velocity = Vector2.zero;
                transform.position = Vector2.MoveTowards(transform.position, originTransform, (maxSpeed - 10f) * Time.deltaTime);
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
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        //방해물과 부딪히면 돌려줌
        if(other.tag == "ObstaclePlatform")
        {
            if(isFacingRight == true)
                rigid.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            else 
                rigid.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            Flip();
        }
    }
    void Move()
    {
        if (animatorState.IsName("01_SLIME_ATTACK"))
            animator.SetInteger("SLIMESTATE", 0);

        Vector3 moveVelocity = Vector3.zero;

        if (this.isFacingRight == true)
        {
            moveVelocity = Vector3.right;
        }
        else
        {
            moveVelocity = Vector3.left;
        }
        transform.position += moveVelocity * maxSpeed * Time.deltaTime;
    }


    //몬스터 돌진 어택
    public void RushAttack()
    {
        if(rushAttack == true && Time.time > nextRush)
        {
            if (EnemyType == 2)
            {
                if(isFacingRight == true)
                {
                    rigid.velocity = Vector2.right * rushX;
                }
                else
                {
                    rigid.velocity = Vector2.left * rushX;
                }
            }
            else
            {
                if (this.isFacingRight == true)
                {
                    //돌진 애니메이션
                    animator.SetInteger("SLIMESTATE", 1);
                    Vector2 rushForce = new Vector2(rushX, rushY);
                    rigid.AddForce(rushForce, ForceMode2D.Impulse);
                }
                else
                {
                    //돌진 애니메이션
                    animator.SetInteger("SLIMESTATE", 1);
                    Vector2 rushForce = new Vector2(-rushX, rushY);
                    rigid.AddForce(rushForce, ForceMode2D.Impulse);
                }
            }

            nextRush = Time.time + rushRate;
        }        
    }
    IEnumerator isHitting()
    {
        //Renderer r = this.GetComponent<Renderer>();
        //Material m = r.material;
        //m.color = Color.red;
        //r.material = m;

        yield return new WaitForSeconds(stunTime);
        isHit = false;

        //m.color = Color.white;
        //r.material = m;
    }
}
