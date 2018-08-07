using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour {

    public GameObject TargetObject;
    private float TakeDamage;

    public Slider HealthBar;
    public Text NameText;
    public RawImage BarAcc;

    /*
    /// 1. 레이캐스트로 클릭된 오브젝트 태그 비교  
    /// 2. 태그가 적일 경우 타겟으로 설정 
    /// 3. 타겟으로 설정된 적 오브젝트 불러옴
    /// 4. 타겟으로 설정된 적 오브젝트의 이름을 읽어와서 NameText ui에 넣어줌
    /// 5. MaxValue 는 적 최대체력 / Value 는 적 현재 체력으로 불러들어옴
    /// 6. 일시정지 버튼 하나 만들어놓고 게임마스터에 있는 PauseGame불러옴
    */



    void Update()
    {
        DetectingTarget();
        SetTargetHealth();
        SetTargetName();
        GetTargetHealth();
        MissingTargetName();
    }

    //타겟 설정 해주는 부분 레이캐스트 클릭으로 할당
    public void DetectingTarget()
    {
        if(Input.GetMouseButton(0) && Time.timeScale != 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Boss")
                {
                    TargetObject = hit.collider.gameObject;

                    HealthBar.gameObject.SetActive(true);
                    NameText.gameObject.SetActive(true);
                    BarAcc.gameObject.SetActive(true);
                }
                else
                    return;
            }
            else
            {
                HealthBar.gameObject.SetActive(false);
                NameText.gameObject.SetActive(false);
                BarAcc.gameObject.SetActive(false);
            }
        }
    }

    //최대 체력 maxValue 설정해주는 함수
    public void SetTargetHealth()
    {
        if (TargetObject == null)
            return;
        HealthBar.maxValue = TargetObject.GetComponentInParent<EnemyController>().MaxHealth;     
    }
    //현재 체력 Value 갱신 해주는 함수
    public void GetTargetHealth()
    {
        if (TargetObject == null)
            return;
        HealthBar.value = TargetObject.GetComponentInParent<EnemyController>().CurrentHealth;
        TakeDamage = HealthBar.maxValue - HealthBar.value; //왜 구했지
    }
    //몬스터 타겟 이름 보여주는 함수
    public void SetTargetName()
    {
        if (TargetObject == null)
            return;
        NameText.text = TargetObject.GetComponentInParent<EnemyController>().Name;
    }
    public void MissingTargetName()
    {
        if(TargetObject == null)
        {
            HealthBar.gameObject.SetActive(false);
            NameText.gameObject.SetActive(false);
            BarAcc.gameObject.SetActive(false);
        }
    }
}
