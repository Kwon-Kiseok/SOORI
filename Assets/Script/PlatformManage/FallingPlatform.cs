using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

    /* 
     * spawnPosition = 재생성 되는 위치
     * platform = 떨어지는 플랫폼 게임오브젝트
     * originalPos = 플랫폼이 흔들릴때 고정좌표 위치
     * shakeAmt = 플랫폼이 흔들리는 정도값
     * fallDelay = 떨어지기까지 시간
     * spawnDelay = 재생성 되기까지 시간
     * shaking = 흔들리지 여부
     */
    //public Transform tempTransform;
    public GameObject spawnPosition;
    public GameObject platform;

    private Rigidbody2D rigid2d;
    private Vector3 originalPos;
    public float fallDelay;
    public float spawnDelay;
    public float shakeAmt;

    private bool shaking = false;

    void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }
    private void Update()
    {
        if(shaking)
        {
            Vector3 newPos = originalPos + Random.insideUnitSphere * (Time.deltaTime * shakeAmt);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Shake());
            StartCoroutine(Fall());
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rigid2d.isKinematic = false;
        GetComponent<Collider2D>().isTrigger = true;
        
        yield return 0;
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        GetComponent<Collider2D>().isTrigger = false;
        rigid2d.isKinematic = true;
        GameObject copy = (GameObject) Instantiate(platform, spawnPosition.transform.position, spawnPosition.transform.rotation);

        Destroy(platform);
        
    }
    IEnumerator Shake()
    {        
        if (shaking == false)
            shaking = true;

        yield return new WaitForSeconds(1f);
        shaking = false;
        transform.position = originalPos;
    }

    /*
     * 발판 위에 올라왔을 때만 떨어지게 변경해야함 
     */
}
