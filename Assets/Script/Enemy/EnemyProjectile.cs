using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public GameObject Target;
    public float speed;
    public float deleteTime;
    private bool isRight = false;
    private Rigidbody2D rigid = null;    

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();

        Destroy(gameObject, deleteTime);

        //투사체 방향
        if (rigid.velocity.x < 0)
            isRight = false;
        else if (rigid.velocity.x > 0)
            isRight = true;

        if(isRight == true)
        {
            rigid.velocity = transform.right * speed;
        }
        else if(isRight == false)
        {
            rigid.velocity = -transform.right * speed;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //땅에 부딪힐 경우 사라짐
        if(other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
