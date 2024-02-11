using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyStart : MonoBehaviour
{
    // �¾��, ��� ��ġ�� ��ġ�� ������(Scene���� ��ġ)
    // �����ϰ� 2�� �Ŀ� ���� ������ ����(�Ʒ�, ��)���� �����Ѵ�
    // �����ϴ� ���� ��� ȸ���Ѵ�
    // ���� �� ���� ������,
    // ȸ���� �����
    // ĳ���� setActive�� false�� �ϰ�
    // �������� setActive�� ture�� �Ѵ�
    // �̶� start �ִϸ��̼��� �����Ѵ�(triger)

    // ���� �ð�
    public float currTime = 0;
    // ���� �ð�
    public float startTime = 1f;
    // �ִϸ��̼� ������ �ð�
    public float animEndTime = 2f;

    Animator anim;

    Rigidbody rig;

    public GameObject Player;
    

    // Start is called before the first frame update
    void Start()
    {
        //������ٵ� ��������
        rig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;

        if (currTime >= animEndTime)
        {
            // �������� Ų��
            Player.gameObject.SetActive(true);
            // ���� ����
            this.gameObject.SetActive(false);

        }

        //// �ִϸ��̼��� ������ 
        //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 1)
        //{
        //    // �������� Ų��
        //    Player.gameObject.SetActive(true);
        //    // �� �� ����
        //    this.gameObject.SetActive(false);
        //}

    }

   
}
