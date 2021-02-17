using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect_01 : MonoBehaviour
{
    public float fTime = 0f;
    public float ShowCycle = 0.15f;
    public GameObject BallForEffect;
    public Sprite Skin;
    public Ball ball;

    void FixedUpdate()
    {
        AfterImage();
    }

    public void AfterImage()
    {
        fTime += Time.fixedDeltaTime;

        if (fTime >= ShowCycle)
        {
            fTime -= ShowCycle;

            SpriteRenderer obj = Instantiate(BallForEffect).GetComponent<SpriteRenderer>();
            obj.sprite = Skin;
            obj.transform.SetParent(ball.GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
    }
}
