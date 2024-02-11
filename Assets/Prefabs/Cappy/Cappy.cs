using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cappy : MonoBehaviour
{
    Rigidbody rig;

    // ȸ�� �ӵ�
    float rotSpeed = 1500f;

    // �̵� �ӵ�
    public float speed = 10f;

    // Ÿ��
    public GameObject target;

    // ���� �ð�
    public float currTime = 0;
    // �ִ� �Ÿ� ��ġ, �ǵ��ƿ� �ð�
    float stopTime = 0.5f;


    // ������
    public bool isGoing = false;

    // ������
    public bool power = false;

    // �ִ� �̵� �Ÿ�
    float maxDistance = 5f;

    // ĳ�� ����
    float originZ = 0;

    // �̵� ����
    Vector3 dir;


    private void OnEnable()
    {
        // ó�� ĳ���� ��ġ�� ����
        originZ = transform.position.z;

        //�� ������ ���Ѵ�
        dir = target.transform.forward;
        dir.Normalize();

        isGoing = true;

        power = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //������ٵ� ��������
        //rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Capture();
        //ȸ���Ѵ�
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }

    void Capture()
    {
        // ����ڰ� fire1��ư�� ������(���������� ȣ��)
        // ĸ�Ļ�����
        //isCapture = true;
        // ĳ�Ǹ� �Ҵ�(����������)
        //this.gameObject.SetActive(true);


        if (isGoing == true)
        {
            print("������");

            //������ ������ �Ÿ���ŭ �̵��Ѵ�
            //rig.velocity = (dir * speed); // ������ ��� ���µ�, �ȸ���
            //if (power == true)
            //{
            //    print("power");

            //    rig.AddForce(dir * 100, ForceMode.Impulse);
            //    power = false;
            //}
            transform.position += dir * speed * Time.deltaTime; // ������ 1���� �̵���, ����

            //���� ���� z���� originZ + maxDistace ���� ���ų� Ŀ����
            if (transform.position.z >= originZ + maxDistance)
            {
                // ��� �����
                speed = 0;
                // �ð��� �帣�� �Ѵ�
                currTime += Time.deltaTime;
                if (currTime >= stopTime)
                {
                    // ���� ����
                    isGoing = false;
                }

            }
        }

        else
        {
            print("������");
            // ���ƿ� ������ ���Ѵ�
            dir = target.transform.position - this.transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            dir.Normalize();
            // �ٽ� �ݴ�� ���ư���
            // ���ǵ� �������
            speed = 10f;
            //rig.AddForce(dir * speed);
            transform.position += dir * speed * Time.deltaTime;

            if (transform.position.z <= originZ)
            {
                print("��");
                // cappy�� ����
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
