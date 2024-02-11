using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public GameObject bullet;                                           //�ҷ� ������

    public Transform spawnPoint;����������������������������������������//�ҷ��� ������ ��ġ
    public Transform playerTransform;                                   //�÷��̾��� Transform������Ʈ

    private Queue<GameObject> bulletPool = new Queue<GameObject>();     //�ҷ��� ������ ��� ����� ������Ʈ Ǯ

    private float reactDistance = 80f;                                  //�����Ÿ�
    private float distance = 0f;                                        //�÷��̾���� �Ÿ�
    private float spawnTime = 3f;                                       //�ҷ��� ������ �ֱ�
    private float afterSpawnTime = 0f;                                  //�ҷ��� ������ �� �ð�

    private int poolCount = 6;                                          //������Ʈ Ǯ�� ����� ������Ʈ ��

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for(int i = 0; i < poolCount; i++)
        {
            GameObject b = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation, transform);
            bulletPool.Enqueue(b);
            b.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MeasureDistance();
        //if (distance <= reactDistance)
        //{
        //    Timer();
        //}
        //else
        //    afterSpawnTime = 0f;
    }

    //3�ʸ��� �ҷ� ����
    void Timer()
    {
        afterSpawnTime += Time.deltaTime;

        if (afterSpawnTime >= spawnTime)
        {
            SpawnBullet();
            afterSpawnTime = 0f;
        }
    }

    void MeasureDistance()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
    }

    //�ҷ� ����
    void SpawnBullet()
    {
        GameObject b = bulletPool.Dequeue();
        b.transform.position = spawnPoint.position;
        b.transform.rotation = spawnPoint.rotation;
        b.SetActive(true);
    }

    //���� �ҷ��� �ٽ� ������Ʈ Ǯ�� �־��ش�
    public void EnqueueBullet(GameObject b)
    {
        bulletPool.Enqueue(b);
        b.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Timer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            afterSpawnTime = 0f;
        }
    }
}
