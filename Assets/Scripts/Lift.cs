using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    private float speed = 8f;

    private float maxX = -4530.5f; 
    private float minX = -4573f;
    private float curTime = 0f;
    private float waitTime = 1f;

    private bool isRight = false;
    private bool isWait = false;

    //private Transform originParent;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isWait)
        {
            curTime += Time.deltaTime;
            if (curTime >= waitTime)
            {
                curTime = 0f;
                isWait = false;
            }
        }
        else
        {
            if (isRight)
            {
                transform.position += transform.right * speed * Time.deltaTime;
                if(transform.localPosition.x >= maxX)
                {
                    isWait = true;
                    isRight = !isRight;
                }
            }
            else
            {
                transform.position -= transform.right * speed * Time.deltaTime;
                if(transform.localPosition.x <= minX)
                {
                    isWait = true;
                    isRight = !isRight;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //originParent = collision.transform.parent;
            collision.transform.parent= transform;
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
