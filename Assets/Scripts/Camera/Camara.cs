using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public float shakeTime = 0.5f;
    public float shakeSpeed = 2f;
    public float shakeAmount = 1f;

    private Transform cam;

    // 추적할 타겟
    public Transform target;
    // 플레이어
    public GameObject player;

    // 움직임 값
    private float xRotateMove, yRotateMove;
    // 회전 스피드
    public float rotateSpeed = 500.0f;
    // 카메라와 타겟의 거리
    public float dist = 50f;
    // 부드럽게 이동하는 시간?
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
        // 플레이어가 없으면 입력값 안받음
        if (player.activeSelf)
        {
            // 마우스 입력을 받아온다 (x = 마우스 좌우, y=마우스 위아래 움직임)
            xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;

            // 그 방향으로 회전한다
            transform.RotateAround(target.position, Vector3.right, -yRotateMove);
            transform.RotateAround(target.position, Vector3.up, -xRotateMove);

            // 카메라가 플레이어와 일정거리를 두고 따라간다(이동)
            // 플레이어 방향으로 벡터를 구함
            Vector3 targetdir = target.position + (transform.position - target.position).normalized * dist;

            // 카메라가 플레이어 밑을 보지 못하게 제한함
            if (targetdir.y < target.position.y)
            {
                targetdir.y = target.position.y;
            }

            // 부드럽게 추적
            transform.position = Vector3.SmoothDamp(transform.position, targetdir, ref currentVelocity, smoothTime);

            // 타겟을 바라본다
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
