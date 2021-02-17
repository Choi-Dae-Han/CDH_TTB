using UnityEngine;

public class ShopDropDown : MonoBehaviour
{
    public Sprite[] BackGround;
    public Sprite[] Grounds;
    TMPro.TMP_Dropdown Dd;
    public Shop shop;

    private void Awake()
    {
        Dd = GetComponent<TMPro.TMP_Dropdown>();
        shop.BackGroundForStage = BackGround[0];
        shop.GroundsForStage = Grounds[0];
    }

    public void ChangeShopStageSprite()
    {
        shop.BackGroundForStage = BackGround[Dd.value];
        shop.GroundsForStage = Grounds[Dd.value];
    }
}
