using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    // 추적할 타겟
    public Transform target;
    // 플레이어
    public GameObject player;

    // 움직임 값
    private float xRotateMove, yRotateMove;
    // 회전 스피드
    public float rotateSpeed = 500.0f;
    // 카메라와 타겟의 거리
    public float dist = 20f;
    // 부드럽게 이동하는 시간?
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;

    // 문
    public Transform moon;
    // 달 위치
    // 달 스크립트
    private PowerMoon powermoon;

    // 카메라 지진
    public float shakeTime = 0.5f;
    public float shakeSpeed = 2f;
    public float shakeAmount = 1f;

    // 처음 위치 값
    public float startY;

    // 변화된 위치 값
    public float deltaY;

    // y 값 증가함
    public bool isTargetUp = false;

    // 시작
    public Transform cameraStartPos;

    // 달이 나타나면 달이 점프 하는 동안 따라갔다가
    // 다시 제자리에 오면 플레이어를 다시 따라가고 싶다

    // Start is called before the first frame update
    void Start()
    {
        powermoon = moon.GetComponent<PowerMoon>();

        // 시작할때 플레이어의 y 값을 저장
        startY = target.transform.position.y;

        // 카메라의 위치를 처음 위치로
        transform.position = cameraStartPos.position;
        transform.rotation = cameraStartPos.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 만약 달이 있다면 달을 타겟으로 바꾼다
        if (moon.gameObject.activeSelf && !powermoon.ISWAITING)
        {
            transform.LookAt(moon);
        }
        // 플레이어가 없으면 입력값 안받음
        else if (player.activeSelf)
        {
            // 마우스 입력을 받아온다 (x = 마우스 좌우, y=마우스 위아래 움직임)
            xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;

            
            // 아니면 타겟을 플레이어로 바꾼다
            //if()
            //{
                target = player.transform;
            //}

            // 그 방향으로 회전한다
            transform.RotateAround(target.position, Vector3.right, yRotateMove);
            transform.RotateAround(target.position, Vector3.up, xRotateMove);

            // 카메라가 플레이어와 일정거리를 두고 따라간다(이동)
            // 플레이어 방향으로 벡터를 구함
            Vector3 targetdir = target.position + (transform.position - target.position).normalized * dist;
            targetdir.y = transform.position.y;

            // 만약 플레이어의 y 값이 처음 y값보다 커지면 변화된 값만큼을 카메라에 더한다
            // 변화될때 딱 한번만 = startY 값을 바꾼다
            if (target.position.y != startY)
            {
                // y값이 증가함 상태를 킨다
                isTargetUp = true;
                
                if (isTargetUp == true)
                {
                    print("playerup");
                    // 변화된 값만큼을 카메라의 포지션에 더한다
                    targetdir.y += (target.position.y - startY);
                    // startY 값을 변화시킨다
                    startY = target.position.y;
                    isTargetUp = false;
                }
            }
            // 속성
            // 처음 플레이어의 y 값(태어날때 저장)
            // 현재 플레이어의 y 값 = target.position.y
            // 변화된 y 값 = 업데이트에서 지정?



            // 카메라가 플레이어 밑을 보지 못하게 제한함
            //if (targetdir.y < target.position.y)
            //{
            //    targetdir.y = target.position.y + 30;
            //}


            // 부드럽게 추적
            transform.position = Vector3.SmoothDamp(transform.position, targetdir, ref currentVelocity, smoothTime);

            // 타겟을 바라본다
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
