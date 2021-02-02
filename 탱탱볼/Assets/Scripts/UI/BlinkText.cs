using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    TMPro.TMP_Text T;
    bool isBright = false;

    void Start()
    {
        T = GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        BlinkAlpha(T, 1.5f * Time.smoothDeltaTime);
    }

    void BlinkAlpha(TMPro.TMP_Text text, float speed)
    {
        if (isBright == true)
        {
            text.alpha += speed;
            if (text.alpha >= 1f) isBright = false;
        }
        else
        {
            text.alpha -= speed;
            if (text.alpha <= 0f) isBright = true;
        }
    }
}
