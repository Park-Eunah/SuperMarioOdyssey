using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyRotate : MonoBehaviour
{
    // 회전 속도
    float rotSpeed = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //회전한다
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }
}
