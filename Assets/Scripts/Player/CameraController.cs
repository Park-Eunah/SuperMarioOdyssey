using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // ĳ���� �ٵ�
    public Transform marioBody;
    // ī�޶� �� (ī�޶� ȸ��)
    public Transform cameraArm;

    // �ִϸ��̼� ����
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // ĳ���Ϳ� �ִ� animaitor ������Ʈ�� ������ �����Ѵ�.
        animator = marioBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
    }

    void LookAround()
    {
        // ���콺 �Է��� �޾ƿ´� (x = ���콺 �¿�, y=���콺 ���Ʒ� ������)
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // ī�޶� ȸ�� ������ �޾ƿ´�
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        // ī�޶� ȸ���Ѵ� (���콺 ���� = �ٶ󺸴� ����/�ݴ�ε� �� �� ����)
        cameraArm.rotation = Quaternion.Euler(cameraAngle.x - mouseDelta.y, cameraAngle.y + mouseDelta.x, cameraAngle.z);
        

        // ī�޶� x������ �����Ѵ�
        float x = cameraAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        
        // cameraArm.eulerAngles = new Vector3(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }

    private void Move()
    {
        // ī�޶� �ٶ󺸴� ���� Ȯ��
        Debug.DrawRay(cameraArm.position,
        new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

    }
}
