using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour {

    public int damage = 1;
    public bool playHitReaction = false;

    private GameObject playerObj;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerObj.GetComponent<PlayerController>().TakeDamage(this.damage, this.playHitReaction);
        }
    }
}
