using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmove : MonoBehaviour
{
    float x;
    float z;
    float speed = 5f;
    Vector3 dir = Vector3.zero;

    enum jumpState {nojump , jump1, jump2, jump3 }

    jumpState jump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        dir = new Vector3(x, 0, z);

        transform.Translate(dir.normalized * speed * Time.deltaTime);
        //transform.position += dir.normalized * speed * Time.deltaTime;

        if ( jump == jumpState.jump1)
        {

        }else if(jump == jumpState.jump2)
        {

        }
        else
        {

        }
    }
} 
