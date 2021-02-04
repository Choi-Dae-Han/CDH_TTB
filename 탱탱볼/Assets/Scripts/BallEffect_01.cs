using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect_01 : MonoBehaviour
{
    public float DisappearSpeed = 0f;
    SpriteRenderer SR;

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        SR.color -= new Color(0f, 0f, 0f, 0.8f);
        DisappearSpeed *= Time.fixedDeltaTime;
    }

    void Update()
    {
        Disappear();
    }

    void Disappear()
    {
        SR.color -= new Color(0f, 0f, 0f, DisappearSpeed);

        if (SR.color.a <= 0f) Destroy(gameObject);
    }
}
