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

    public void Raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.red);
        spotted = Physics2D.Linecast(sightStart.position, sightEnd.position, 1<<LayerMask.NameToLayer("Player"));

        CheckRange();
    }
    void CheckRange()
    {
        if(spotted == true)
        {
            SurpriseMark.SetActive(true);
            gameObject.GetComponent<EnemySlime>().rushAttack = true;
            gameObject.GetComponent<EnemySlime>().RushAttack();
        }
        else if(spotted == false)
        {
            SurpriseMark.SetActive(false);
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
