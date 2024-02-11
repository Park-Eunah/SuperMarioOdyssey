using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform playerTransform = null;

    private float speed = 5f;
    private float turnSpeed = 90f;
    private float explodeTime = 10f;                  //불렛이 터지는 시간
    private float beforeExplodeTime = 7f;             //불렛이 터질 준비를 시간
    private float afterCreateTime = 0f;               //불렛이 생성되고 지난 시간
    private float dot = 0f;
    private float colorChangeSpeed = 2f;
    private float redTime = 0.5f;                     //불렛이 터지기 전 깜빡거릴 때 빨간색으로 있는 시간
    private float curTime = 0f;                       //불렛이 터지기 전 깜빡거릴 때 사용할 시간 변수
    private float explodeCurTime = 0f;
    private float explosionEffectTime = 0.3f;

    private bool isRight = false;
    private bool isWhite = true;
    private bool isFollowPlayer = true;

    private Vector3 dir = Vector3.zero;               //불렛이 향할 방향

    private MainCamera cam;

    private Renderer bulletMt;
    private Collider collider;
    private Rigidbody rig;

    public ParticleSystem dieEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem explosionEffect;

    private void OnEnable()
    {
        afterCreateTime = 0f;
        fireEffect.Play();
    }

    void Start()
    {
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bulletMt = transform.GetComponentInChildren<Renderer>();
        collider = GetComponent<Collider>();
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isFollowPlayer)
        {
            Timer();
            SetDirection();
            FollowPlayer();
        }
        else
        {
            Die();
        }
        //print(bulletMt.material.color.maxColorComponent);
    }

    //불렛이 생성된 이후의 시간을 잰다
    void Timer()
    {
        afterCreateTime += Time.deltaTime;

        if(afterCreateTime>= beforeExplodeTime)
        {
            //터질 준비하기
            BeforeExplode();

            if (afterCreateTime >= explodeTime)
            {
                Explode();
            }
        }
    }

    void SetDirection()
    {
        dir = playerTransform.position - transform.position;
        dir.Normalize();

        // 내적으로 플레이어가 오른쪽에 있는지 왼쪽에 있는지 확인
        dot = Vector3.Dot(dir, transform.right);

        // dot가 양수이면 플레이어가 오른쪽에 있음
        if (dot > 0)
        {
            isRight = true;
        }
        else if (dot < 0)
        {
            isRight = false;
        }
    }

    //플레이어를 향해 이동
    void FollowPlayer()
    {
        if (isRight)
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void BeforeExplode()
    {
        if (isWhite)
        {
            bulletMt.material.color = Color.red;
            curTime += Time.deltaTime;
            if(curTime >= redTime)
            {
                isWhite = false;
            }
        }
        else
        {
            bulletMt.material.color += new Color( 0f, colorChangeSpeed, colorChangeSpeed) * Time.deltaTime; 
            if (bulletMt.material.color.g >= 1f && bulletMt.material.color.b >= 1f)
            {
                isWhite = true;
            }
        }
    }

    //생성된 후 10초가 지나거나 어딘가에 부딪히면 터짐
    private void Explode()
    {
        fireEffect.Stop();
        explosionEffect.Play();
        bulletMt.enabled = false;
        collider.enabled = false;
        //cam.CameraShake();

        explodeCurTime += Time.deltaTime;
        if (explodeCurTime >= explosionEffectTime)
        {
            explodeCurTime = 0f;
            print("explosion stop");
            ResetBullet();
            GetComponentInParent<BulletLauncher>().EnqueueBullet(gameObject);
        }
        else
            print("explosion playing");
        //GameManager.instance.LifeCount(false);
    }

    //플레이어한테 밟히면 죽음
    private void Die()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
        if(!bulletMt.enabled && dieEffect.isStopped)
        {
            ResetBullet();
            GetComponentInParent<BulletLauncher>().EnqueueBullet(gameObject);
        }

    }
    
    private void ResetBullet()
    {
        bulletMt.enabled = true;
        collider.enabled = true;
        isFollowPlayer = true;
        bulletMt.material.color = Color.white;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
    }

    //다른 오브젝트와 부딪히면 터짐
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].point.y > transform.position.y)
            {
                isFollowPlayer = false;
            }
            else
            {
                GameManager.instance.LifeCount(false);
                Explode();
            }
        }
        else
        {
            if (!isFollowPlayer)
            {
                dieEffect.Play();
                collider.enabled = false;
                bulletMt.enabled = false;
                //gameObject.SetActive(false);
            }
            else
            {
                Explode();
            }
        }
    }
}