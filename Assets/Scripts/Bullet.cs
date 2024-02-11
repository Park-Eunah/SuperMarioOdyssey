using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform playerTransform = null;

    private float speed = 5f;
    private float turnSpeed = 90f;
    private float explodeTime = 10f;                  //�ҷ��� ������ �ð�
    private float beforeExplodeTime = 7f;             //�ҷ��� ���� �غ� �ð�
    private float afterCreateTime = 0f;               //�ҷ��� �����ǰ� ���� �ð�
    private float dot = 0f;
    private float colorChangeSpeed = 2f;
    private float redTime = 0.5f;                     //�ҷ��� ������ �� �����Ÿ� �� ���������� �ִ� �ð�
    private float curTime = 0f;                       //�ҷ��� ������ �� �����Ÿ� �� ����� �ð� ����
    private float explodeCurTime = 0f;
    private float explosionEffectTime = 0.3f;

    private bool isRight = false;
    private bool isWhite = true;
    private bool isFollowPlayer = true;

    private Vector3 dir = Vector3.zero;               //�ҷ��� ���� ����

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

    //�ҷ��� ������ ������ �ð��� ���
    void Timer()
    {
        afterCreateTime += Time.deltaTime;

        if(afterCreateTime>= beforeExplodeTime)
        {
            //���� �غ��ϱ�
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

        // �������� �÷��̾ �����ʿ� �ִ��� ���ʿ� �ִ��� Ȯ��
        dot = Vector3.Dot(dir, transform.right);

        // dot�� ����̸� �÷��̾ �����ʿ� ����
        if (dot > 0)
        {
            isRight = true;
        }
        else if (dot < 0)
        {
            isRight = false;
        }
    }

    //�÷��̾ ���� �̵�
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

    //������ �� 10�ʰ� �����ų� ��򰡿� �ε����� ����
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

    //�÷��̾����� ������ ����
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

    //�ٸ� ������Ʈ�� �ε����� ����
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