using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public GameObject UnDamagedState;
    public GameObject DamagedState;

    void Awake()
    {
        UnDamagedState.GetComponent<PlayerController>();
        DamagedState.GetComponent<PlayerController>();
        UnDamagedState.SetActive(true);
        DamagedState.SetActive(false);

    }
    void Update()
    {
        if(DamagedState.GetComponent<PlayerController>().Health == 2 && DamagedState.activeSelf && DamagedState.GetComponent<PlayerController>().isImmune == true)
        {
            UnDamagedState.transform.position = DamagedState.transform.position;
           
            
            //변환동안 시간 줘야함
            UnDamagedState.SetActive(true);
            DamagedState.SetActive(false);
        }
        if (UnDamagedState.GetComponent<PlayerController>().Health == 1 && UnDamagedState.activeSelf &&UnDamagedState.GetComponent<PlayerController>().isImmune == false)
        {
            DamagedState.transform.position = UnDamagedState.transform.position;

            //변환동안 시간 줘야함 - > 플레이어 바로 죽어버림
            UnDamagedState.SetActive(false);

            DamagedState.SetActive(true);
        }

        
    }

}
