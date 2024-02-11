using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyMove : MonoBehaviour
{
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
    float maxDistance = 8f;

    // ĳ�� ����
    public Transform cappyPos;

    // �̵� ����
    Vector3 dir;

    // ī�޶� ��ũ��Ʈ
    public Transform mainCamera;

    private void OnEnable()
    {
        //cappyPos.rotation = mainCamera.rotation;
        //�� ������ ���Ѵ�
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
            print("������");

            transform.position += dir * speed * Time.deltaTime; // ������ 1���� �̵���, ����

            //���� ���� z���� originZ + maxDistace ���� ���ų� Ŀ����
            if (transform.position.z >= cappyPos.position.z + maxDistance)
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

            if (transform.position.z <= cappyPos.position.z)
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
