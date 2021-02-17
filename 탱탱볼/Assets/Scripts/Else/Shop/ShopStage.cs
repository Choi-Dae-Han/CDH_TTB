using UnityEngine;

public class ShopStage : MonoBehaviour
{
    public SpriteRenderer BackGround;
    public SpriteRenderer[] Grounds;
    public Shop shop;

    private void Awake()
    {
        BackGround.sprite = shop.BackGroundForStage;

        for (int i = 0; i < Grounds.Length; ++i)
        {
            Grounds[i].sprite = shop.GroundsForStage;
        }
    }
}
