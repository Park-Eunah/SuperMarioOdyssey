using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturing : MonoBehaviour
{
    // ĳ�Ǹ� ��ȯ
    public GameObject Cappy;
    // ������ �ð�
    public float ThrowTime = 1f;
    // ������ �Ÿ�
    public float ThrowDistance = 3f;
    // �ǵ��ƿ���
    bool Return;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ����ڰ� ������ ���콺�� ������, return�� false�̸�
        if (Input.GetButtonDown("Fire1") && Return == false)
        {
            CapThrow();
        }
    }

    void CapThrow()
    {
        // ĳ�� ��ġ�� �θ� null������ �Ѵ�?
        Cappy.transform.SetParent(null);
        // ������ ���Ѵ�
        Vector3 Forward = transform.forward;
        // �� �������� ������ �Ÿ�, �ð���ŭ �����δ� DOTween ���(�ص��ǳ�???)
        // Cappy.transform.DOBle
    }
}
