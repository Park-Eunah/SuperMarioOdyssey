using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // 회전값(마우스 움직임 누적)
    float rotX = 0;
    float rotY = 0;

    // 회전 스피드
    float rotSpeed = 200f;

    // 회전 최대값
    float maxRotY = 90f;
    // 회전 최솟값
    float minRotY = 20f;

    // 거리 최대값
    float maxDist = 130f;
    // 거리 최솟값(각도 0일때 값)
    float minDist = 70f;
    // 가장 가까운 거리
    float defDist = 20f;

    // 마리오
    public Transform marioBody;

    // 메인 카메라
    public Transform mainCamera;

    // 카메라 처음 위치에서의 거리
    float defDistance;

    // 추적할 타겟
    public Transform target;

    public Transform camStartPos;

    float currTime = 0;
    float startTime = 2f;

    // 카메라 지진
    public float shakeTime = 0.3f;
    public float shakeSpeed = 1f;
    public float shakeAmount = 2f;

    // 문
    public Transform moon;
    // 달 위치
    // 달 스크립트
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

    // 마우스가 위 아래로 움직이면(x 회전 값), 마리오와 거리가 정해진대로 이동하고 싶다 (마리오와의 거리 최댓값 = 100, 최솟값 = 50)
    // x회전값의 각도에 따라 마리오와의 거리를 정한다
    // 회전 최대값과 최솟값을 정한다
    // 마리오와의 거리의 최댓값과 최솟값을 정한다
    // 마우스의 x회전값이 90일때 마리오와의 
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

        // 카메라회전
        print("mouseinput");

        // 사용자의 마우스 움직임 값을 받는다
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 마우스의 움직임값을 누적시키자
        rotX += mx * Time.deltaTime * rotSpeed;
        rotY += my * Time.deltaTime * rotSpeed;

        //if (rotY < -90) rotY = -90;
        //if (rotY > 0) rotY = 0;

        // X회전 최대값과 최솟값을 정한다
        rotY = Mathf.Clamp(rotY, -maxRotY, minRotY);

        // 마우스의 x회전값이 최소일때(가까워지게), 최대일떄(멀어지게) 메인 카메라의 z값을 움직인다
        float cameraZ = (maxDist - minDist) * rotY / maxRotY - minDist;

        if (rotY > 0)
        {
            cameraZ = -defDist;
        }

        mainCamera.localPosition = new Vector3(0, 0, cameraZ);

        // 누적된 값을 물체의 회전값으로 셋팅
        transform.localEulerAngles = new Vector3(-rotY, rotX, 0);

        // 현재 거리값을 계산한다
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
