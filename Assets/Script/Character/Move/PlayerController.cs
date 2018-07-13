﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //--Move elements
    public float movePower = 1f;
    private bool Rdir;

    //--Jump elements
    public float jumpPower = 1f;
    public int jumpCount = 2;
    bool isJumping = false;

    //--BackJump elements
    public float backjumpX = 1f;
    public float backjumpY = 1f;
    bool isBackjump = false;
    private float backjumpCoolTime;

    //--Dash elements
    public float dashSpeed;
    public float startDashTime;
    public int direction;
    private float dashCoolTime;
    private float dashTime;

    //Components
    Rigidbody2D rigid;
    Animator animator;
    Vector3 movement;



    void Awake()
    {
        dashCoolTime = 0.01f; //대쉬 쿨타임
        backjumpCoolTime = 0.01f; //백점프 쿨타임
        jumpCount = 0; //시작부터 점프하는 것 방지
        Rdir = true;

    }

    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();     

        dashTime = startDashTime;
        
	}

	void Update () {

        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);

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
                if (Input.GetButtonDown("Jump"))
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
            if (animator.GetBool("isJumping") || animatorState.IsName("SONIC_DASH") || animator.GetBool("isBackJump"))
                return;
            isBackjump = true;
            animator.SetBool("isBackJump", true);
            BackJump();
        }        
	}

    void FixedUpdate()
    {
        //백점프 중에는 행동 불가
        if (!animator.GetBool("isBackJump"))
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
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }
    //-------[BackJump Function]------------
    void BackJump()
    {
        if (!isBackjump)
            return;

        rigid.velocity = Vector2.zero;

        if(Rdir == true && animator.GetBool("isBackJump"))
        {
            Vector2 backjumpVelocity = new Vector2(-backjumpX, backjumpY);
            rigid.AddForce(backjumpVelocity, ForceMode2D.Impulse);
        }
        else if(Rdir == false && animator.GetBool("isBackJump"))
        {
            Vector2 backjumpVelocity = new Vector2(backjumpX, backjumpY);
            rigid.AddForce(backjumpVelocity, ForceMode2D.Impulse);
        }

        backjumpCoolTime = 2f;
        isBackjump = false;
        
    }

    //-------[Landing Function]---------------
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attach : " + other.gameObject.layer);
        if(other.gameObject.layer == 8)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isBackJump", false);
            jumpCount = 2;
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Detach : " + other.gameObject.layer);
    }


    //-------[Dash Function]---------------
    void Dash()
    {
        if (dashCoolTime > 0)
            return;

            if (direction == 0)
            {
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetBool("isDash", true);
                direction = 1;
                }
                else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetBool("isDash", true);
                direction = 2;
            }
            } else
            {
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                    rigid.velocity = Vector2.zero;
                    dashCoolTime = 2f;            //대쉬 쿨타임 
                    animator.SetBool("isDash", false);
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

}
