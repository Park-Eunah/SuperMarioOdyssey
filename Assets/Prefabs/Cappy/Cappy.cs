using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cappy : MonoBehaviour
{
    Rigidbody rig;

    // 회전 속도
    float rotSpeed = 1500f;

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
    float maxDistance = 5f;

    // 캐피 원점
    float originZ = 0;

    // 이동 방향
    Vector3 dir;


    private void OnEnable()
    {
        // 처음 캐피의 위치를 저장
        originZ = transform.position.z;

        //앞 방향을 정한다
        dir = target.transform.forward;
        dir.Normalize();

        isGoing = true;

        power = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //리지드바디를 가져오자
        //rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Capture();
        //회전한다
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }

    void Capture()
    {
        // 사용자가 fire1버튼을 누르면(마리오에서 호출)
        // 캡쳐상태임
        //isCapture = true;
        // 캐피를 켠다(마리오에서)
        //this.gameObject.SetActive(true);


        if (isGoing == true)
        {
            print("가는중");

            //앞으로 정해진 거리만큼 이동한다
            //rig.velocity = (dir * speed); // 앞으로 계속 가는데, 안멈춤
            //if (power == true)
            //{
            //    print("power");

            //    rig.AddForce(dir * 100, ForceMode.Impulse);
            //    power = false;
            //}
            transform.position += dir * speed * Time.deltaTime; // 앞으로 1번만 이동함, 멈춤

            //만약 현재 z값이 originZ + maxDistace 보다 같거나 커질때
            if (transform.position.z >= originZ + maxDistance)
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

            if (transform.position.z <= originZ)
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
