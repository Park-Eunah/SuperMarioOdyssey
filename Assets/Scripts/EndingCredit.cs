using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    private float speed = 100f;
    private float curTime = 0f;
    private readonly float minX = -800f;
    private readonly float endTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(transform.position.x > minX)
        {
            transform.position -= transform.right * speed * Time.unscaledDeltaTime;
        }
        else
        {
            curTime += Time.unscaledDeltaTime;
            if(curTime >= endTime)
            {
                GameManager.instance.ISSTART = false;
            }
        }

    }
}
