using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    // ������ ���� (����, ����)
    private enum State
    {
        Patrol,
        Chase,
        Attack
    }

    private State state = State.Patrol;

    private float speed = 0f;                     //���� �ӷ�
    private float walkSpeed = 3f;                 //���� �� �ӷ�(�ڵ� �̵�)
    private float runSpeed = 8f;                  //�� �� �ӷ�(�÷��̾ ���� �̵�)
    private float responseDistance = 20f;         //�����Ÿ�
    private float distanceFromPlayer = 0f;        //�÷��̾���� �Ÿ� 
    private float rotateSpeed = 10f;              //ȸ���ӵ�
    private float curTime = 0f;                   //���� �ð�
    private float autoMoveTime = 4f;              //�ڵ� �̵� �ð�
    private float autoMovePeriod = 3f;            //�ڵ� �̵� �ֱ�
    private float attackTime = 2f;                //���ݽð�(�÷��̾�� ����� �� �����ִ� �ð�
    private float jumpPower = 5f;                 //�÷��̾ �߰��ϸ� ������ ��

    private bool isNearbyPlayer = false;          //�÷��̾ �����Ÿ� �ȿ� �ִ��� ����
    private bool isJumped = false;                //�÷��̾ �߰��ϰ� ������ �ߴ��� Ȯ��
    private bool isAttached = false;              //�÷��̾�� ����ִ��� Ȯ��
    private bool isAutoMoving = false;            //�ڵ� �̵������� Ȯ��
    private bool isRotateTiming = false;          //ȸ�� Ÿ�̹����� Ȯ��

    private Vector3 dir = Vector3.zero;           //�̵��� ����

    private Transform playerTransform;            //�÷��̾��� Transform ������Ʈ

    private Rigidbody rig;

    private Animator anim;                        //�ִϸ�����

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        //������ Player��� �±׸� ���� ���ӿ�����Ʈ�� transform�� playerTransform�� �־���
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

    // 6������ ������ �������� �����̱�(�÷��̾ �����Ÿ� �ȿ� ���� ��)
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

    // �÷��̾ ���� �����̱�(�÷��̾ �����Ÿ� �ȿ� ���� ��)
    private void MoveToPlayer()
    {
        //�ȴ� �ִϸ��̼� ����
        if(speed < runSpeed)
        {
            speed += Time.deltaTime * 10;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //�÷��̾ ���� ���ƺ�
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
        
    //�÷��̾ ����(�÷��̾� �տ� ���� ��)
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

    //�÷��̾ �����Ÿ� �ȿ� ���Դ��� üũ
    private void CheckDistance()
    {
        //�÷��̾���� �Ÿ��� ���
        distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //�÷��̾���� �Ÿ��� �����Ÿ����� ª�ų� ������ ���º���
        if (distanceFromPlayer <= responseDistance)
        {
            if (state != State.Patrol)
                return;

            isNearbyPlayer = true;

            curTime = 0f;

            if (!isJumped)
            {
                speed = 0f;
                
                // ���ٰ� �̵����� ���� ������ ��(���� �� ���� ������ ���� ��ȯ)
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
        //        print(collision.contacts[0].point.y + ", " + transform.position.y + 2f); //�׳� ��Ƶ� �׾ ���� ��ġ�� +2f �ؼ� ���.
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
            // �÷��̾ ������ ������ ����
            if (collision.contacts[0].point.y > transform.position.y + 2f)
            {
                Die();
            }

            // �ƴϸ� �÷��̾� hp����, �������� ���� ��ȯ
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
