using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour {

    public float speed = 1f;
    private bool isRight = true;
    private Rigidbody2D rigid = null;
    public PlayerController playerObj = null;
	// Use this for initialization
	void Start () {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);

        if(playerObj.Rdir == true)
        {
            isRight = true;
        }
        else if(playerObj.Rdir == false)
        {
            isRight = false;
        }
	}
    void Update()
    {
        float moveX = speed * Time.deltaTime;

        if (isRight == true)
        {
            transform.Translate(moveX, 0, 0);
        }
        else if (isRight == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate(-moveX, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //플랫폼에 부딪힐 경우 화살 사라짐
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

}
