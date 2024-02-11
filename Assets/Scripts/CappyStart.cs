using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyStart : MonoBehaviour
{
    // 태어날때, 깃발 위치에 배치된 상태임(Scene에서 배치)
    // 시작하고 2초 후에 힘이 가해진 방향(아래, 위)으로 점프한다
    // 점프하는 동안 계속 회전한다
    // 점프 후 땅에 닿으면,
    // 회전을 멈춘다
    // 캐피의 setActive를 false로 하고
    // 마리오의 setActive를 ture로 한다
    // 이때 start 애니메이션을 실행한다(triger)

    // 현재 시간
    public float currTime = 0;
    // 시작 시간
    public float startTime = 1f;
    // 애니메이션 끝나는 시간
    public float animEndTime = 2f;

    Animator anim;

    Rigidbody rig;

    public GameObject Player;
    

    // Start is called before the first frame update
    void Start()
    {
        //리지드바디를 가져오자
        rig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;

        if (currTime >= animEndTime)
        {
            // 마리오를 킨다
            Player.gameObject.SetActive(true);
            // 나를 끈다
            this.gameObject.SetActive(false);

        }

        //// 애니메이션이 끝나면 
        //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 1)
        //{
        //    // 마리오를 킨다
        //    Player.gameObject.SetActive(true);
        //    // 나 를 끈다
        //    this.gameObject.SetActive(false);
        //}

    }

   
}
