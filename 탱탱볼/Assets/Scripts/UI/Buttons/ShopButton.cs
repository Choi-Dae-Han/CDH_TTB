using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : BasicButton
{
    public enum GOODSSTATE
    {
        UNSOLD, SOLD
    }
    public GOODSSTATE goodsState = GOODSSTATE.UNSOLD;

    Shop shop;
    public Transform GoodsImgTR;
    public RectTransform ContentRT;
    [SerializeField] private GameObject SellingEffect;
    [SerializeField] private GameObject SellingSE;
    [SerializeField] private GameObject SellingSkin;
    [SerializeField] private Text GoodsName;
    [SerializeField] private int MoneyPrice;
    [SerializeField] private int CoinPrice;

    private new void Awake()
    {
        base.Awake();
        shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();

        if (SellingEffect != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, SellingEffect.GetComponent<EffectGoods>().NameForData, "/JsonData/Goods/Effect/");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
        if (SellingSE != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, SellingSE.GetComponent<SoundEffectGoods>().NameForData, "/JsonData/Goods/SoundEffect/");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
        if (SellingSkin != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, SellingSkin.GetComponent<SkinGoods>().NameForData, "/JsonData/Goods/Skin/");
            if (data.IsHaving) ChangeGoodsStage(GOODSSTATE.SOLD);
        }
    }

    public void ChangeGoodsStage(GOODSSTATE s)
    {
        if (goodsState == s) return;
        goodsState = s;

        switch (s)
        {
            case GOODSSTATE.UNSOLD:
                break;
            case GOODSSTATE.SOLD:
                ShowSoldImg();
                break;
        }
    }

    public void ShowShopList()
    {
        if (UsingUI == null)
        {
            ContentRT.GetChild(0).GetComponent<ChildUI>().ParentButton.GetComponent<BasicButton>()
                .ChangeButtonState(BUTTONSTATE.UNLOCK);
            ChangeButtonState(BUTTONSTATE.SELECTEDLOCK);
            GM.ClearChild(ContentRT);
            ContentRT.transform.localPosition = Vector3.zero;
            UsingUI = Instantiate(UI);
            UsingUI.GetComponent<ChildUI>().ParentButton = gameObject;
            UsingUI.transform.SetParent(ContentRT);
            UsingUI.transform.localPosition = ContentRT.rect.center;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void ShowGoodsInfo()
    {
        if (UsingUI == null)
        {
            GM.UsingWhiteScreen = GM.CreateUI(GM.WhiteScreen, Vector3.zero);
            GM.UsingWhiteScreen.GetComponent<RectTransform>().sizeDelta = GM.MainScreenTr.sizeDelta;
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;

            GoodsInfo GI = UsingUI.GetComponent<GoodsInfo>();

            switch (goodsState)
            {
                case GOODSSTATE.UNSOLD:
                    GameObject btn1 = Instantiate(shop.TestApplyButton);
                    btn1.transform.SetParent(UsingUI.transform);
                    btn1.transform.position = GI.TestApplyButtonTR.position;
                    btn1.transform.localScale = Vector3.one;
                    GameObject btn2 = Instantiate(shop.BuyButton);
                    btn2.transform.SetParent(UsingUI.transform);
                    btn2.transform.position = GI.BuyButtonTR.position;
                    btn2.transform.localScale = Vector3.one;
                    break;
                case GOODSSTATE.SOLD:
                    GameObject btn3 = Instantiate(shop.ApplyButton);
                    btn3.transform.SetParent(UsingUI.transform);
                    btn3.transform.position = GI.ApplyButtonTR.position;
                    btn3.transform.localScale = Vector3.one;
                    break;
            }

            GI.ParentButton = gameObject;
            GI.GoodsName.text = GoodsName.text;
            GI.GoodsImage.texture = gameObject.transform.GetChild(0).GetComponent<RawImage>().texture;
            GI.CoinPrice.text = CoinPrice + "";
            GI.MoneyPrice.text = MoneyPrice + "";

            if (SellingSkin != null) GI.SellingSkin = SellingSkin;
            else if (SellingEffect != null) GI.SellingEffect = SellingEffect;
            else if (SellingSE != null) GI.SellingSE = SellingSE;
        }
    }

    public void BuyGoods()
    {
        GoodsInfo GI = transform.parent.GetComponent<GoodsInfo>();
        if (GI.SellingSkin != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, GI.SellingSkin.name, "/JsonData/Goods/Skin/");
            if (!data.IsHaving)
            {
                SkinGoods SG = GI.SellingSkin.GetComponent<SkinGoods>();
                SG.SaveGoodsData(true);
                ChangeBallSkin(SG.SellingSkin);
                shop.ShopBallSprite = SG.SellingSkin;
                shop.Text_Skin.text = GI.SellingSkin.name;
                shop.BGIofShopBallSprite.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
            }
        }
        else if (GI.SellingEffect != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, GI.SellingEffect.name, "/JsonData/Goods/Effect/");
            if (!data.IsHaving)
            {
                EffectGoods EG = GI.SellingEffect.GetComponent<EffectGoods>();
                EG.SaveGoodsData(true);
                ChangeBallEffect(EG.SellingEffect);
                shop.ShopBallEffect = EG.SellingEffect;
                shop.Text_Effect.text = GI.SellingEffect.name;
                shop.BGIofShopBallEffect.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
            }
        }
        else if (GI.SellingSE != null)
        {
            var data = DataManager.LoadJsonFile<GoodsData>(
                Application.dataPath, GI.SellingSE.name, "/JsonData/Goods/SoundEffect/");
            if (!data.IsHaving)
            {
                SoundEffectGoods SEG = GI.SellingSE.GetComponent<SoundEffectGoods>();
                SEG.SaveGoodsData(true);
                ChangeBallSE(SEG.SellingSE);
                shop.ShopBallSE = SEG.SellingSE;
                shop.Text_SoundEffect.text = GI.SellingSE.name;
                shop.BGIofShopBallSE.color = Color.white;
                GI.ParentButton.GetComponent<ShopButton>().ChangeGoodsStage(GOODSSTATE.SOLD);
            }
        }
    }

    public void Apply()
    {
        GoodsInfo GI = transform.parent.GetComponent<GoodsInfo>();
        var data = DataManager.LoadJsonFile<BallData>(
            Application.dataPath, "BallData", "/JsonData/Player/");

        if (GI.SellingSkin != null)
        {
            data.Skin = GI.SellingSkin.GetComponent<SkinGoods>().SellingSkin;
            shop.ShopBallSprite = GI.SellingSkin.GetComponent<SkinGoods>().SellingSkin;
            shop.Text_Skin.text = GI.SellingSkin.name;
            shop.BGIofShopBallSprite.color = Color.white;
        }
        else if (GI.SellingEffect != null)
        {
            data.Effect = GI.SellingEffect.GetComponent<EffectGoods>().SellingEffect;
            shop.ShopBallEffect = GI.SellingEffect.GetComponent<EffectGoods>().SellingEffect;
            shop.Text_Effect.text = GI.SellingEffect.name;
            shop.BGIofShopBallEffect.color = Color.white;
        }
        else if (GI.SellingSE != null)
        {
            data.SE = GI.SellingSE.GetComponent<SoundEffectGoods>().SellingSE;
            shop.ShopBallSE = GI.SellingSE.GetComponent<SoundEffectGoods>().SellingSE;
            shop.Text_SoundEffect.text = GI.SellingSE.name;
            shop.BGIofShopBallSE.color = Color.white;
        }

        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player", jsonData);
    }

    public void TestApply()
    {
        GoodsInfo GI = transform.parent.GetComponent<GoodsInfo>();
        if (GI.SellingSkin != null)
        {
            shop.ShopBallSprite = GI.SellingSkin.GetComponent<SkinGoods>().SellingSkin;
            shop.Text_Skin.text = GI.SellingSkin.name;
            shop.BGIofShopBallSprite.color = Color.blue;
        }
        else if (GI.SellingEffect != null)
        {
            shop.ShopBallEffect = GI.SellingEffect.GetComponent<EffectGoods>().SellingEffect;
            shop.Text_Effect.text = GI.SellingEffect.name;
            shop.BGIofShopBallEffect.color = Color.blue;
        }
        else if (GI.SellingSE != null)
        {
            shop.ShopBallSE = GI.SellingSE.GetComponent<SoundEffectGoods>().SellingSE;
            shop.Text_SoundEffect.text = GI.SellingSE.name;
            shop.BGIofShopBallSE.color = Color.blue;
        }
    }

    public void ResetApply()
    {
        Shop shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();
        var data = DataManager.LoadJsonFile<BallData>(Application.dataPath, "BallData", "/JsonData/Player/");

        if (data.Skin != null)
        {
            shop.ShopBallSprite = data.Skin;
            shop.Text_Skin.text = data.Skin.name;
            shop.BGIofShopBallSprite.color = Color.white;
        }
        if (data.Effect != null)
        {
            shop.ShopBallEffect = data.Effect;
            shop.Text_Effect.text = data.Effect.name;
            shop.BGIofShopBallEffect.color = Color.white;
        }
        else
        {
            shop.ShopBallEffect = null;
            shop.Text_Effect.text = "기본";
            shop.BGIofShopBallEffect.color = Color.white;
        }
        if (data.SE != null)
        {
            shop.ShopBallSE = data.SE;
            shop.Text_SoundEffect.text = data.SE.name;
            shop.BGIofShopBallSE.color = Color.white;
        }
    }

    public void ChangeBallEffect(GameObject effect)
    {
        var data = DataManager.LoadJsonFile<BallData>(Application.dataPath, "BallData", "/JsonData/Player/");
        data.Effect = effect;
        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player/", jsonData);
    }

    public void ChangeBallSkin(Sprite skin)
    {
        var data = DataManager.LoadJsonFile<BallData>(Application.dataPath, "BallData", "/JsonData/Player/");
        data.Skin = skin;
        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player/", jsonData);
    }

    public void ChangeBallSE(AudioClip se)
    {
        var data = DataManager.LoadJsonFile<BallData>(Application.dataPath, "BallData", "/JsonData/Player/");
        data.SE = se;
        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player/", jsonData);
    }

    public void ShowSoldImg()
    {
        GameObject obj = Instantiate(shop.SoldImg);
        obj.transform.SetParent(GoodsImgTR);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    public void LoadShopStage()
    {
        GM.shopStage.GetComponent<ShopStage>().shop = shop;
        GM.Ball_Obj_Shop.GetComponent<Ball>().shop = shop;
        ChangeGMState(4);
    }
}
