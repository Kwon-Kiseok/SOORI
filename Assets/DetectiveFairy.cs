using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveFairy : MonoBehaviour {

    public GameObject jem;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            jem.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            jem.gameObject.SetActive(false);
        }
    }
}
