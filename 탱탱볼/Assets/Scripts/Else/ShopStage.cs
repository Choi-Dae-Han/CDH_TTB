using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStage : MonoBehaviour
{
    public Sprite GounrdSkinToApply;
    public Sprite BackGroundToApply;
    public SpriteRenderer BackGround;
    public SpriteRenderer[] Grounds;

    private void Awake()
    {
        BackGround.sprite = BackGroundToApply;

        for (int i = 0; i < Grounds.Length; ++i)
        {
            Grounds[i].sprite = GounrdSkinToApply;
        }
    }
}
