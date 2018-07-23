using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour {

    public float speed = 1f;
    private bool isRight = true;
    private Rigidbody2D rigid = null;
    public PlayerController playerObj = null;
    private GameObject[] targetObjs; 
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
        //float moveX = speed * Time.deltaTime;

        FindClosestEnemy();

        if (isRight == true)
        {
            //transform.Translate(moveX, 0, 0);
            transform.position = Vector2.MoveTowards(transform.position, FindClosestEnemy().transform.position, speed * Time.deltaTime);
        }
        else if (isRight == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //transform.Translate(-moveX, 0, 0);
            transform.position = Vector2.MoveTowards(transform.position, FindClosestEnemy().transform.position, speed * Time.deltaTime);
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

    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;

        //이 부분에서 카메라 화면 내의 적만 찾도록 변경해줘야 함
        float distance = Mathf.Infinity;

        Vector3 position = transform.position;
        foreach(GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

}
