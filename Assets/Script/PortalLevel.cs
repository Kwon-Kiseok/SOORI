using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLevel : MonoBehaviour {

    private bool playerInPortal;
    GameObject gm;

	// Use this for initialization
	void Start () {
        playerInPortal = false;
        gm = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E) && playerInPortal == true)
        {
            gm.GetComponent<GameMaster>().EndLevel();
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInPortal = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInPortal = false;
        }
    }
}
