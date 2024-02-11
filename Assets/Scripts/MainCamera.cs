using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // ȸ����(���콺 ������ ����)
    float rotX = 0;
    float rotY = 0;

    // ȸ�� ���ǵ�
    float rotSpeed = 200f;

    // ȸ�� �ִ밪
    float maxRotY = 90f;
    // ȸ�� �ּڰ�
    float minRotY = 20f;

    // �Ÿ� �ִ밪
    float maxDist = 130f;
    // �Ÿ� �ּڰ�(���� 0�϶� ��)
    float minDist = 70f;
    // ���� ����� �Ÿ�
    float defDist = 20f;

    // ������
    public Transform marioBody;

    // ���� ī�޶�
    public Transform mainCamera;

    // ī�޶� ó�� ��ġ������ �Ÿ�
    float defDistance;

    // ������ Ÿ��
    public Transform target;

    public Transform camStartPos;

    float currTime = 0;
    float startTime = 2f;

    // ī�޶� ����
    public float shakeTime = 0.3f;
    public float shakeSpeed = 1f;
    public float shakeAmount = 2f;

    // ��
    public Transform moon;
    // �� ��ġ
    // �� ��ũ��Ʈ
    private PowerMoon powermoon;

    public Transform mooncamera;

    public bool IsMooncamera = false;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = camStartPos.position;
        //transform.rotation = camStartPos.rotation;
        rotX = transform.localEulerAngles.y;
        rotY = -transform.localEulerAngles.x;
        powermoon = moon.GetComponent<PowerMoon>();
    }

    // ���콺�� �� �Ʒ��� �����̸�(x ȸ�� ��), �������� �Ÿ��� ��������� �̵��ϰ� �ʹ� (���������� �Ÿ� �ִ� = 100, �ּڰ� = 50)
    // xȸ������ ������ ���� ���������� �Ÿ��� ���Ѵ�
    // ȸ�� �ִ밪�� �ּڰ��� ���Ѵ�
    // ���������� �Ÿ��� �ִ񰪰� �ּڰ��� ���Ѵ�
    // ���콺�� xȸ������ 90�϶� ���������� 
    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (moon.gameObject.activeSelf && !powermoon.ISWAITING)
        {
            IsMooncamera = true;
            mainCamera.position = mooncamera.position;
            transform.LookAt(moon);
            
            return;
        }


        IsMooncamera = false;
        //if (currTime < startTime)
        //return;

        // ī�޶�ȸ��
        print("mouseinput");

        // ������� ���콺 ������ ���� �޴´�
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ���콺�� �����Ӱ��� ������Ű��
        rotX += mx * Time.deltaTime * rotSpeed;
        rotY += my * Time.deltaTime * rotSpeed;

        //if (rotY < -90) rotY = -90;
        //if (rotY > 0) rotY = 0;

        // Xȸ�� �ִ밪�� �ּڰ��� ���Ѵ�
        rotY = Mathf.Clamp(rotY, -maxRotY, minRotY);

        // ���콺�� xȸ������ �ּ��϶�(���������), �ִ��ϋ�(�־�����) ���� ī�޶��� z���� �����δ�
        float cameraZ = (maxDist - minDist) * rotY / maxRotY - minDist;

        if (rotY > 0)
        {
            cameraZ = -defDist;
        }

        mainCamera.localPosition = new Vector3(0, 0, cameraZ);

        // ������ ���� ��ü�� ȸ�������� ����
        transform.localEulerAngles = new Vector3(-rotY, rotX, 0);

        // ���� �Ÿ����� ����Ѵ�
        //defDistance = Vector3.Distance(marioBody.position, mainCamera.position);
        //print("distance: "+ defDistance);

        

    }

    public void CameraRestart()
    {
        transform.position = camStartPos.position;
        transform.rotation = camStartPos.rotation;
    }

    public void CameraShake()
    {
        print("CameraShake");
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
            print("Shaking");
        }

        transform.localPosition = originPos;
    }
}
