using UnityEngine;

public class BlinkText : MonoBehaviour
{
    TMPro.TMP_Text T;
    bool isBright = false;
    public float BlinkSpeed = 1.5f;

    private void Awake()
    {
        T = GetComponent<TMPro.TMP_Text>();
        BlinkSpeed *= Time.smoothDeltaTime;
    }

    void Update()
    {
        BlinkAlpha(T, BlinkSpeed);
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
