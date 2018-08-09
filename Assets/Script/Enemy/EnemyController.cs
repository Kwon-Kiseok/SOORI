using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [HideInInspector]
    public bool isFacingRight = false;
    public float maxSpeed = 1.5f;
    

    public Transform sightStart, sightEnd;
    public bool spotted = false;

    public GameObject SurpriseMark = null;

    public float rushRate;
    public float nextRush;

    //몬스터 체력 및 이름
    public int MaxHealth;
    public int CurrentHealth;
    public string Name;

    public void Raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.red);
        spotted = Physics2D.Linecast(sightStart.position, sightEnd.position, 1<<LayerMask.NameToLayer("Player"));

        CheckRange();


            if (spotted == true && Time.time + 0.5 >= nextRush)
            {
                SurpriseMark.SetActive(true);
            }
            else
            {
                SurpriseMark.SetActive(false);
            }

    }
    void CheckRange()
    {
        if(spotted == true && gameObject.tag != "Boss")
        {
            if (SurpriseMark.activeSelf == true)
            {
                gameObject.GetComponent<EnemySlime>().rushAttack = true;
                gameObject.GetComponent<EnemySlime>().RushAttack();
            }
        }
        else if(spotted == false && gameObject.tag != "Boss")
        {
           // SurpriseMark.SetActive(false);
            gameObject.GetComponent<EnemySlime>().rushAttack = false;
        }
    }


    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 enemyScale = this.transform.localScale;
        enemyScale.x = enemyScale.x * -1;
        this.transform.localScale = enemyScale;
    }

}
