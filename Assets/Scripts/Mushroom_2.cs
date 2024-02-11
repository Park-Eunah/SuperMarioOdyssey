using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_2 : MonoBehaviour
{
    private float speed = 20f;
    private float minY = 108f;
    private float maxY = 130f;
    private float curTime = 0f;
    private float upTime = 5f;
    private float downTime = 2f;
    private bool isCheckTiming = true;
    private bool isUp = false;          

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckTiming)
        {
            CheckTime();
        }
        else
        {
            MoveUpDown();
        }
    }

    private void CheckTime()
    {
        curTime += Time.deltaTime;

        if (isUp)
        {
            if(curTime >= downTime)
            {
                curTime = 0f;
                isUp = false;
                isCheckTiming = false;
            }
        }
        else
        {
            if(curTime >= upTime)
            {
                curTime = 0f;
                isUp = true;
                isCheckTiming = false;
            }
        }
    }

    private void MoveUpDown()
    {
        if (isUp)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            if(transform.position.y >= maxY)
            {
                isCheckTiming = true;
            }
        }
        else
        {
            transform.position -= transform.up * speed * Time.deltaTime;
            if(transform.position.y <= minY)
            {
                isCheckTiming = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
