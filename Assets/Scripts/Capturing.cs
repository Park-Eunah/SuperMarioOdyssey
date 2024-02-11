using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturing : MonoBehaviour
{
    // 캐피를 소환
    public GameObject Cappy;
    // 던지는 시간
    public float ThrowTime = 1f;
    // 던지는 거리
    public float ThrowDistance = 3f;
    // 되돌아오기
    bool Return;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 만약 사용자가 오른쪽 마우스를 누르면, return이 false이면
        if (Input.GetButtonDown("Fire1") && Return == false)
        {
            CapThrow();
        }
    }

    void CapThrow()
    {
        // 캐피 위치의 부모를 null값으로 한다?
        Cappy.transform.SetParent(null);
        // 방향을 정한다
        Vector3 Forward = transform.forward;
        // 그 방향으로 정해진 거리, 시간만큼 움직인다 DOTween 사용(해도되나???)
        // Cappy.transform.DOBle
    }
}
