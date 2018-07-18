using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController {

    Rigidbody2D rigid2d;

    void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(this.isFacingRight == true)
        {
            rigid2d.velocity = new Vector2(maxSpeed, this.rigid2d.velocity.y);
        }
        else
        {
            this.rigid2d.velocity = new Vector2(maxSpeed * -1, this.rigid2d.velocity.y);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //벽과 부딪혔을 경우
        if(other.tag == "Wall")
        {
            Flip();
        }
        //장애물와 부딪혔을 경우
        else if(other.tag == "ObstaclePlatform")
        {
            Flip();
        }

    }
}
