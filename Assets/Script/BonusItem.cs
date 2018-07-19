using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : MonoBehaviour {

    private PlayerController playerObj = null;

	void Update () {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (playerObj.Health == 1)
                playerObj.Health = 2;
            else if (playerObj.Health == 2)
                playerObj.Health = 2;

            Destroy(gameObject);
        }
    }

}
