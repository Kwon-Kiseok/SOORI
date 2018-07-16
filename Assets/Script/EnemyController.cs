using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [HideInInspector]
    public bool isFacingRight = false;
    public float maxSpeed = 1.5f;

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 enemyScale = this.transform.localScale;
        enemyScale.x = enemyScale.x * -1;
        this.transform.localScale = enemyScale;
    }
}
