using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour
{
    private float speed = 30f;
    private float originY = 0f;
    private float maxY = 5f;

    private void OnEnable()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y >= originY + maxY)
        {
            gameObject.SetActive(false);
        }
    }
}
