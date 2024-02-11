using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoom_1 : MonoBehaviour
{
    private float speed = 20f;
    private float originY = 0f;
    private float maxY = 16f;
    private float curTime = 0f;
    private float downTime = 2f;

    private bool isUp = false;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Up();
        }
    }

    private void Up()
    {

        if (isUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            print("up");
            if(transform.position.y >= originY + maxY)
            {
                isUp = false;
                curTime = 0f;
            }
        }
        else
        {
            if(curTime < downTime)
            {
                curTime += Time.deltaTime;
                print("stay");
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                print("down");
                if(transform.position.y <= originY)
                {
                    isActive = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
        {
            isActive = true;
            isUp = true;
            print("mushroom bullet");
        }
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
