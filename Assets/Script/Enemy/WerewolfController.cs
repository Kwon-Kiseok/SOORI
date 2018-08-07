using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfController : EnemyController {

    private Rigidbody2D rigid;
    [SerializeField] private GameObject playerObj;
    Animator animator;
    AnimatorStateInfo animatorState;
    private GameObject cm;
    //AudioSource audio;
    public GameObject hitParticle;

    private float Distance;
    private bool isHit;

    //보스 페이즈 
    private int phase = 1;

    //패턴 트리거 변수들
    private bool isWolfRun = true;
    private int WolfRunCount = 0;
    private bool isWolfHowling = false;
    private bool isWolfScratch = false;
    private bool isWolfRush = false;
    private bool PatternCycle = false;
    public bool Appearance = false;

    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("Camera");
        animator = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        Raycasting();
        
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
        playerObj = GameObject.FindGameObjectWithTag("Player");
        Distance = Vector2.Distance(transform.position, playerObj.transform.position);    

        if(isHit == true)
        {
            StartCoroutine("isHitting");
        }
        if(CurrentHealth <= 0)
        {
            StartCoroutine("isDead");
        }
    }

    void FixedUpdate()
    {

        //보스전 시작일 경우 활성화
        if (cm.GetComponent<CameraControl>().MeetTheBoss == true)
        {
            //달리기 패턴
            WolfRun();
            //하울링 패턴
            WolfHowling();
            //손톱 활퀴기 패턴
            WolfScratch();
            //페이즈 전환
            WolfPhaseChange();
            //달려드는 패턴
            WolfRush();

            if(PatternCycle == true && animatorState.IsName("BOSS_WEREWOLF_RUSH"))
            {
                isWolfRun = true;
                PatternCycle = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {       
        //화살에 맞았을 때
        if (other.tag == "Arrow" && CurrentHealth > 0)
        {
            isHit = true;
            CurrentHealth -= playerObj.GetComponent<PlayerController>().AttackDamage;
            Destroy(other.gameObject);
            if(CurrentHealth > 0 && animatorState.IsName("BOSS_WEREWOLF_IDLE"))
            {
                animator.SetTrigger("HIT");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //패트롤 콜라이더 부딪힐 때 돌려줄 부분 
        if (other.gameObject.tag == "ObstaclePlatform")
        {
            if (isFacingRight == true)
            {
                rigid.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            }
            WolfRunCount++;
            Flip();
        }
    }

    //보스전 시작할 때 제자리 하울링 애니메이션 한번 시작
    //1. 플레이어와 거리가 인지범위 밖일 경우 직진으로 달림 / 화면 끝에 갈 경우 돌아서 반대로
    //2. 1번 한번 한 다음 , 점프해서 달려듬 2~3번 정도
    //3. 가까울 경우는 왠만해서 손톱휘두르기 패턴 나오게 
    //2번이 끝나면 1번 트리거 다시 발동 반복
    //체력 반정도 되었을 때 2페이즈 알리는 하울링 한번
    //전체적인 속도 증가 
    //보스 위치로 오면 패트롤 콜라이더 두 개 양쪽에 생성 - 패트롤 콜라이더 부딪힐 때 까지 런 -> 부딪히면 방향 바꿔줌 
    //피격 애니메이션은 IDLE때만 나오도록 (패턴 방해 불가)


    //늑대 달리기 패턴
    void WolfRun()
    { 
        //2페이즈일 경우 강제 하울링
        if(phase == 2 && isWolfHowling == true && animatorState.IsName("BOSS_WEREWOLF_RUN"))
        {
            animator.SetInteger("BOSSSTATE", 0);          
            isWolfRun = false;
            WolfRunCount = 0;
        }


        if (WolfRunCount < 3 && isWolfRun == true)
        {
            animator.SetInteger("BOSSSTATE", 1);

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
        else if(WolfRunCount == 3)
        {
            WolfRunCount = 0;
            isWolfRun = false;
            animator.SetInteger("BOSSSTATE", 0);
            isWolfHowling = true;
        }
    }

    //늑대 하울링 패턴
    void WolfHowling()
    {
         

        ////등장할 때
        //if(Appearance == true)
        //{
        //    animator.SetInteger("BOSSSTATE", 3);
        //    Appearance = false;

        //}
        //달리기 패턴 끝난 후에
        if (isWolfHowling == true && animatorState.IsName("BOSS_WEREWOLF_IDLE"))
        {
            //대상이 왼쪽에 있을 경우
            if (transform.position.x > playerObj.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingRight = false;
            }
            //대상이 오른쪽에 있을 경우
            else if (transform.position.x < playerObj.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingRight = true;
            }

            animator.SetInteger("BOSSSTATE", 3);

            isWolfHowling = false;
        }
        //하울링 한번 하고 다시 ..
        else if(isWolfHowling == false && animatorState.IsName("BOSS_WEREWOLF_HOWLING"))
        {
            animator.SetInteger("BOSSSTATE", 0);
            isWolfRush = true;
        }
        
    }

    //늑대 손톱 활퀴기 패턴
    void WolfScratch()
    {
        if (spotted == true && !animatorState.IsName("BOSS_WEREWOLF_RUN"))
        {
            isWolfScratch = true;
            //오른쪽 보고 있었을 때
            if (isFacingRight == true && (sightEnd.position.x - sightStart.position.x) / 2 > Distance)
            {
                rigid.AddForce(new Vector2(10,0), ForceMode2D.Impulse);
                animator.SetInteger("BOSSSTATE", 2);
            }
            //왼쪽 보고 있었을 때
            else if (isFacingRight == false && (sightStart.position.x - sightEnd.position.x) / 2 > Distance)
            {
                rigid.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
                animator.SetInteger("BOSSSTATE", 2);
            }           
        }
        else if(spotted == false && !animatorState.IsName("BOSS_WEREWOLF_RUN") && isWolfScratch == true)
        {
            animator.SetInteger("BOSSSTATE", 0);
            isWolfScratch = false;
        }
    }

    //늑대 달려드는 패턴
    void WolfRush()
    {
        if(isWolfRush == true && animatorState.IsName("BOSS_WEREWOLF_IDLE"))
        {
            //대상이 왼쪽에 있을 경우
            if(transform.position.x > playerObj.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFacingRight = false;
            }
            //대상이 오른쪽에 있을 경우
            else if(transform.position.x < playerObj.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingRight = true;
            }

            if (phase == 1)
            {
                //오른쪽 보고 있었을 때
                if (isFacingRight == true)
                {
                    rigid.AddForce(new Vector2(50, 30), ForceMode2D.Impulse);
                    animator.SetInteger("BOSSSTATE", 4);
                    isWolfRush = false;
                }
                //왼쪽 보고 있었을 때
                else if (isFacingRight == false)
                {
                    rigid.AddForce(new Vector2(-50, 30), ForceMode2D.Impulse);
                    animator.SetInteger("BOSSSTATE", 4);
                    isWolfRush = false;
                }
            }
            else if(phase == 2)
            {
                //오른쪽 보고 있었을 때
                if (isFacingRight == true)
                {
                    rigid.AddForce(new Vector2(60, 30), ForceMode2D.Impulse);
                    animator.SetInteger("BOSSSTATE", 4);
                    isWolfRush = false;
                }
                //왼쪽 보고 있었을 때
                else if (isFacingRight == false)
                {
                    rigid.AddForce(new Vector2(-60, 30), ForceMode2D.Impulse);
                    animator.SetInteger("BOSSSTATE", 4);
                    isWolfRush = false;
                }
            }
        }
        else if(isWolfRush == false && animatorState.IsName("BOSS_WEREWOLF_RUSH"))
        {
            animator.SetInteger("BOSSSTATE", 0);
            PatternCycle = true;
        }
    }

    //늑대 페이즈 전환
    void WolfPhaseChange()
    {
        //체력 반 이하일 경우 페이즈 2로 전환
        if(phase == 1 && CurrentHealth <= MaxHealth/2)
        {
            phase = 2;
            maxSpeed *= 2;
        }
    }

   


    IEnumerator isHitting()
    {
        yield return new WaitForSeconds(0.25f);
        isHit = false;
    }

    IEnumerator isDead()
    {
        animator.SetTrigger("DEAD");
        WerewolfController controller = this.gameObject.GetComponent<WerewolfController>();
        controller.enabled = false;

        yield return new WaitForSeconds(1.5f);
        if(CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
   
}
