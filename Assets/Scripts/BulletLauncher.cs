using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public GameObject bullet;                                           //불렛 프리펩

    public Transform spawnPoint;　　　　　　　　　　　　　　　　　　　　//불렛이 생성될 위치
    public Transform playerTransform;                                   //플레이어의 Transform컴포넌트

    private Queue<GameObject> bulletPool = new Queue<GameObject>();     //불렛을 여러개 담아 사용할 오브젝트 풀

    private float reactDistance = 80f;                                  //반응거리
    private float distance = 0f;                                        //플레이어와의 거리
    private float spawnTime = 3f;                                       //불렛을 생성할 주기
    private float afterSpawnTime = 0f;                                  //불렛을 생성한 후 시간

    private int poolCount = 6;                                          //오브젝트 풀에 담아줄 오브젝트 수

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

    //3초마다 불렛 생성
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

    //불렛 생성
    void SpawnBullet()
    {
        GameObject b = bulletPool.Dequeue();
        b.transform.position = spawnPoint.position;
        b.transform.rotation = spawnPoint.rotation;
        b.SetActive(true);
    }

    //터진 불렛은 다시 오브젝트 풀에 넣어준다
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
