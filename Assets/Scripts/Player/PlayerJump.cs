using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Collider co;
    Rigidbody rig;

    // 스피드 (합칠때 삭제하기
    public float speed = 5f;
    // 점프파워
    public float jumpPower = 8;

    // 최대 점프 횟수
    public int jumpMaxCount = 3;
    // 현재 점프 횟수
    public int jumpCurrCount = 0;

    // 현재 점프 시간
    public float jumpCurrTime = 0;
    // 점프 최대 시간
    // float jumpMaxTime = 2f;
    
    // 점프 상태 확인
    public bool isJumping = false;

    // 점프 방향
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        //콜라이더를 가져오자
        co = GetComponent<Collider>();

        //리지드바디를 가져오자
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 점프 하는 중인지 확인한다
        // = isjumping = false일때
        // 만약 플레이어가 점프하는 중이 아니라면
        if (isJumping == false)
        {
            // 점프 시간을 잰다
            jumpCurrTime += Time.deltaTime;


            // 사용자가 스페이스 바를 눌렀을때 (만약 사용자가 스페이스 바를 누른다면)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 점프 횟수를 센다
                jumpCurrCount++;
                print("is jumping");
                // 점프 중으로 바꾼다
                isJumping = true;

                // 1번 점프
                // 만약 점프 횟수가 1이라면
                if (jumpCurrCount == 1)
                {
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 1번 점프 = 기본 점프
                    print("jump1");
                    // 점프할 방향을 정한다
                    dir = transform.up;
                    // 1번 점프를 한다
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);

                }

                // 2번 점프 
                // 만약 1번 점프= count = 1 후 , n초가 지났을때,
                else if (jumpCurrCount == 2 && jumpCurrTime <= 1)
                {
                    // 스페이스 바를 누르면
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 2번 점프를 한다
                    print("jump2");
                    // 점프할 방향을 정한다
                    dir = transform.up;
                    // 2번 점프를 한다
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);
                    // 2번 점프 = 애니메이션이 바뀜

                }

                // 3번 점프
                // 만약 2번 점프 후 n초가 지났을때,
                else if (jumpCurrCount == 3 && jumpCurrTime <= 1)
                {
                    // 스페이스 바를 누르면
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 3번 점프를 한다
                    print("jump3");
                    
                    // 점프할 방향을 정한다
                    dir = transform.up;
                    // 스피드를 빠르게한다
                    speed = 10f;
                    // 3번 점프를 한다
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);

                }


            }

            else if (jumpCurrTime >= 1)
            {
                // 점프 횟수를 초기화 한다
                jumpCurrCount = 0;
            }

        }

        // 만약 플레이어가 점프중 일 때
        else if (isJumping == true)
        {
            // 키보드 왼쪽 컨트롤 키를 누른다면
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                // 현재 높이에서 멈춘다

                // 구르기 애니메이션을 실행한다

                // 땅으로 빠르게 떨어진다

            }
        }
               

    }
    // 땅에 닿으면 점프 횟수를 초기화 한다
    private void OnCollisionEnter(Collision collision)
    {
        // 만약 플레이어가 땅에 닿아있다면
        if(collision.gameObject.CompareTag("Ground"))
        {
            print("Ground");
            isJumping = false;
            speed = 5f;

            // 땅에 닿았을 때, 점프 현재 횟수가 최대 횟수보다 크다면,
            if (jumpCurrCount >= jumpMaxCount)
            {
                // 점프 횟수를 초기화 한다
                jumpCurrCount = 0;
            }

        }
    }

}
