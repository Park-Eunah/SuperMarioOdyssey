using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    // ������ Ÿ��
    public Transform target;
    // �÷��̾�
    public GameObject player;

    // ������ ��
    private float xRotateMove, yRotateMove;
    // ȸ�� ���ǵ�
    public float rotateSpeed = 500.0f;
    // ī�޶�� Ÿ���� �Ÿ�
    public float dist = 20f;
    // �ε巴�� �̵��ϴ� �ð�?
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;

    // ��
    public Transform moon;
    // �� ��ġ
    // �� ��ũ��Ʈ
    private PowerMoon powermoon;

    // ī�޶� ����
    public float shakeTime = 0.5f;
    public float shakeSpeed = 2f;
    public float shakeAmount = 1f;

    // ó�� ��ġ ��
    public float startY;

    // ��ȭ�� ��ġ ��
    public float deltaY;

    // y �� ������
    public bool isTargetUp = false;

    // ����
    public Transform cameraStartPos;

    // ���� ��Ÿ���� ���� ���� �ϴ� ���� ���󰬴ٰ�
    // �ٽ� ���ڸ��� ���� �÷��̾ �ٽ� ���󰡰� �ʹ�

    // Start is called before the first frame update
    void Start()
    {
        powermoon = moon.GetComponent<PowerMoon>();

        // �����Ҷ� �÷��̾��� y ���� ����
        startY = target.transform.position.y;

        // ī�޶��� ��ġ�� ó�� ��ġ��
        transform.position = cameraStartPos.position;
        transform.rotation = cameraStartPos.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �ִٸ� ���� Ÿ������ �ٲ۴�
        if (moon.gameObject.activeSelf && !powermoon.ISWAITING)
        {
            transform.LookAt(moon);
        }
        // �÷��̾ ������ �Է°� �ȹ���
        else if (player.activeSelf)
        {
            // ���콺 �Է��� �޾ƿ´� (x = ���콺 �¿�, y=���콺 ���Ʒ� ������)
            xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;

            
            // �ƴϸ� Ÿ���� �÷��̾�� �ٲ۴�
            //if()
            //{
                target = player.transform;
            //}

            // �� �������� ȸ���Ѵ�
            transform.RotateAround(target.position, Vector3.right, yRotateMove);
            transform.RotateAround(target.position, Vector3.up, xRotateMove);

            // ī�޶� �÷��̾�� �����Ÿ��� �ΰ� ���󰣴�(�̵�)
            // �÷��̾� �������� ���͸� ����
            Vector3 targetdir = target.position + (transform.position - target.position).normalized * dist;
            targetdir.y = transform.position.y;

            // ���� �÷��̾��� y ���� ó�� y������ Ŀ���� ��ȭ�� ����ŭ�� ī�޶� ���Ѵ�
            // ��ȭ�ɶ� �� �ѹ��� = startY ���� �ٲ۴�
            if (target.position.y != startY)
            {
                // y���� ������ ���¸� Ų��
                isTargetUp = true;
                
                if (isTargetUp == true)
                {
                    print("playerup");
                    // ��ȭ�� ����ŭ�� ī�޶��� �����ǿ� ���Ѵ�
                    targetdir.y += (target.position.y - startY);
                    // startY ���� ��ȭ��Ų��
                    startY = target.position.y;
                    isTargetUp = false;
                }
            }
            // �Ӽ�
            // ó�� �÷��̾��� y ��(�¾�� ����)
            // ���� �÷��̾��� y �� = target.position.y
            // ��ȭ�� y �� = ������Ʈ���� ����?



            // ī�޶� �÷��̾� ���� ���� ���ϰ� ������
            //if (targetdir.y < target.position.y)
            //{
            //    targetdir.y = target.position.y + 30;
            //}


            // �ε巴�� ����
            transform.position = Vector3.SmoothDamp(transform.position, targetdir, ref currentVelocity, smoothTime);

            // Ÿ���� �ٶ󺻴�
            transform.LookAt(target);
        }


        
       
    }

    public void CameraRestart()
    {
        transform.position = cameraStartPos.position;
        transform.rotation = cameraStartPos.rotation;
    }

    public void CameraShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 originPos = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < shakeTime)
        {
            Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            elapsedTime += Time.deltaTime;

        }

        transform.localPosition = originPos;
    }
}
