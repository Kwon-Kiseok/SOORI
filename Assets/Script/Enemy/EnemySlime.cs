using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController {

    private Rigidbody2D rigid;
    private GameObject playerObj;
    Animator animator;
    AnimatorStateInfo animatorState;
    AudioSource audio;
    public GameObject hitParticle;

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

    //원거리 공격 
    public GameObject projectile;
    public float projectileDelay;
    [SerializeField]
    private bool isShot = false;

    //몬스터 원래 위치
    private Vector2 originTransform;

    //화면에 들어오는지 
    public bool isVisible = false;

    //몬스터 체력
    //public int Health = 3;

    //몬스터 효과음
    public AudioClip Hit_audio;
    public AudioClip[] Dead_audio;

    [SerializeField] private bool isTracing = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        originTransform = transform.position;
        audio = GetComponent<AudioSource>();
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Distance = Vector2.Distance(transform.position, playerObj.transform.position);

        Raycasting();

        if(spotted == true && EnemyType == 3 && isShot == false)
        {
            StartCoroutine("projectileAttack");
        }
        if (isHit == true && CurrentHealth >= 1)
        {
            rigid.velocity = Vector2.zero;
            StartCoroutine("isHitting");
        }
        if(CurrentHealth <= 0)
        {
            StartCoroutine("isDead");
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
            if (animatorState.IsName("01_SLIME_ATTACK"))
                animator.SetInteger("SLIMESTATE", 0);

            //추적 범위 내 일 경우
            if (Distance < TraceRange)
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
            if (animatorState.IsName("02_SLIME_ATTACK"))
                animator.SetInteger("SLIMESTATE", 0);


            //추적 범위 내 일 경우
            if (Distance < TraceRange)
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

                //대쉬 쿨타임이 다 찼을 경우 
                //해당 시간 플레이어 감지 위치로 addforce 해줌
                if (SurpriseMark.activeSelf == true && Time.time > nextRush)
                {
                    sightEnd.position = playerObj.transform.position;
                    Vector3 temp = sightEnd.position;
                    rigid.AddForce((temp - transform.position) * maxSpeed, ForceMode2D.Impulse);
                }
            }
            //추적 범위 밖 일 경우
            //대쉬 이동 속도 초기화 및 몬스터 원래 위치로 돌아감
            else if(Distance > TraceRange)
            {
                //isTracing = false;
                rigid.velocity = Vector2.zero;
                transform.position = Vector2.MoveTowards(transform.position, originTransform, 10f*Time.deltaTime);
            }
        }


        //--------------------------------------------------04 원거리형-----------------------------------------------
        else if(EnemyType == 3 && isHit == false)
        {
            if (animatorState.IsName("03_SLIME_ATTACK"))
                animator.SetInteger("SLIMESTATE", 0);

            if(Distance < TraceRange)
            {
                //대상이 보다 오른쪽에 있을 경우
                if (playerObj.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = true;
                }
                else if (playerObj.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = false;
                }
            }
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow" && CurrentHealth > 0)
        {
            //피격 파티클 이펙트
            Instantiate(hitParticle, transform.position, transform.rotation);

            //껍데기 슬라임의 경우 IDLE 일 경우 데미지 안받음
            if (EnemyType == 3 && animatorState.IsName("03_SLIME_IDLE"))
            {
                Destroy(other.gameObject);
                return;
            }
            isHit = true;
            CurrentHealth -= playerObj.GetComponent<PlayerController>().AttackDamage;
            Destroy(other.gameObject);
            //죽을 때 맞는 애니메이션 재생 방지
            if (CurrentHealth > 0)
            {
                audio.PlayOneShot(Hit_audio);
                animator.SetTrigger("HIT");
            }
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

        //슬라임끼리 부딪히면 돌려줌 껍데기 슬라임 제외
        if(other.tag == "Enemy" && EnemyType != 3)
        {
            Flip();
        }
    }
    void Move()
    {
        if (animatorState.IsName("01_SLIME_ATTACK"))
            animator.SetInteger("SLIMESTATE", 0);

        if (animatorState.IsName("01_SLIME_HIT") || animatorState.IsName("02_SLIME_HIT"))
        {
            return;
        }
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
        if (isHit == true)
            return;

        //껍데기 슬라임의 경우 돌진공격 제외
        if (EnemyType == 3)
            return;

        if(rushAttack == true && Time.time > nextRush)
        {
            if (EnemyType == 2)
            {
                if(isFacingRight == true)
                {
                   // Vector2 temp = sightEnd.position;
                   // transform.position = Vector2.Lerp(transform.position , temp, maxSpeed*Time.deltaTime);
                }
                else
                {
                  //  Vector2 temp = sightEnd.position;
                   // transform.position = Vector2.Lerp(transform.position, temp, maxSpeed*Time.deltaTime);
                }
            }
            else
            {
                if (this.isFacingRight == true)
                {
                    Vector2 rushForce = new Vector2(rushX, rushY);
                    rigid.AddForce(rushForce, ForceMode2D.Impulse);
                }
                else
                {
                    Vector2 rushForce = new Vector2(-rushX, rushY);
                    rigid.AddForce(rushForce, ForceMode2D.Impulse);
                }
            }
            //돌진 애니메이션
            animator.SetInteger("SLIMESTATE", 1);
            nextRush = Time.time + rushRate;
        }        
    }
    IEnumerator isHitting()
    {
        yield return new WaitForSeconds(stunTime);
        isHit = false;
    }

    IEnumerator isDead()
    {
        //Dead Audio Play
        audio.PlayOneShot(Dead_audio[Random.Range(0,2)]);
        //Dead Animation Play
        animator.SetTrigger("DEAD");
        //animator.SetInteger("SLIMESTATE", -1);
        EnemySlime controller = this.gameObject.GetComponent<EnemySlime>();
        controller.enabled = false;
        this.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;        

        yield return new WaitForSeconds(0.5f);
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator projectileAttack()
    {
        if (isShot == false)
        {
            animator.SetInteger("SLIMESTATE", 1);
            GameObject clone = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
            isShot = true;
        }

        yield return new WaitForSeconds(projectileDelay);

        isShot = false;
    }
}
