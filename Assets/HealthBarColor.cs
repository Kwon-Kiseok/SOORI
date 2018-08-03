using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarColor : MonoBehaviour {

    void Update()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (GetComponent<Image>().fillAmount >= 0.6)
        {
            GetComponent<Image>().color = new Color32(81,220,15,255);
        }
        else if(GetComponent<Image>().fillAmount < 0.6 && GetComponent<Image>().fillAmount > 0.3)
        {
            GetComponent<Image>().color = new Color32(200,96,35,255);
        }
        else
        {
            GetComponent<Image>().color = new Color32(220,15,15,255);
        }
    }
}
