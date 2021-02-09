using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text Text_Skin;
    public Text Text_Effect;
    public Text Text_SoundEffect;

    public Sprite ShopBallSprite;
    public AudioClip ShopBallSE;
    public GameObject ShopBallEffect;
    public Image BGIofShopBallSprite;
    public Image BGIofShopBallSE;
    public Image BGIofShopBallEffect;

    private void Awake()
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
