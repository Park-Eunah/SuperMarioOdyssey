using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    // 굼바의 상태 (순찰, 추적)
    private enum State
    {
        Patrol,
        Chase,
        Attack
    }

    private State state = State.Patrol;

    private float speed = 0f;                     //현재 속력
    private float walkSpeed = 3f;                 //걸을 때 속력(자동 이동)
    private float runSpeed = 8f;                  //뛸 때 속력(플레이어를 향해 이동)
    private float responseDistance = 20f;         //반응거리
    private float distanceFromPlayer = 0f;        //플레이어와의 거리 
    private float rotateSpeed = 10f;              //회전속도
    private float curTime = 0f;                   //현재 시간
    private float autoMoveTime = 4f;              //자동 이동 시간
    private float autoMovePeriod = 3f;            //자동 이동 주기
    private float attackTime = 2f;                //공격시간(플레이어와 닿았을 때 멈춰있는 시간
    private float jumpPower = 5f;                 //플레이어를 발견하면 점프할 힘

    private bool isNearbyPlayer = false;          //플레이어가 반응거리 안에 있는지 여부
    private bool isJumped = false;                //플레이어를 발견하고 점프를 했는지 확인
    private bool isAttached = false;              //플레이어와 닿아있는지 확인
    private bool isAutoMoving = false;            //자동 이동중인지 확인
    private bool isRotateTiming = false;          //회전 타이밍인지 확인

    private Vector3 dir = Vector3.zero;           //이동할 방향

    private Transform playerTransform;            //플레이어의 Transform 컴포넌트

    private Rigidbody rig;

    private Animator anim;                        //애니메이터

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        //씬에서 Player라는 태그를 가진 게임오보젝트의 transform을 playerTransform에 넣어줌
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CheckDistance();
        AutoRotate();
        SetAnimation();
        //if (isNearbyPlayer)
        //{
        //    if (isAttached)
        //    {
        //        Attack();
        //    }
        //    else if(isRun)
        //    {
        //        MoveToPlayer();
        //    }
        //}
        //else
        //{
        //    if (isAttached == false)
        //    {
        //        AutoMoving();
        //    }
        //}

        print(gameObject.name +": "+ state);
        switch (state)
        {
            case State.Patrol:
                AutoMoving();
                break;

            case State.Chase:
                MoveToPlayer();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    // 6걸음씩 랜덤한 방향으로 움직이기(플레이어가 반응거리 안에 없을 때)
    private void AutoMoving()
    {
        speed = walkSpeed;
        curTime += Time.deltaTime;

        if(!isAutoMoving && curTime >= autoMovePeriod)
        {
            curTime = 0f;
            //dir = 
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
            //transform.Rotate(0, 90, 0);
            isRotateTiming = true;
            isAutoMoving = true;
        }
        else if(isAutoMoving && curTime >= autoMoveTime)
        {
            curTime = 0f;
            isAutoMoving = false;
        }

        if (isAutoMoving)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    // 플레이어를 향해 움직이기(플레이어가 반응거리 안에 있을 때)
    private void MoveToPlayer()
    {
        //걷는 애니메이션 실행
        if(speed < runSpeed)
        {
            speed += Time.deltaTime * 10;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //플레이어를 향해 돌아봄
    private void AutoRotate()
    {
        if (state != State.Patrol)
        {
            dir = playerTransform.position - transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
        else
        {
            if (isRotateTiming)
            {
                dir = new Vector3(0, transform.rotation.eulerAngles.y + 90f, 0);
                isRotateTiming = false;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(dir), Time.deltaTime * rotateSpeed);
        }
        //dir = tr.position - transform.position;
    }
        
    //플레이어를 공격(플레이어 앞에 갔을 때)
    private void Attack()
    {
        curTime += Time.deltaTime;

        if (curTime >= attackTime)
        {
            curTime = 0f;
            GameManager.instance.LifeCount(false);
            //isAttached = false;
        }        
    }

    //플레이어가 반응거리 안에 들어왔는지 체크
    private void CheckDistance()
    {
        //플레이어와의 거리를 계산
        distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //플레이어와의 거리가 반응거리보다 짧거나 같으면 상태변경
        if (distanceFromPlayer <= responseDistance)
        {
            if (state != State.Patrol)
                return;

            isNearbyPlayer = true;

            curTime = 0f;

            if (!isJumped)
            {
                speed = 0f;
                
                // 굼바가 이동중일 때만 점프를 함(점프 후 땅에 닿으면 상태 전환)
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                anim.SetTrigger("jump");
                isJumped = true;
            }
        }
        else 
        {
            if (state == State.Patrol)
                return;

            isJumped = false;
            curTime = 0f;

            state = State.Patrol;
        }
    }

    private void Die() 
    {
        anim.SetBool("isDie", true);
    }

    private void SetActiveFalse()
    {
        GameManager.instance.ActiveGetCoin(transform, 1);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!isAttached)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        print(collision.contacts[0].point.y + ", " + transform.position.y + 2f); //그냥 닿아도 죽어서 굼바 위치에 +2f 해서 계산.
        //        if (collision.contacts[0].point.y > transform.position.y + 2f) 
        //        {
        //            Die();
        //        }
        //        else
        //        {
        //            GameManager.instance.LifeCount(false);
        //            isAttached = true;
        //        }

        //    }
        //    else if (collision.gameObject.CompareTag("Ground"))
        //    {
        //        if (isNearbyPlayer)
        //        {
        //            if (isJumped)
        //            {
        //                print("isrun");
        //                //isRun = true;

        //                state = State.Chase;
        //            }
        //        }
        //    }
        //}

        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 위에서 밟으면 죽음
            if (collision.contacts[0].point.y > transform.position.y + 2f)
            {
                Die();
            }

            // 아니면 플레이어 hp감소, 공격으로 상태 전환
            else
            {
                GameManager.instance.LifeCount(false);
                state = State.Attack;
                //isAttached = true;
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            if (isNearbyPlayer && isJumped)
            {
                state = State.Chase;
            }
        }
    }

    private void SetAnimation()
    {
        anim.SetBool("isWalk", state == State.Patrol && isAutoMoving);
        anim.SetBool("isRun", state == State.Chase);

        //anim.SetBool("isWalk", isAutoMoving);
        //anim.SetBool("isFind", isNearbyPlayer);

        //if(isNearbyPlayer && !isAttached)
        //{
        //    anim.SetBool("isRun", true);
        //}
        //else
        //{
        //    anim.SetBool("isRun", false);
        //}
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        isAttached = true;
    //        print("isAttached : " + isAttached);
    //    }
    //}
}
