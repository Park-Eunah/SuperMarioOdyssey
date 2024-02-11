using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Collider co;
    Rigidbody rig;
    // �߷º���
    ConstantForce force;

    public GameObject mario;
    // ĳ���� �ٵ�
    public Transform marioBody;
    // ī�޶� �� (ī�޶� ȸ��)
    public Transform mainCamera;

    // �ִϸ��̼� ����
    Animator animator;

    // ���� ��ġ
    public GameObject StartPos;

    // ���� �ӵ�
    public float walkSpeed = 0;
    // �̵� �ӵ�
    public float speed = 20f;
    // ��� �ӵ�
    public float dashSpeed = 30f;
    // ������ ���� �⺻ �ӵ�
    const float defSpeed = 20f;

    // ������ �Է� ����
    Vector2 moveInput;
    // �� ����
    Vector3 lookForword;
    // ������ ����
    Vector3 lookRight;
    // ȸ�� ���ǵ�
    float rotatespeed = 10f;

    
    // �� ���� �Ŀ�
    public float wjumpPower = 25f;

    // �⺻ ���� �Ŀ�
    public float defJumpPower = 25f;

    // ���� �Ŀ�
    public float JumpPower;

    // 3�� ���� �Ŀ�
    public float triJumpPower = 40f;

    // �ִ� ���� Ƚ��
    public int jumpMaxCount = 3;
    // ���� ���� Ƚ��
    public int jumpCurrCount = 0;

    // ���� ���� �ð�
    public float jumpCurrTime = 0;

    // ���� ���� Ȯ��
    public bool isJumping = false;

    // �ڽ� �� Ȯ��
    public bool ontheBox = false;

    // �� ���� ���� Ȯ��
    public bool isWallJumping = false;

    // ���� ����
    Vector3 dir;

    // �� ���� Ȯ��
    public bool isWall = false;

    // �ð� ���
    public float currTime = 0;

    // ������ ��� ���� Ȯ��
    public bool isHipdrop = false;

    // ������ ���� �� ���� ��Ҵ� ���� Ȯ��
    public bool isHipdropGround = false;

    // ��� ������ �ð�
    private float stopTime = 0.2f;

    // ��� �ִ� �ð�
    float dashMaxTime = 2f;
    // ���� ��� �ð�
    float dashCurrTime = 0;

    // ��� ���� Ȯ��
    public bool isDashing = false;

    // ����Ƽ ���� ���� Ȯ��
    public bool isMeeting = false;

    // ĸ�� ���� Ȯ��
    public bool isCapture = false;

    // ������ �پ��
    public bool isLifedown = false;

    // ���İ� ���� ����
    public bool isAlpaup = false;

    int blinkCount = 0;

    // ������ ���� �ð�
    public float blinkCurrTime = 0;

    // ������ �ð�
    public float blinkTime = 1f;
        
    // ĳ�Ǹ� �ҷ��´�
    public GameObject cappy;

    // ī�޶� ��ũ��Ʈ
    public MainCamera camerascripts;

    // ������ �迭 ����
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
        //�ݶ��̴��� ��������
        co = GetComponent<Collider>();

        //������ٵ� ��������
        rig = GetComponent<Rigidbody>();

        // ĳ���Ϳ� �ִ� animaitor ������Ʈ�� ������ �����Ѵ�.
        animator = marioBody.GetComponent<Animator>();


        force = GetComponent<ConstantForce>();

    }


    // Update is called once per frame
    void Update()
    {
        // ī�޶�
        //LookAround();

        // ���� ��ũ��Ʈ
        Move();

        // ��� ��ũ��Ʈ
        Dash();

        // ����, ������� ��ũ��Ʈ
        Jump();

        // ȸ�� ��ũ��Ʈ
        Rotation();

        // ĳ�� ĸ�� ��ũ��Ʈ
        Capture();

        currTime += Time.deltaTime;

        // �����Ÿ�
        Blink();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = test.position;
        }
                
    }

    //void LookAround()
    //{
    //    // ���콺 �Է��� �޾ƿ´� (x = ���콺 �¿�, y=���콺 ���Ʒ� ������)
    //    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //    // ī�޶� ȸ�� ������ �޾ƿ´�
    //    Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
    //    // ī�޶� ȸ���Ѵ� (���콺 ���� = �ٶ󺸴� ����/�ݴ�ε� �� �� ����)
    //    cameraArm.rotation = Quaternion.Euler(cameraAngle.x - mouseDelta.y, cameraAngle.y + mouseDelta.x, cameraAngle.z);


    //    // ī�޶� x������ �����Ѵ�
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
        // ������� ���� �� �̵� ����
        if (isHipdrop == true) // �����̸� ��ȣ��������
            return;

        // ����Ƽ ������ �� �̵� ����
        if (isMeeting == true)
            return;

        if (camerascripts.IsMooncamera == true)
        {
            animator.SetBool("Walk", false);
            return;
        }
       
        //�����ڵ�
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        bool isMove = moveInput.magnitude != 0;
        if (isMove && isWallJumping == false)
        {
            // ������ �����
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
            
            // �ִϸ��̼�
            // walk�� true��
            animator.SetBool("Walk", true);
            
            if (isJumping == true)
            {
                // ���ǵ� ���ֱ�
                walkSpeed = 10f;
            }

            else
            {
                // �ӵ��� õõ�� �ø���
                if (walkSpeed < speed)
                {
                    //print("walkSpeed:" + walkSpeed);
                    walkSpeed += Time.deltaTime * 20;
                }
                else if (walkSpeed >= speed)
                {
                    walkSpeed = speed;
                }
                // ���ϴ� �ӵ��� �Ǹ� �� �ӵ��� �����Ѵ�
                // ���߸� �ٷ� �����

            }
            
            //�� �������� �̵�
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
            // �ִϸ��̼�, �ٽ� �����·� ��ȯ
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
        // ���� shift ��ư�� ������
        if (dashCurrTime < dashMaxTime && Input.GetKeyDown(KeyCode.LeftShift))
        {

            // �ִϸ��̼�
            animator.SetBool("Roll", true);

            // ���� ��ư�� ������ �� ��� ���� �ƴ϶��
            if (isDashing == false)
            {
                // ��� ������
                isDashing = true;
                //�����̴� �ӵ��� 2�� ���δ�
                walkSpeed = dashSpeed;
            }

        }

        // ���� ��� ���̶��
        if (isDashing == true)
        {
            // ���� ��ýð��� ���
            dashCurrTime += Time.deltaTime;
            // ���� ���� ��� �ð��� �ִ� ��� �ð��� �Ǹ�
            if (dashCurrTime >= dashMaxTime)
            {
                //�����̴� �ӵ��� ������� �Ѵ�
                walkSpeed = defSpeed;
                //���� ��� �ð��� 0���� �����
                dashCurrTime = 0;
                //��� ���� �ƴ�
                isDashing = false;
                //��û��� ��
                animator.SetBool("Roll", false);
            }
        }
    }

    void Jump()
    {
        // �÷��̾ ���� �ϴ� ������ Ȯ���Ѵ�
        // = isjumping = false�϶�
        // ���� �÷��̾ �����ϴ� ���� �ƴ϶��
        if (isJumping == false || ontheBox == true)
        {
            // ���� �ð��� ���
            jumpCurrTime += Time.deltaTime;


            // ����ڰ� �����̽� �ٸ� �������� (���� ����ڰ� �����̽� �ٸ� �����ٸ�)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                // ���� Ƚ���� ����
                jumpCurrCount++;
                // ���� ������ �ٲ۴�
                isJumping = true;
                ontheBox = false;
                // 
                if (isWall == true)
                {
                    WallJump();
                }

                // 1�� ����
                // ���� ���� Ƚ���� 1�̶��
                else if (jumpCurrCount == 1)
                {
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 1�� ���� = �⺻ ����

                    dir = transform.up;

                    // �ִϸ��̼�
                    animator.SetBool("Jump", true);
                    JumpPower = defJumpPower;
                    // 1�� ������ �Ѵ�
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    AudioSource.PlayClipAtPoint(jump1, transform.position, 1f);

                }

                // 2�� ���� 
                // ���� 1�� ����= count = 1 �� , n�ʰ� ��������,
                else if (jumpCurrCount == 2 && jumpCurrTime <= 0.3f)
                {
                    // �����̽� �ٸ� ������
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 2�� ������ �Ѵ�
                    // ������ ������ ���Ѵ�
                    dir = transform.up;
                    // �ִϸ��̼�
                    animator.SetBool("Jump2", true);
                    // 2�� ������ �Ѵ�
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    // 2�� ���� = �ִϸ��̼��� �ٲ�
                    AudioSource.PlayClipAtPoint(jump2, transform.position, 1f);
                    // ���� �Է°��� ���� ������ ���� �����̸�
                    if (moveInput == Vector2.zero)
                    {
                        // ���� Ƚ���� �ʱ�ȭ�Ѵ�
                        jumpCurrCount = 0;
                    }
                }

                // 3�� ����
                // ���� 2�� ���� �� n�ʰ� ��������,
                else if (jumpCurrCount == 3 && jumpCurrTime <= 1f)
                {
                    // �����̽� �ٸ� ������
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 3�� ������ �Ѵ�

                    // ������ ������ ���Ѵ�
                    dir = transform.up;

                    // �ִϸ��̼�
                    animator.SetBool("Jump3", true);
                    // ���ǵ带 �������Ѵ�
                    //speed = 10f;
                    JumpPower = triJumpPower;
                    // 3�� ������ �Ѵ�
                    rig.AddForce(dir * JumpPower, ForceMode.Impulse);
                    AudioSource.PlayClipAtPoint(jump3, transform.position, 1f);
                }



            }

            else if (jumpCurrTime >= 0.3f)
            {
                // ���� Ƚ���� �ʱ�ȭ �Ѵ�
                jumpCurrCount = 0;
            }

        }

        // Hipdrop
        // ���� �÷��̾ ������ �� ��
        else if (isJumping == true)
        {
            // ������� ��ũ��Ʈ, Hipdrop
            // Ű���� ���� ��Ʈ�� Ű�� �����ٸ�
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isHipdrop = true;
                currTime = 0;
                // ���� ���̿��� 0.2 �ʰ� �����
                currTime += Time.deltaTime;
                
                // ������� �ִϸ��̼��� �����Ѵ�
                animator.SetBool("HipDrop", true);

                // = x, z���� 0���� ����� = �Է� x
                // �������� ������ �������� = yVelocity ���� -�� �ϸ� �� ������ ��������
                // ���� ����
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
                
                // ����� ���¸� ����
                isHipdrop = false;

                // ����� �� ���� ��Ҵ� ���¸� Ų��
                isHipdropGround = true;
                PoundEffect();
                // reaction �ִϸ��̼��� ����
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

        // ���� �ð��� 0����
        jumpCurrTime = 0;
        // ������(���������� ��Ӱ���
        isWallJumping = true;
        // ������ �ڸ� ���� �Ѵ�
        //marioBody.Rotate(0, 180, 0);
        animator.SetBool("Walk", false);
        // �ִϸ��̼� �ѱ�
        animator.SetBool("WallJump", true);
        
        // ���� ���� ������ ���Ѵ�
        dir = marioBody.up + -marioBody.forward;
        dir.Normalize();
        // velocity ���� 0���� �ʱ�ȭ�Ѵ�
        rig.velocity = Vector3.zero;
        // ������ �Ѵ�
        rig.AddForce(dir * wjumpPower, ForceMode.Impulse);
    }

    void Capture()
    {
        // ����ڰ� fire1��ư�� ������(���������� ȣ��)
        if (Input.GetMouseButtonDown(0))
        {
            // ĸ�Ļ�����
            isCapture = true;
            // ĳ�Ǹ� �Ҵ�(����������)
            cappy.SetActive(true);

        }
    }

    void Rotation()
    {
        // �������� ȸ���Ҷ� �ڿ������� ȸ���Ѵ�
        marioBody.rotation = Quaternion.Lerp(marioBody.rotation, Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z)), Time.deltaTime * rotatespeed);
    }

    public void Die()
    {
        // ������ ������ 0�̵Ǹ� ���ӸŴ����� ȣ���Ѵ�
        // ���� �ִϸ��̼��� �����Ѵ�
        animator.SetBool("Die", true);

    }

    public void ReStart()
    {
        // ���� �ִϸ��̼��� ����
        animator.SetBool("Die", false);
        // �������� ��ġ�� ������ġ�� �Ѵ�
        this.transform.position = StartPos.transform.position;
        this.transform.rotation = StartPos.transform.rotation;
    }

    public void MoonJump()
    {
        // Moon�� �������,(moon��ũ��Ʈ���� Ȯ��)     
        // �������� Ȱ��ȭ ��Ų��
        gameObject.SetActive(true);

        // ī�޶��� -forward ������ ����
        transform.forward = -mainCamera.forward;
        // ���� �̵��Ѵ� (�̵� ����)
        rig.AddForce(Vector3.up * defJumpPower, ForceMode.Impulse);

        // transform.position += Vector3.up * speed * Time.unscaledDeltaTime;
        // �̵��� Time.unscaledeltatime�� ���� (�ð��� �����ִ� ���¿��� �������� �Ҷ� deltatime��� ���)
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
            for (int i = 0; i < 50; i++) // 50 = 0.1�ʾ� �ݺ��� 5�ʵ��� �ҰŶ� 10*5��
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
        // isHipdrop�� true �϶�, quistion block�� �浹 ���¶��, reaction �ִϸ��̼��� �Ҵ�
        if (collision.gameObject.name.Contains("Question"))
        {
            //isJumping = false;
            ontheBox = true;
            force.force = new Vector3(0, -39.81f, 0);
        }

        else if (isHipdrop == true && collision.gameObject.name.Contains("Block"))
        {

            collision.gameObject.GetComponent<Block>().SetAnimDown();
            // reaction �ִϸ��̼��� �Ҵ�
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
        
        // ���� �÷��̾ ���� ����ִٸ�
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // ���� ����� ��, ���� ���� Ƚ���� �ִ� Ƚ������ ũ�ٸ�,
            if (jumpCurrCount >= jumpMaxCount)
            {
                // ���� Ƚ���� �ʱ�ȭ �Ѵ�
                jumpCurrCount = 0;
            }

            if (isHipdrop == true)
            {
                camerascripts.CameraShake();
                PoundEffect();
            }
            // ���� ���� ������ �� ��
            if (isHipdrop && collision.gameObject.name.Contains("HipDrop"))
            {
                // �������� ����
                collision.transform.parent.gameObject.SetActive(false);
                // shake �Լ� ȣ��?
                camerascripts.CameraShake();

            }

            // collision���� �޾ƿ� �΋H�� ��ġ�� y ���� �÷��̾��� y������ ������ 
            if (collision.contacts[0].point.y < transform.position.y)
            {
                //�̰� ���̴�
                // ���϶� �ؾ��ϴ� ��
                ontheBox = false;
                isJumping = false;
                speed = defSpeed;
                isHipdropGround = true;
                isHipdrop = false;
                isDashing = false;
                isWall = false;
                isWallJumping = false;
                force.force = new Vector3(0, -39.81f, 0);
                // �ִϸ��̼�
                //animator.SetBool("Wait", true);

                // �����ִϸ��̼� ����
                animator.SetBool("Jump", false);
                // ���� 2 �ִϸ��̼�
                animator.SetBool("Jump2", false);
                // �ִϸ��̼�
                animator.SetBool("Jump3", false);
                // �ִϸ��̼�
                animator.SetBool("HipDrop", false);
                animator.SetBool("WallJump", false);
                animator.SetBool("Reaction", false);
            }

            // collision���� ���� y ���� �÷��̾��� ������ ũ�ų� ������
            else if (collision.contacts[0].point.y >= transform.position.y)
            {
                //�̰� ���̴�
                isWall = true;
                // ���϶� �ؾ��� ��
                // �����ϱ�
                isJumping = false;
                isWallJumping = false;
                // ���� Ƚ���� �ʱ�ȭ
                jumpCurrCount = 0;

                
                // ���� 2 �ִϸ��̼�
                animator.SetBool("Jump2", false);
                // �ִϸ��̼�
                animator.SetBool("Jump3", false);
                // �ִϸ��̼�
                animator.SetBool("HipDrop", false);

            }
        }
    }

}
