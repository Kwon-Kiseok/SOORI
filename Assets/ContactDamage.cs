using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour {

    public int damage;
    public bool playHitReaction = false;
    public bool playerHit = false; //중첩충돌 방지용

    private GameObject playerObj;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && playerHit == false)
        {
            playerObj.GetComponent<PlayerController>().TakeDamage(this.damage, this.playHitReaction);
            playerHit = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && playerHit == true)
            playerHit = false;
    }
}
