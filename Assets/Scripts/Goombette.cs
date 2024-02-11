using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goombette : MonoBehaviour
{
    public GameObject powerMoon;

    private void SetActiveTrue()
    {
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Invoke("SetActiveTrue", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.name.Contains("Goomba"))
        {
            // ����Ʈ
            powerMoon.SetActive(true);

            //  moon true �� ���ֱ�
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
