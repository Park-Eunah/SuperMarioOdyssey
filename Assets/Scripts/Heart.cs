using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
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

    // 플레이어가 닿으면 life를 증가시켜줌.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.LifeCount(true);
            gameObject.SetActive(false);
        }
    }
}
