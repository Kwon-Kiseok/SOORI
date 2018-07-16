using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour {

    public int damage = 1;
    public bool playHitReaction = false;

    private PlayerController playerObj;

    void Awake()
    {
        playerObj = GetComponent<PlayerController>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerObj.TakeDamage(this.damage, this.playHitReaction);
        }
    }
}
