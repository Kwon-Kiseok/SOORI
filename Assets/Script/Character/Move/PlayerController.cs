using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //--Move elements
    public float movePower = 1f;

    //--Jump elements
    public float jumpPower = 1f;
    public int jumpCount = 2;
    bool isJumping = false;

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
        jumpCount = 0; //시작부터 점프하는 것 방지
    }

    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();     

        dashTime = startDashTime;
        
	}

	void Update () {
        
        //가만히 서있을 경우
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isMoving", false);
        }
        //왼쪽 이동
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool("isMoving", true);
        }
        //오른쪽 이동
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("isMoving",true); 
        }
        //점프
        if (jumpCount > 0)
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

    void FixedUpdate()
    {
        Move();
        Dash();
        Jump();
    }

    void LateUpdate()
    {
        if (dashCoolTime > 0)
            dashCoolTime -= Time.smoothDeltaTime;
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


    //-------[Landing Function]---------------
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attach : " + other.gameObject.layer);
        if(other.gameObject.layer == 8 && rigid.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            jumpCount = 2;
        }
    }
    void OnTriggerExit2D(Collider2D other)
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
                    direction = 1;
                    animator.SetBool("isDash", true);
                }
                else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    direction = 2;
                    animator.SetBool("isDash", true);             
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
