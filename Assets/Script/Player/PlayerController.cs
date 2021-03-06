﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //--State elements
    public int Health;
    public bool isImmune; //현재 플레이어가 무적인지 아닌지
    public float immunityDuration = 1.5f; //무적시간이 얼마인지 나타낸다
    [SerializeField]private float immunityTime = 0f; //무적이 된 지 몇초인지 나타낸다
    [SerializeField]private bool isDead;
    [SerializeField] private bool isHit;

    //--Attack elements
    public GameObject ArrowPrefab;
    public bool canShoot = true;
    public float shootDelay;
    float shootTimer = 0;
    public int AttackDamage = 1;
    [SerializeField] private bool isHolding = false;
    private bool isJumpShot = false;

    //--Select Target elements
    public ClickManager cm;

    //--Dead event elements
    public Sprite deadSprite;

    //--SpriteFlicker elements
    private float flickerDuration = 0.1f; //스프라이트가 화면에 그려지거나 안그려지는 시간을 의미, 깜빡임의 시간 조절
    private float flickerTime = 0f; //현재 플리커의 상태가 지속된 상태의 누적 시간 

    //--Move elements
    public float movePower = 1f;
    public bool Rdir;

    //--Jump elements
    public float jumpPower = 1f;
    public int jumpCount = 2;
    bool isJumping = false;
    public bool isFalling = false;

    //--BackJump elements
    public float backjumpX = 1f;
    public float backjumpY = 1f;
    bool isBackStep = false;
    private float backjumpCoolTime;

    //--Dash elements
    public float dashSpeed;
    public float startDashTime;
    public int direction;
    private float dashCoolTime;
    private float dashTime;

    //--Camera Shake elements
    public float camShakeAMT = 0.1f;
    public GameObject camShake;


    //--AudioSource Arrow
    public AudioClip D1Arrow_audio;
    public AudioClip D2Arrow_audio;
    public AudioClip D3Arrow_audio;

    //--AudioSource PlayerState
    public AudioClip Hit_audio;


    //Components
    Rigidbody2D rigid;
    Animator animator;
    Vector3 movement;
    SpriteRenderer spriteRenderer;
    AnimatorStateInfo animatorState;
    AudioSource audio;
    GameObject GM;

    void OnEnable()
    {
        dashCoolTime = 0.01f; //대쉬 쿨타임
        backjumpCoolTime = 0.01f; //백점프 쿨타임
        jumpCount = 0; //시작부터 점프하는 것 방지
        Rdir = true;
        isImmune = false;
        isDead = false;
    }

    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        dashTime = startDashTime;
        audio = GetComponent<AudioSource>();
        GM = GameObject.FindGameObjectWithTag("GameController");
    }

	void Update () {

        if (Time.timeScale == 0f)
            return;

        animatorState = animator.GetCurrentAnimatorStateInfo(0);
        //떨어지는 상태 체크
        FallCheck();

        //공격 발사
        ShootControl();

        //플레이어 상태 효과음 체크
        AudioState();

        //피격무적인지 아닌지 체크
        if(this.isImmune == true)
        {
            SpriteFlicker();
            immunityTime = immunityTime + Time.deltaTime;

            if(immunityTime >= immunityDuration)
            {
                this.isImmune = false;
                this.spriteRenderer.enabled = true;
                Debug.Log("Imuunity has ended");
            }
        }


        //가만히 서있을 경우
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);

        }
        //왼쪽 이동
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool("isMoving", true);
            Rdir = false;

        }
        //오른쪽 이동
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("isMoving",true);
            Rdir = true;

        }
        //점프
        if (jumpCount > 0)
        {
            if (backjumpCoolTime < 1)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    jumpCount--;
                    isJumping = true;
                    animator.SetBool("isJumping", true);
                    animator.SetTrigger("doJumping");
                }
            }
        }
        //백점프
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") == 0 )
        {
            if (animator.GetBool("isJumping") || animator.GetBool("isBackStep") || animatorState.IsName("SOORI_DASH"))
                return;
            isBackStep = true;
            animator.SetBool("isBackStep", true);
            BackJump();
        }        
	}

    void FixedUpdate()
    {
        //백점프 중에는 행동 불가
        if (!animator.GetBool("isBackStep"))
        {
            Move();
            Dash();
            Jump();
        }

    }

    void LateUpdate()
    {
        if (dashCoolTime > 0)
            dashCoolTime -= Time.smoothDeltaTime;
        if (backjumpCoolTime > 0)
            backjumpCoolTime -= Time.smoothDeltaTime;
    }

    //-------[Moving Function]---------------
    void Move()
    {
        //캐릭터 공격 애니메이션 동안은 이동 불가
        if (animatorState.IsName("SOORI_SHOT_HOLD") || isDead == true)
            return;

        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    //-------[Jumping Function]---------------
    void Jump()
    {
        if (!isJumping || isDead == true)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isJumping = false;
    }
    //-------[BackJump Function]------------
    void BackJump()
    {
        if (!isBackStep || isDead == true)
            return;

        rigid.velocity = Vector2.zero;

        if(Rdir == true && animator.GetBool("isBackStep"))
        {
            Vector2 backjumpVelocity = new Vector2(-backjumpX, backjumpY);
            rigid.AddForce(backjumpVelocity, ForceMode2D.Impulse);
        }
        else if(Rdir == false && animator.GetBool("isBackStep"))
        {
            Vector2 backjumpVelocity = new Vector2(backjumpX, backjumpY);
            rigid.AddForce(backjumpVelocity, ForceMode2D.Impulse);
        }

        backjumpCoolTime = 2f;
        isBackStep = false;
        
    }

    //-------[Landing Function]---------------
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attach : " + other.gameObject.layer);
        if(other.gameObject.layer == 8)
        {
            isJumpShot = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isBackStep", false);
            animator.SetBool("isFalling", false);
            jumpCount = 2;
        }
        
    }
    //--------[MovingPlatform Function]-------
    void OnTriggerStay2D(Collider2D other)
    {
        //플레이어가 MovingPlatform 태그 위에 있을 경우 
        //움직이는 플랫폼의 자식이 되어 같이 움직임
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
        if (other.gameObject.layer == 8)
        {
            animator.SetBool("isBackStep", false);
            animator.SetBool("isFalling", false);
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Detach : " + other.gameObject.layer);
        //플레이어가 MovingPlatform 태그에서 떨어졌을 경우
        //움직이는 플랫폼의 자식에서 벗어나 같이 움직이지 않음
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

    //-------[Dash Function]---------------
    void Dash()
    {
        if (dashCoolTime > 0 || isJumpShot == true || isDead == true)
        {
            return;
        }       
            if (direction == 0)
            {
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetTrigger("Dash");
                direction = 1;
                }
                else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetTrigger("Dash");
                direction = 2;
            }
            } else
            {
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                    rigid.velocity = Vector2.zero;
                    dashCoolTime = 1f;            //대쉬 쿨타임 
                    //animator.SetTrigger("Dash");
                }
                else
                {
                    dashTime -= Time.deltaTime;

                    if (direction == 1)
                    {
                        rigid.velocity = Vector2.left * dashSpeed;
                    }
                    else if (direction == 2)
                    {
                        rigid.velocity = Vector2.right * dashSpeed;
                    }
                }
            }
        
    }


    //-------[Falling Check Function]---------
    void FallCheck()
    {
        if((rigid.velocity.y < -0.1 && animator.GetBool("isJumping")) || rigid.velocity.y <-0.1)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }
        else
        {
            isFalling = false;
            animator.SetBool("isFalling", false);
        }
    }

    //-------[Take Damage Function]----------
    public void TakeDamage(int damage, bool playHitReaction)
    {
        if (this.isImmune == false && isDead == false)
        {
            camShake.gameObject.GetComponent<CameraShake>().Shake(camShakeAMT, 0.2f);
            isHit = true;
            this.Health = this.Health - damage;
            Debug.Log("Player Health : " + this.Health.ToString());

            if(this.Health <= 0)
            {
                PlayerIsDead();
            }
            else if (playHitReaction == true)
            {
                Debug.Log("Hit Reaction Called");
                PlayHitReaction();
            }
        }
    }
   //-------[PlayHitReaction Function]----------
   void PlayHitReaction()
    {
        this.isImmune = true;
        this.immunityTime = 0f;
        //피격 애니메이션 넣어야 할 부분
        this.gameObject.GetComponent<Animator>().SetTrigger("Damage");
    }

    //--------[SpriteFlicker Function]---------
    void SpriteFlicker()
    {
        if(this.flickerTime < this.flickerDuration)
        {
            this.flickerTime = this.flickerTime + Time.deltaTime;
        }
        else if(this.flickerTime >= this.flickerDuration)
        {
            spriteRenderer.enabled = !(spriteRenderer.enabled);
            this.flickerTime = 0;
        }
    }

    //--------[PlayerIsDead Function]----------
    void PlayerIsDead()
    {
        this.isDead = true;
        this.spriteRenderer.sprite = deadSprite;
        this.animator.enabled = false;
        this.gameObject.GetComponent<Animator>().SetTrigger("Damage");
        //PlayerController controller = this.gameObject.GetComponent<PlayerController>();
        //controller.enabled = false;
        rigid.velocity = new Vector2(0, 0);
        if (Rdir == true)
            rigid.AddForce(new Vector2(-1000, 1000));
        else
            rigid.AddForce(new Vector2(1000, 1000));

        Invoke("GameOver", 3);
    }

    //--------[Shooting Animation Control Function]------
    void ShootControl()
    {
        if (canShoot == true)
        {
            //처음 버튼 클릭하고 공격자세 취하기 까지 애니메이션
            if (shootDelay > shootTimer && Input.GetMouseButtonDown(0) && !animatorState.IsName("SOORI_JUMP") && !animatorState.IsName("SOORI_WALK") && !animatorState.IsName("SOORI_JUMPSHOT"))
            {
                animator.SetBool("SHOT_BEFORE", true);
            }
            //애니메이션 무한 재생 버그방지용
            if(animator.GetBool("SHOT_BEFORE") && Input.GetMouseButtonUp(0))
            {
                animator.SetBool("SHOT_BEFORE", false);
            }

            //서있을 때 버튼 누르고 있으면서 조준 해주는 부분
            if (Input.GetMouseButton(0) && animatorState.IsName("SOORI_SHOT_BEFORE") && !animatorState.IsName("SOORI_JUMP"))
            {
                animator.SetBool("SHOT_HOLD", true);

                //조준 타이머? 데미지 정하는 함수 실행해줘야함
                //목표 대상 불러와줘야 함
                //목표 대상 변경 할 수 있어야 함
            }

            //버튼 뗀 후 발사 애니메이션 나가는 부분
            if (shootDelay > shootTimer && Input.GetMouseButtonUp(0) && animatorState.IsName("SOORI_SHOT_HOLD") )
            {
                animator.SetBool("SHOT_BEFORE", false);
                animator.SetBool("SHOT_HOLD", false);
                animator.SetTrigger("SHOT_AFTER");
                isHolding = false;

                if (ArrowPrefab.GetComponent<ArrowMover>().targeting() != null)
                {
                    Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
                    AudioArrow();
                }
                else
                    return;
            }

            //점프 샷 점프 도중에 발사하거나 점프중에 발사되게
            if((animatorState.IsName("SOORI_JUMP") || animatorState.IsName("SOORI_LANDING")) && Input.GetMouseButtonUp(0))
            {
                isJumpShot = true;
                animator.SetTrigger("SHOT_JUMP");
                if (ArrowPrefab.GetComponent<ArrowMover>().targeting() != null)
                {
                    Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
                    AudioArrow();
                }
                else
                    return;
            }


            //클릭하고 바로 뗄 경우 예외 처리 하는 부분
            else if(Input.GetMouseButtonUp(0) && !animatorState.IsName("SOORI_SHOT_HOLD"))
            {
                animator.SetBool("SHOT_BEFORE", false);
                animator.SetBool("SHOT_HOLD", false);
                //애니메이션 무한반복 되는 현상 강제정지
                if (animatorState.IsName("SOORI_SHOT_BEFORE"))
                {
                    animator.Play("SOORI_IDLE");
                }
            }
        }
    }


    //데미지마다 화살 발사 소리 다르게 차용
    void AudioArrow()
    {
        if (AttackDamage == 1)
        {
            audio.PlayOneShot(D1Arrow_audio);
        }
        else if (AttackDamage == 2)
        {
            audio.PlayOneShot(D2Arrow_audio);
        }
        else if (AttackDamage == 3)
        {
            audio.PlayOneShot(D3Arrow_audio);
        }
    }

    //플레이어의 상태마다 효과음 나오게
    void AudioState()
    {
        //피격음
        if(isHit == true)
        {
            isHit = false;
            audio.PlayOneShot(Hit_audio);
        }
    }
    
    //게임오버 화면으로
    void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    //잠시 지연
    void DelayTime()
    {
        Debug.Log("Dead");
    }
}
