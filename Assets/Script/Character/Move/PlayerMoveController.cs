using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {

    public float movePower = 1f;
    public float jumpPower = 1f;
    public float backstepPowerX = 1f;
    public float backstepPowerY = 1f;
    public float DashPower = 1f;
    public int jumpCount = 2;

    Rigidbody2D rigid;
    SpriteRenderer myRenderer;
    Animator animator;

    Vector3 movement;
    bool isJumping = false;
    bool isBackstep = false;
    bool isDash = false;
    bool doubleJump = true;
    //캐릭터 왼쪽을 보는지 오른쪽을 보는지 
    bool Ldir = false;
    bool Rdir = true;



    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        myRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }
	

	void Update () {
        //캐릭터 이동 애니메이션
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool("isMoving", true);
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("isMoving", true);
        }

        //캐릭터 점프 애니메이션 
        if (Input.GetButtonDown("Jump") && (!animator.GetBool("isJumping") || (animator.GetBool("isJumping") && doubleJump)))
        {
            jumpCount--;
            isJumping = true;
            animator.SetBool("isJumping", true); //점프 플래그
            animator.SetTrigger("doJumping");

            if(jumpCount == 0)
            {
                doubleJump = false;
                jumpCount = 2;
            }
        }

        //캐릭터 백스텝 가만히 서있을 때만 발동  작동키는 대쉬와 백스텝 동일
        if((Input.GetAxisRaw("Horizontal") == 0) && Input.GetKeyDown(KeyCode.B) && !animator.GetBool("isJumping"))
        {          
            isBackstep = true;
            animator.SetBool("isJumping", true); //점프 플래그 그대로 차용
            animator.SetTrigger("doJumping");
            
        }

        //캐릭터 대쉬 왼쪽 오른쪽 키버튼 받았을 경우 대쉬 발동
        if (Input.GetKeyDown(KeyCode.B) && ((Input.GetAxisRaw("Horizontal") < 0)|| (Input.GetAxisRaw("Horizontal") > 0)) && !animator.GetBool("isDash"))
        {
            Dash();
            isDash = true;
            animator.SetBool("isDash", true);
            animator.SetTrigger("doDash");

        }

    }

    void FixedUpdate()
    {
        Move();
        Jump();
        BackStep();
    }

    //------------------[Movement Function]-----------------
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        
        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            //myRenderer.flipX = true; //left flip

            Ldir = true;
            Rdir = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            //myRenderer.flipX = false; // right flip

            Rdir = true;
            Ldir = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    //-------------------[Jumping Function]--------------------
    void Jump()
    {
        if (!isJumping)
            return;

        if (jumpCount > 0)
        {

            rigid.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
    }

    //--------------------[BackStep Function]--------------------
    void BackStep()
    {
        if (!isBackstep)
            return;

        rigid.velocity = Vector2.zero;

        
        if (Ldir == true)
        {
            Vector2 backstepVelocity = new Vector2(backstepPowerX, backstepPowerY);
            rigid.AddForce(backstepVelocity, ForceMode2D.Impulse);
        }
        else if(Rdir == true)
        {
            Vector2 backstepVelocity = new Vector2(-backstepPowerX, backstepPowerY);
            rigid.AddForce(backstepVelocity, ForceMode2D.Impulse);
        }

        isBackstep = false;
    }

    //--------------------[Dash Function]--------------------
    void Dash()
    {
        if (!isDash)
            return;

        rigid.velocity = Vector2.zero;

        if (Ldir == true)
        {
            Vector2 dashVelocity = new Vector2(-DashPower,0);
            rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
        }
        else if (Rdir == true)
        {
            Vector2 backstepVelocity = new Vector2(DashPower, 0);
            rigid.AddForce(backstepVelocity, ForceMode2D.Impulse);
        }

        isDash = false;
    }


    //바닥 착지 이벤트
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attach : " + other.gameObject.layer);

        if (other.gameObject.layer == 8)
        {
            animator.SetBool("isJumping", false);  //착지
            jumpCount = 2;
            doubleJump = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Dettach : " + other.gameObject.layer); 
    }
}
