using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private float maxY = 1f;
    private float minY = -1f;
    private float speed = 1f;
    private float rotateSpeed = 30f;

    private float curTime = 0f;
    private float coinTime = 0.5f;

    private bool isUp = true;

    private Vector3 originPos = Vector3.zero;


    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        UpDown();
        AutoRatoate();
    }

    private void UpDown()
    {
        if (isUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
            if (transform.position.y >= originPos.y + maxY)
            {
                isUp = !isUp;
            }
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            if (transform.position.y <= originPos.y + minY)
            {
                isUp = !isUp;
            }
        }
    }

    private void AutoRatoate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void GetCoin()
    {
        GameManager.instance.ActiveGetCoin(transform, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Invoke("GetCoin", 0.5f);
            Invoke("GetCoin", 0.5f);
            Invoke("GetCoin", 0.5f);
            gameObject.SetActive(false);
        }
    }
}
