using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public float shakeTime = 0.5f;
    public float shakeSpeed = 2f;
    public float shakeAmount = 1f;

    private Transform cam;

    // ������ Ÿ��
    public Transform target;
    // �÷��̾�
    public GameObject player;

    // ������ ��
    private float xRotateMove, yRotateMove;
    // ȸ�� ���ǵ�
    public float rotateSpeed = 500.0f;
    // ī�޶�� Ÿ���� �Ÿ�
    public float dist = 50f;
    // �ε巴�� �̵��ϴ� �ð�?
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ������ �Է°� �ȹ���
        if (player.activeSelf)
        {
            // ���콺 �Է��� �޾ƿ´� (x = ���콺 �¿�, y=���콺 ���Ʒ� ������)
            xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;

            // �� �������� ȸ���Ѵ�
            transform.RotateAround(target.position, Vector3.right, -yRotateMove);
            transform.RotateAround(target.position, Vector3.up, -xRotateMove);

            // ī�޶� �÷��̾�� �����Ÿ��� �ΰ� ���󰣴�(�̵�)
            // �÷��̾� �������� ���͸� ����
            Vector3 targetdir = target.position + (transform.position - target.position).normalized * dist;

            // ī�޶� �÷��̾� ���� ���� ���ϰ� ������
            if (targetdir.y < target.position.y)
            {
                targetdir.y = target.position.y;
            }

            // �ε巴�� ����
            transform.position = Vector3.SmoothDamp(transform.position, targetdir, ref currentVelocity, smoothTime);

            // Ÿ���� �ٶ󺻴�
            transform.LookAt(target);
        }

        //if (isHipdrop == ture)
        {
            StartCoroutine(Shake());
        }
    }
    IEnumerator Shake()
    {
        Vector3 originPos = cam.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < shakeTime)
        {
            Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;
            cam.localPosition = Vector3.Lerp(cam.localPosition, randomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            elapsedTime += Time.deltaTime;

        }

        cam.localPosition = originPos;
    }
}
