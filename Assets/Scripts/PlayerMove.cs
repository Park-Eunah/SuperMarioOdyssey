using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Collider co;
    Rigidbody rig;
    // 중력변수
    ConstantForce force;

    public GameObject mario;
    // 캐릭터 바디
    public Transform marioBody;
    // 카메라 팔 (카메라 회전)
    public Transform mainCamera;

    // 애니메이션 변수
    Animator animator;

    // 시작 위치
    public GameObject StartPos;

    // 시작 속도
    public float walkSpeed = 0;
    // 이동 속도
    public float speed = 20f;
    // 대시 속도
    public float dashSpeed = 30f;
    // 변하지 않을 기본 속도
    const float defSpeed = 20f;

    // 움직임 입력 방향
    Vector2 moveInput;
    // 앞 방향
    Vector3 lookForword;
    // 오른쪽 방향
    Vector3 lookRight;
    // 회전 스피드
    float rotatespeed = 10f;

    
    // 벽 점프 파워
    public float wjumpPower = 25f;

    // 기본 점프 파워
    public float defJumpPower = 25f;

    // 점프 파워
    public float JumpPower;

    // 3단 점프 파워
    public float triJumpPower = 40f;

    // 최대 점프 횟수
    public int jumpMaxCount = 3;
    // 현재 점프 횟수
    public int jumpCurrCount = 0;

    // 현재 점프 시간
    public float jumpCurrTime = 0;

    // 점프 상태 확인
    public bool isJumping = false;

    // 박스 위 확인
    public bool ontheBox = false;

    // 벽 점프 상태 확인
    public bool isWallJumping = false;

    // 점프 방향
    Vector3 dir;

    // 벽 상태 확인
    public bool isWall = false;

    // 시간 재기
    public float currTime = 0;

    // 엉덩이 찍기 상태 확인
    public bool isHipdrop = false;

    // 엉덩이 찍은 후 땅에 닿았다 상태 확인
    public bool isHipdropGround = false;

    // 찍기 딜레이 시간
    private float stopTime = 0.2f;

    // 대시 최대 시간
    float dashMaxTime = 2f;
    // 현재 대시 시간
    float dashCurrTime = 0;

    // 대시 상태 확인
    public bool isDashing = false;

    // 굼베티 만남 상태 확인
    public bool isMeeting = false;

    // 캡쳐 상태 확인
    public bool isCapture = false;

    // 생명이 줄어듬
    public bool isLifedown = false;

    // 알파값 증감 상태
    public bool isAlpaup = false;

    int blinkCount = 0;

    // 깜빡임 현재 시간
    public float blinkCurrTime = 0;

    // 깜빡임 시간
    public float blinkTime = 1f;
        
    // 캐피를 불러온다
    public GameObject cappy;

    // 카메라 스크립트
    public MainCamera camerascripts;

    // 랜더러 배열 선언
    //public Renderer[] renderers;

    public AudioClip jump1;
    public AudioClip jump2;
    public AudioClip jump3;

    public GameObject effectWalkFactory;
    public GameObject effectGroundPoundFactory;
    public AudioSource audioSource;

    public Transform test;
    public bool audioplay = false;

    

    // Start is called before the first frame update
    void Start()
    {
        //콜라이더를 가져오자
        co = GetComponent<Collider>();

        //리지드바디를 가져오자
        rig = GetComponent<Rigidbody>();

        // 캐릭터에 있는 animaitor 컴포넌트를 가져와 저장한다.
        animator = marioBody.GetComponent<Animator>();


        force = GetComponent<ConstantForce>();

    }


    // Update is called once per frame
    void Update()
    {
        // 카메라
        //LookAround();

        // 무브 스크립트
        Move();

        // 대시 스크립트
        Dash();

        // 점프, 내려찍기 스크립트
        Jump();

        // 회전 스크립트
        Rotation();

        // 캐피 캡쳐 스크립트
        Capture();

        currTime += Time.deltaTime;

        // 깜빡거림
        Blink();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = test.position;
        }
                
    }

    //void LookAround()
    //{
    //    // 마우스 입력을 받아온다 (x = 마우스 좌우, y=마우스 위아래 움직임)
    //    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //    // 카메라 회전 각도를 받아온다
    //    Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
    //    // 카메라를 회전한다 (마우스 방향 = 바라보는 방향/반대로도 할 수 있음)
    //    cameraArm.rotation = Quaternion.Euler(cameraAngle.x - mouseDelta.y, cameraAngle.y + mouseDelta.x, cameraAngle.z);


    //    // 카메라 x각도를 제한한다
    //    float x = cameraAngle.x - mouseDelta.y;
    //    if (x < 180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 70f);
    //    }
    //    else
    //    {
    //        x = Mathf.Clamp(x, 335f, 361f);
    //    }

    //    cameraArm.eulerAngles = new Vector3(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    //    // cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    //}


    void Move()
    {
        // 내려찍기 중일 때 이동 안함
        if (isHipdrop == true) // 한줄이면 괄호생략가능
            return;

        // 굼베티 만났을 때 이동 안함
        if (isMeeting == true)
            return;

        if (camerascripts.IsMooncamera == true)
        {
            animator.SetBool("Walk", false);
            return;
        }
       
        //원래코드
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        bool isMove = moveInput.magnitude != 0;
        if (isMove && isWallJumping == false)
        {
            // 방향을 만든다
            //Vector3 dir = transform.right * h + transform.forward * v;
            lookForword = new Vector3(mainCamera.forward.x, 0f, mainCamera.forward.z);
            lookRight = new Vector3(mainCamera.right.x, 0f, mainCamera.right.z);
            dir = lookForword * moveInput.y + lookRight * moveInput.x;
            dir.Normalize();
            if (audioplay == false)
            {
                audioplay = true;
                audioSource.Play();
                print(audioSource.clip);
            }
            
            // 애니메이션
            // walk를 true로
            animator.SetBool("Walk", true);
            
            if (isJumping == true)
            {
                // 스피드 값넣기
                walkSpeed = 10f;
            }

            else
            {
                // 속도를 천천히 올린다
                if (walkSpeed < speed)
                {
                    //print("walkSpeed:" + walkSpeed);
                    walkSpeed += Time.deltaTime * 20;
                }
                else if (walkSpeed >= speed)
                {
                    walkSpeed = speed;
                }
                // 원하는 속도가 되면 그 속도를 유지한다
                // 멈추면 바로 멈춘다

            }
            
            //그 방향으로 이동
            //rig.MovePosition(transform.position + dir * Time.deltaTime * speed);
            transform.position += dir * walkSpeed * Time.deltaTime;
            //rig.AddForce(dir * speed);
            //rig.velocity = (dir * walkSpeed);

            if (isJumping == false)
            {
                GameObject walkeffect = Instantiate(effectWalkFactory);
                walkeffect.transform.position = transform.position;
                Destroy(walkeffect, 0.5f);
               
            }

            if (isJumping == true)
            {
                audioSource.Stop();
                audioplay = false;
            }
            

        }

        else
        {
            // 애니메이션, 다시 대기상태로 전환
            animator.SetBool("Walk", false);
            if (audioplay == true)
            {

                audioSource.Stop();
                audioplay = false;

            }
            walkSpeed = 0;
        }

    }

    void Dash()
    {
        // 만약 shift 버튼을 누르면
        if (dashCurrTime < dashMaxTime && Input.GetKeyDown(KeyCode.LeftShift))
        {

            // 애니메이션
            animator.SetBool("Roll", true);

            // 만약 버튼을 눌렀을 때 대시 중이 아니라면
            if (isDashing == false)
            {
                // 대시 상태임
                isDashing = true;
                //움직이는 속도를 2배 높인다
                walkSpeed = dashSpeed;
            }

        }

        // 만약 대시 중이라면
        if (isDashing == true)
        {
            // 현재 대시시간을 잰다
            dashCurrTime += Time.deltaTime;
            // 만약 현재 대시 시간이 최대 대시 시간이 되면
            if (dashCurrTime >= dashMaxTime)
            {
                //움직이는 속도를 원래대로 한다
                walkSpeed = defSpeed;
                //현재 대시 시간을 0으로 만든다
                dashCurrTime = 0;
                //대시 상태 아님
                isDashing = false;
                //대시상태 끔
                animator.SetBool("Roll", false);
            }
        }
    }

    void Jump()
    {
        // 플레이어가 점프 하는 중인지 확인한다
        // = isjumping = false일때
        // 만약 플레이어가 점프하는 중이 아니라면
        if (isJumping == false || ontheBox == true)
        {
            // 점프 시간을 잰다
            jumpCurrTime += Time.deltaTime;


            // 사용자가 스페이스 바를 눌렀을때 (만약 사용자가 스페이스 바를 누른다면)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                // 점프 횟수를 센다
                jumpCurrCount++;
                // 점프 중으로 바꾼다
                isJumping = true;
                ontheBox = false;
                // 
                if (isWall == true)
                {
                    WallJump();
                }

                // 1번 점프
                // 만약 점프 횟수가 1이라면
                else if (jumpCurrCount == 1)
                {
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 1번 점프 = 기본 점프

                    dir = transform.up;

                    // 애니메이션
                    animator.SetBool("Jump", true);
                    JumpPower = defJumpPower;
                    // 1번 점프를 한다
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    AudioSource.PlayClipAtPoint(jump1, transform.position, 1f);

                }

                // 2번 점프 
                // 만약 1번 점프= count = 1 후 , n초가 지났을때,
                else if (jumpCurrCount == 2 && jumpCurrTime <= 0.3f)
                {
                    // 스페이스 바를 누르면
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 2번 점프를 한다
                    // 점프할 방향을 정한다
                    dir = transform.up;
                    // 애니메이션
                    animator.SetBool("Jump2", true);
                    // 2번 점프를 한다
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    // 2번 점프 = 애니메이션이 바뀜
                    AudioSource.PlayClipAtPoint(jump2, transform.position, 1f);
                    // 만약 입력값이 없이 점프를 누른 상태이면
                    if (moveInput == Vector2.zero)
                    {
                        // 점프 횟수를 초기화한다
                        jumpCurrCount = 0;
                    }
                }

                // 3번 점프
                // 만약 2번 점프 후 n초가 지났을때,
                else if (jumpCurrCount == 3 && jumpCurrTime <= 1f)
                {
                    // 스페이스 바를 누르면
                    // 점프 시간을 0으로
                    jumpCurrTime = 0;
                    // 3번 점프를 한다

                    // 점프할 방향을 정한다
                    dir = transform.up;

                    // 애니메이션
                    animator.SetBool("Jump3", true);
                    // 스피드를 빠르게한다
                    //speed = 10f;
                    JumpPower = triJumpPower;
                    // 3번 점프를 한다
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    AudioSource.PlayClipAtPoint(jump3, transform.position, 1f);
                }



            }

            else if (jumpCurrTime >= 0.3f)
            {
                // 점프 횟수를 초기화 한다
                jumpCurrCount = 0;
            }

        }

        // Hipdrop
        // 만약 플레이어가 점프중 일 때
        else if (isJumping == true)
        {
            // 내려찍기 스크립트, Hipdrop
            // 키보드 왼쪽 컨트롤 키를 누른다면
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isHipdrop = true;
                currTime = 0;
                // 현재 높이에서 0.2 초간 멈춘다
                currTime += Time.deltaTime;
                
                // 내려찍기 애니메이션을 실행한다
                animator.SetBool("HipDrop", true);

                // = x, z값을 0으로 만든다 = 입력 x
                // 수직으로 빠르게 떨어진다 = yVelocity 값을 -로 하면 더 빠르게 떨어진다
                // 오류 수정
                if (currTime < stopTime)
                {
                    force.force = Vector3.zero;
                    rig.velocity = Vector3.zero;

                    Invoke("DropForce", 0.3f);
                }
                else
                {
                    

                    rig.velocity = new Vector3(0, -50f, 0);
                    rig.AddForce(Vector3.up * -50f * defJumpPower);
                    
                }

            }

            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                
                // 힙드롭 상태를 끈다
                isHipdrop = false;

                // 힙드롭 후 땅에 닿았다 상태를 킨다
                isHipdropGround = true;
                PoundEffect();
                // reaction 애니메이션을 끈다
                animator.SetBool("Reaction", false);
                animator.SetBool("HipDrop", false);
                animator.SetBool("Jump", false);
            }

        }
    }

    void DropForce()
    {
        force.force = new Vector3(0, -150f, 0);
    }
    void WallJump()
    {

        // 점프 시간을 0으로
        jumpCurrTime = 0;
        // 벽점프(벽만있으면 계속가능
        isWallJumping = true;
        // 몸통을 뒤를 보게 한다
        //marioBody.Rotate(0, 180, 0);
        animator.SetBool("Walk", false);
        // 애니메이션 켜기
        animator.SetBool("WallJump", true);
        
        // 점프 위로 방향을 정한다
        dir = marioBody.up + -marioBody.forward;
        dir.Normalize();
        // velocity 값을 0으로 초기화한다
        rig.velocity = Vector3.zero;
        // 점프를 한다
        rig.AddForce(dir * wjumpPower, ForceMode.Impulse);
    }

    void Capture()
    {
        // 사용자가 fire1버튼을 누르면(마리오에서 호출)
        if (Input.GetMouseButtonDown(0))
        {
            // 캡쳐상태임
            isCapture = true;
            // 캐피를 켠다(마리오에서)
            cappy.SetActive(true);

        }
    }

    void Rotation()
    {
        // 마리오가 회전할때 자연스럽게 회전한다
        marioBody.rotation = Quaternion.Lerp(marioBody.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), Time.deltaTime * rotatespeed);
    }

    public void Die()
    {
        // 마리오 생명이 0이되면 게임매니저가 호출한다
        // 죽음 애니메이션을 실행한다
        animator.SetBool("Die", true);

    }

    public void ReStart()
    {
        // 죽음 애니메이션을 끈다
        animator.SetBool("Die", false);
        // 마리오의 위치를 시작위치로 한다
        this.transform.position = StartPos.transform.position;
        this.transform.rotation = StartPos.transform.rotation;
    }

    public void MoonJump()
    {
        // Moon에 닿았을때,(moon스크립트에서 확인)     
        // 마리오를 활성화 시킨다
        gameObject.SetActive(true);

        // 카메라의 -forward 방향을 본다
        transform.forward = -mainCamera.forward;
        // 위로 이동한다 (이동 수정)
        rig.AddForce(Vector3.up * defJumpPower, ForceMode.Impulse);

        // transform.position += Vector3.up * speed * Time.unscaledDeltaTime;
        // 이동시 Time.unscaledeltatime을 쓴다 (시간이 멈춰있는 상태에서 움직여야 할때 deltatime대신 사용)
    }

    void Blink()
    {
        if (isLifedown == true)
        {
            isLifedown = false;
            StartCoroutine("MarioBlink");
            
        }
    }

    IEnumerator MarioBlink()
    {
        //if (blinkCurrTime <= blinkTime)
        {
            //blinkCount++;
            //if (blinkCount % 2 == 0)
            //{
            //    mario.gameObject.SetActive(false);
            //}
            //else
            //{
            //    mario.gameObject.SetActive(true);
            //}
            //if (blinkCount == 50)
            //{
            //    blinkCount = 0;
            //    StopCoroutine(MarioBlink());
            //    isLifedown = false;
            //}
            //else
            //yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < 50; i++) // 50 = 0.1초씩 반복을 5초동안 할거라 10*5함
            {
                if (i % 2 == 0)
                {
                    mario.gameObject.SetActive(false);
                }
                else
                {
                    mario.gameObject.SetActive(true);
                }
                if (i == 49)
                {
                    StopCoroutine(MarioBlink());
                }
                yield return new WaitForSeconds(0.05f);

            }

        }

    }

    void PoundEffect()
    {
        GameObject poundeffect = Instantiate(effectGroundPoundFactory);
        poundeffect.transform.position = transform.position;
        Destroy(poundeffect, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // isHipdrop이 true 일때, quistion block과 충돌 상태라면, reaction 애니메이션을 켠다
        if (collision.gameObject.name.Contains("Question"))
        {
            //isJumping = false;
            ontheBox = true;
            force.force = new Vector3(0, -39.81f, 0);
        }

        else if (isHipdrop == true && collision.gameObject.name.Contains("Block"))
        {

            collision.gameObject.GetComponent<Block>().SetAnimDown();
            // reaction 애니메이션을 켠다
            //animator.SetBool("Reaction", true);
            animator.SetBool("HipDrop", false);
            isHipdrop = false;
            camerascripts.CameraShake();

            PoundEffect();

        }

        else if (collision.gameObject.name.Contains("Block"))
        {
            ontheBox = true;

            force.force = new Vector3(0, -39.81f, 0);
            animator.SetBool("Jump", false);
            animator.SetBool("Jump2", false);
            animator.SetBool("Jump3", false);
            animator.SetBool("HipDrop", false);
        }

        else if (collision.gameObject.CompareTag("DeadZone"))
        {
            GameManager.instance.Die();
        }
        
        // 만약 플레이어가 땅에 닿아있다면
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // 땅에 닿았을 때, 점프 현재 횟수가 최대 횟수보다 크다면,
            if (jumpCurrCount >= jumpMaxCount)
            {
                // 점프 횟수를 초기화 한다
                jumpCurrCount = 0;
            }

            if (isHipdrop == true)
            {
                camerascripts.CameraShake();
                PoundEffect();
            }
            // 만약 땅이 솟은땅 일 때
            if (isHipdrop && collision.gameObject.name.Contains("HipDrop"))
            {
                // 솟은땅을 끈다
                collision.transform.parent.gameObject.SetActive(false);
                // shake 함수 호출?
                camerascripts.CameraShake();

            }

            // collision에서 받아온 부딫힌 위치의 y 값이 플레이어의 y값보다 작을때 
            if (collision.contacts[0].point.y < transform.position.y)
            {
                //이건 땅이다
                // 땅일때 해야하는 일
                ontheBox = false;
                isJumping = false;
                speed = defSpeed;
                isHipdropGround = true;
                isHipdrop = false;
                isDashing = false;
                isWall = false;
                isWallJumping = false;
                force.force = new Vector3(0, -39.81f, 0);
                // 애니메이션
                //animator.SetBool("Wait", true);

                // 점프애니메이션 끄기
                animator.SetBool("Jump", false);
                // 점프 2 애니메이션
                animator.SetBool("Jump2", false);
                // 애니메이션
                animator.SetBool("Jump3", false);
                // 애니메이션
                animator.SetBool("HipDrop", false);
                animator.SetBool("WallJump", false);
                animator.SetBool("Reaction", false);
            }

            // collision에서 받은 y 값이 플레이어의 값보다 크거나 같을때
            else if (collision.contacts[0].point.y >= transform.position.y)
            {
                //이건 벽이다
                isWall = true;
                // 벽일때 해야할 것
                // 점프하기
                isJumping = false;
                isWallJumping = false;
                // 점프 횟수를 초기화
                jumpCurrCount = 0;

                
                // 점프 2 애니메이션
                animator.SetBool("Jump2", false);
                // 애니메이션
                animator.SetBool("Jump3", false);
                // 애니메이션
                animator.SetBool("HipDrop", false);

            }
        }
    }

}
