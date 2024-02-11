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

    // 플레이어가 닿으면 코인 증가시켜줌
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CoinPlus();
            gameObject.SetActive(false);
        }
    }
}
