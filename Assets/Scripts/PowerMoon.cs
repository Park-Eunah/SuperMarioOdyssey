using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMoon : MonoBehaviour
{
    // Discovery() ���� ���Ǵ� ������
    private float speed = 5f;
    private float jumpSpeed = 120f;
    private float rotateSpeed = 2000f;
    private float jumpY = 10f;
    private float maxY = 100f;
    private float origiinY = 0f;

    // ���¸� üũ�ϴ� ����
    private bool isUp = true;
    private bool isJump = false;

    // �ð� üũ�� ����
    private float curTime = 0f;
    private float endTime = 2f;

    // ��Ȳ�� üũ�� ����(�߰ߵ� ���� �����ΰ���)
    private bool isEnding = false;

    // ī�޶� �Ŀ����� �ٶ���� �ϴ� ��Ȳ���� Ȯ��
    private bool isWaiting = false;


    // EndingPose()���� ���Ǵ� ������
    private float endingposY = 3f;
    public Transform endTransform;
    public FollowCamera follow;
    public MainCamera maincam;
    public Transform camera;
    public AudioClip endingBGM;
    public AudioSource audioSource;

    private void Start()
    {
        origiinY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnding)
        {
            EngingPose();
        }
        else
        {
            Discovery();
        }
    }

    public bool ISWAITING
    {
        get { return isWaiting; }
    }

    public bool ISENDING
    {
        get { return isEnding; }
    }

    private void Discovery()
    {
        //maincam.enabled = false;
        //follow.enabled = true;
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        if (isUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime + Vector3.right * speed * 0.05f * Time.deltaTime, Space.World);
            if (transform.position.y <= origiinY)
            {
                speed = 0f;
                rotateSpeed--;
                isWaiting = true;

                if (rotateSpeed <= 90f)
                {
                    rotateSpeed = 90f;
                }
            }
        }

        if (transform.position.y >= origiinY + jumpY)
        {
            speed = jumpSpeed;
            isJump = true;
        }

        if (isJump)
        {
            if (transform.position.y >= origiinY + maxY)
            {
                isUp = !isUp;
            }
        }
    }

    private void EngingPose()
    {
        print("endingpose");
        rotateSpeed = 1440f;
        speed = 30f;
        curTime += Time.unscaledDeltaTime;
        //print(curTime);
        if (curTime <= endTime)
        {
            transform.position = endTransform.position;
            transform.Rotate(0, rotateSpeed * Time.unscaledDeltaTime, 0);
        }
        else
        {
            transform.LookAt(camera);
            if(transform.position.y <= endTransform.position.y + endingposY)
            {
                transform.Translate(Vector3.up * speed * Time.unscaledDeltaTime);
                print(curTime);
            }
            else if (curTime >= endTime + 2f)
            {
                UIManager.instance.EndingCredit();
            }

            if (curTime >= 3f)
            {
                //UIManager.instance.EndingCredit(); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //You Got A Moon ����
            Time.timeScale = 0;

            //������ MoonJump ȣ��
            audioSource.clip = endingBGM;
            audioSource.Play();


            isEnding = true;
            print("trigger ");
            UIManager.instance.EndingUI();
        }
    }
}
