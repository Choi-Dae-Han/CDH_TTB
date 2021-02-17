using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text Text_Skin;
    public Text Text_Effect;
    public Text Text_SoundEffect;

    public GameObject TestApplyButton;
    public GameObject BuyButton;
    public GameObject ApplyButton;

    public Sprite ShopBallSprite;
    public AudioClip ShopBallSE;
    public GameObject ShopBallEffect;
    public Image BGIofShopBallSprite;
    public Image BGIofShopBallSE;
    public Image BGIofShopBallEffect;
    public GameObject SoldImg;

    public Sprite BackGroundForStage;
    public Sprite GroundsForStage;

    private void Awake()
    {
        LoadBallData();
    }

    void LoadBallData()
    {
        var data = DataManager.LoadJsonFile<BallData>(Application.dataPath, "BallData", "/JsonData/Player/");
        if (data.Skin != null)
        {
            ShopBallSprite = data.Skin;
            Text_Skin.text = data.Skin.name;
        }
        if (data.SE != null)
        {
            ShopBallSE = data.SE;
            Text_SoundEffect.text = data.SE.name;
        }
        if (data.Effect != null)
        {
            ShopBallEffect = data.Effect;
            Text_Effect.text = data.Effect.name;
        }
    }
}
