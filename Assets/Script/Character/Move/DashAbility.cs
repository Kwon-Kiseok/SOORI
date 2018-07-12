using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public int direction;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashTime = startDashTime;
    }
    void Update()
    {
        //-------[dash Function]
        //1일 경우 왼쪽 2일 경우 오른쪽
        //방향키를 누른 상태 + 왼쪽 쉬프트 한번 클릭으로 대쉬 
        if (direction == 0)
        {
            if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                direction = 1;
                animator.SetTrigger("doDash");
            }
            else if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                direction = 2;
                animator.SetTrigger("doDash");
            }
        }
        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                
                if(direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if(direction == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
            }
            
        }
    }
}