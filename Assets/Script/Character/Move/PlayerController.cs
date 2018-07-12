using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movePower = 1f;
    public float jumpPower = 1f;
    public int jumpCount = 2;

    Rigidbody2D rigid;
    Animator animator;

    Vector3 movement;
    bool isJumping = false;

	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        jumpCount = 0; //시작부터 점프하는 것 방지
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
        Jump();
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
}
