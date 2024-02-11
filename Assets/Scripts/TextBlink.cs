using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{

    float speed = 5f;
    TMP_Text text;
    Color textColor;

    bool isUp = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        textColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            textColor.a += Time.deltaTime * speed;
            if(textColor.a >= 1f)
            {
                isUp = false;
            }
        }
        else
        {
            textColor.a -= Time.deltaTime * speed;
            if (textColor.a <= 0f)
            {
                isUp = true;
            }
        }
        text.color = textColor;
    }
}
