using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyMove : MonoBehaviour
{
    // 이동 속도
    public float speed = 10f;

    // 타겟
    public GameObject target;

    // 현재 시간
    public float currTime = 0;
    // 최대 거리 위치, 되돌아올 시간
    float stopTime = 0.5f;
    // 가는중
    public bool isGoing = false;

    // 가는힘
    public bool power = false;

    // 최대 이동 거리
    float maxDistance = 8f;

    // 캐피 원점
    public Transform cappyPos;

    // 이동 방향
    Vector3 dir;

    // 카메라 스크립트
    public Transform mainCamera;

    private void OnEnable()
    {
        //cappyPos.rotation = mainCamera.rotation;
        //앞 방향을 정한다
        dir = target.transform.forward;
        dir.Normalize();

        isGoing = true;

        power = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Capture();
    }

    void Capture()
    {

        if (isGoing == true)
        {
            print("가는중");

            transform.position += dir * speed * Time.deltaTime; // 앞으로 1번만 이동함, 멈춤

            //만약 현재 z값이 originZ + maxDistace 보다 같거나 커질때
            if (transform.position.z >= cappyPos.position.z + maxDistance)
            {
                // 잠시 멈춘다
                speed = 0;
                // 시간을 흐르게 한다
                currTime += Time.deltaTime;
                if (currTime >= stopTime)
                {
                    // 오는 중임
                    isGoing = false;
                }

            }
        }

        else
        {
            print("오는중");
            // 돌아올 방향을 정한다
            dir = target.transform.position - this.transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            dir.Normalize();
            // 다시 반대로 돌아간다
            // 스피드 원래대로
            speed = 10f;
            //rig.AddForce(dir * speed);
            transform.position += dir * speed * Time.deltaTime;

            if (transform.position.z <= cappyPos.position.z)
            {
                print("끝");
                // cappy를 끈다
                this.gameObject.SetActive(false);

            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);

        }
    }
}
