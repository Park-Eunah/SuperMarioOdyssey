using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float rotateSpeed = 180f;

    void Update()
    {
        AutoRotate();
    }

    private void AutoRotate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    // �÷��̾ ������ ���� ����������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CoinPlus();
            gameObject.SetActive(false);
        }
    }
}
