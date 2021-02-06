using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearEffect : MonoBehaviour
{
    public SpriteRenderer SR;
    public float DisappearSpeed = 0f;

    private void Awake()
    {
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
