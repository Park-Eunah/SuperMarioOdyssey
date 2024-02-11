using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // 캐릭터 바디
    public Transform marioBody;
    // 카메라 팔 (카메라 회전)
    public Transform cameraArm;

    // 애니메이션 변수
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // 캐릭터에 있는 animaitor 컴포넌트를 가져와 저장한다.
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
        // 마우스 입력을 받아온다 (x = 마우스 좌우, y=마우스 위아래 움직임)
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // 카메라 회전 각도를 받아온다
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        // 카메라를 회전한다 (마우스 방향 = 바라보는 방향/반대로도 할 수 있음)
        cameraArm.rotation = Quaternion.Euler(cameraAngle.x - mouseDelta.y, cameraAngle.y + mouseDelta.x, cameraAngle.z);
        

        // 카메라 x각도를 제한한다
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
        // 카메라가 바라보는 방향 확인
        Debug.DrawRay(cameraArm.position,
        new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

    }
}
