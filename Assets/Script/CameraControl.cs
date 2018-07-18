using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*
     * Margin = 카메라 움직이지 않고 플레이어가 움직일수 있는 양
     * Smooth = 플레이어를 얼마나 빨리 따라잡는지 결정함
     * maxXAndY = 카메라가 가질 수 있는 X,Y축의 최대값
     * minXAndY = 카메라가 가질 수 있는 X,Y축의 최소값
     * offsetX = 캐릭터와 카메라 사이의 X축 거리값
     * downview = 아래키 입력 시 보여지는 거리값
    */
    public float xMargin = 1;
    public float yMargin = 1;
    public float xSmooth = 4;
    public float ySmooth = 4;
    public float offsetX = 1f;
    public float downview = 1f;

    [SerializeField] private bool Rdir = true;
    [SerializeField] private bool downInput = false;

    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    private Transform player;
    private Animator animator;


    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }
    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        //캐릭터가 보는 방향 체크
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Rdir = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Rdir = false;
        }
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorState.IsName("SONIC_IDLE"))
        {
            downInput = true;
        }
        else
        {
            downInput = false;
        }

    }
    void FixedUpdate()
    {
        TrackPlayer();    
    }


    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if(CheckXMargin() == true)
        {
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        }
        if (CheckYMargin() == true)
        {
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        if (Rdir == true)
        {
            transform.position = new Vector3(targetX+offsetX, targetY, transform.position.z);
        }
        else if(Rdir == false)
        {
            transform.position = new Vector3(targetX-offsetX, targetY, transform.position.z);
        }
        //아래키 누를 때 카메라 무빙
        if(Input.GetKey(KeyCode.DownArrow) && transform.position.y > minXAndY.y && downInput == true)
        {
            if (Rdir == true)
            {
                transform.position = new Vector3(targetX + offsetX, targetY-downview, transform.position.z);
            }
            else if (Rdir == false)
            {
                transform.position = new Vector3(targetX - offsetX, targetY-downview, transform.position.z);
            }
        }
    }


}
