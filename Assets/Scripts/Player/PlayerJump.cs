using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Collider co;
    Rigidbody rig;

    // ���ǵ� (��ĥ�� �����ϱ�
    public float speed = 5f;
    // �����Ŀ�
    public float jumpPower = 8;

    // �ִ� ���� Ƚ��
    public int jumpMaxCount = 3;
    // ���� ���� Ƚ��
    public int jumpCurrCount = 0;

    // ���� ���� �ð�
    public float jumpCurrTime = 0;
    // ���� �ִ� �ð�
    // float jumpMaxTime = 2f;
    
    // ���� ���� Ȯ��
    public bool isJumping = false;

    // ���� ����
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        //�ݶ��̴��� ��������
        co = GetComponent<Collider>();

        //������ٵ� ��������
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ���� �ϴ� ������ Ȯ���Ѵ�
        // = isjumping = false�϶�
        // ���� �÷��̾ �����ϴ� ���� �ƴ϶��
        if (isJumping == false)
        {
            // ���� �ð��� ���
            jumpCurrTime += Time.deltaTime;


            // ����ڰ� �����̽� �ٸ� �������� (���� ����ڰ� �����̽� �ٸ� �����ٸ�)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ���� Ƚ���� ����
                jumpCurrCount++;
                print("is jumping");
                // ���� ������ �ٲ۴�
                isJumping = true;

                // 1�� ����
                // ���� ���� Ƚ���� 1�̶��
                if (jumpCurrCount == 1)
                {
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 1�� ���� = �⺻ ����
                    print("jump1");
                    // ������ ������ ���Ѵ�
                    dir = transform.up;
                    // 1�� ������ �Ѵ�
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);

                }

                // 2�� ���� 
                // ���� 1�� ����= count = 1 �� , n�ʰ� ��������,
                else if (jumpCurrCount == 2 && jumpCurrTime <= 1)
                {
                    // �����̽� �ٸ� ������
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 2�� ������ �Ѵ�
                    print("jump2");
                    // ������ ������ ���Ѵ�
                    dir = transform.up;
                    // 2�� ������ �Ѵ�
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);
                    // 2�� ���� = �ִϸ��̼��� �ٲ�

                }

                // 3�� ����
                // ���� 2�� ���� �� n�ʰ� ��������,
                else if (jumpCurrCount == 3 && jumpCurrTime <= 1)
                {
                    // �����̽� �ٸ� ������
                    // ���� �ð��� 0����
                    jumpCurrTime = 0;
                    // 3�� ������ �Ѵ�
                    print("jump3");
                    
                    // ������ ������ ���Ѵ�
                    dir = transform.up;
                    // ���ǵ带 �������Ѵ�
                    speed = 10f;
                    // 3�� ������ �Ѵ�
                    rig.AddForce(dir * jumpPower, ForceMode.Impulse);

                }


            }

            else if (jumpCurrTime >= 1)
            {
                // ���� Ƚ���� �ʱ�ȭ �Ѵ�
                jumpCurrCount = 0;
            }

        }

        // ���� �÷��̾ ������ �� ��
        else if (isJumping == true)
        {
            // Ű���� ���� ��Ʈ�� Ű�� �����ٸ�
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                // ���� ���̿��� �����

                // ������ �ִϸ��̼��� �����Ѵ�

                // ������ ������ ��������

            }
        }
               

    }
    // ���� ������ ���� Ƚ���� �ʱ�ȭ �Ѵ�
    private void OnCollisionEnter(Collision collision)
    {
        // ���� �÷��̾ ���� ����ִٸ�
        if(collision.gameObject.CompareTag("Ground"))
        {
            print("Ground");
            isJumping = false;
            speed = 5f;

            // ���� ����� ��, ���� ���� Ƚ���� �ִ� Ƚ������ ũ�ٸ�,
            if (jumpCurrCount >= jumpMaxCount)
            {
                // ���� Ƚ���� �ʱ�ȭ �Ѵ�
                jumpCurrCount = 0;
            }

        }
    }

}
