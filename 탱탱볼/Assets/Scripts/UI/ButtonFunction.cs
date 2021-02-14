using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;

public class ButtonFunction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum BUTTONSTATE
    {
        LOCK, SELECTEDLOCK, UNLOCK
    }
    public BUTTONSTATE buttonState = BUTTONSTATE.UNLOCK;

    public enum GOODSSTATE
    {
        UNSOLD, SOLD
    }
    public GOODSSTATE goodsState = GOODSSTATE.UNSOLD;

    public GameObject UsingLockImg;
    public GameObject UI;
    public GameObject UsingUI;
    public AudioClip OnMouseSE;
    public RawImage BGI;
    public Texture2D BGIofButton;
    public RectTransform ContentRT;
    public RectTransform StageListRT;
    public Sprite CoinImage;
    public Image[] CoinUIPos;
    public Stage stage;
    public Shop shop;
    AudioSource AM;
    GameManager GM;
    Vector2 ButtonScale = Vector2.zero;

    public GameObject TestApplyButton;
    public GameObject BuyButton;
    public GameObject ApplyButton;

    public Transform GoodsImgTR;
    public GameObject SoldImg;

    [SerializeField] private GameObject SellingEffect;
    [SerializeField] private GameObject SellingSE;
    [SerializeField] private GameObject SellingSkin;

    [SerializeField] private Text GoodsName;
    [SerializeField] private int MoneyPrice;
    [SerializeField] private int CoinPrice;

    private void Awake()
    {
        GameObject GM_Obj = GameObject.Find("GameManager");
        AM = GM_Obj.GetComponent<AudioSource>();
        GM = GM_Obj.GetComponent<GameManager>();
        ButtonScale = new Vector2(transform.localScale.x, transform.localScale.y);

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

        if (stage != null)
        {
            LoadButtonData();
            var data = stage.LoadStageData(stage.AreaName, stage.StageNum);
            if (data.Opened)
            {
                ChangeButtonState(BUTTONSTATE.UNLOCK);
                SaveButtonState(2);
            }

            for (int i = 0; i < data.GotCoins; ++i)
            {
                CoinUIPos[i].sprite = CoinImage;
            }
        }
        else
        {
            switch (buttonState)
            {
                case BUTTONSTATE.LOCK:
                    GetComponent<Button>().interactable = false;
                    break;
                case BUTTONSTATE.SELECTEDLOCK:
                    GetComponent<Button>().interactable = false;
                    transform.localScale = ButtonScale * 1.1f;
                    break;
                case BUTTONSTATE.UNLOCK:
                    break;
            }
        }
    }

    public void ChangeButtonState(BUTTONSTATE s)
    {
        buttonState = s;

        switch (s)
        {
            case BUTTONSTATE.LOCK:
                GetComponent<Button>().interactable = false;
                break;
            case BUTTONSTATE.SELECTEDLOCK:
                GetComponent<Button>().interactable = false;
                transform.localScale = ButtonScale * 1.1f;
                break;
            case BUTTONSTATE.UNLOCK:
                GetComponent<Button>().interactable = true;
                transform.localScale = ButtonScale;
                if (UsingLockImg != null) Destroy(UsingLockImg);
                break;
        }
    }

    public void ChangeGoodsStage(GOODSSTATE s)
    {
        if (goodsState == s) return;
        goodsState = s;

        switch(s)
        {
            case GOODSSTATE.UNSOLD:
                break;
            case GOODSSTATE.SOLD:
                ShowSoldImg();
                break;
        }
    }

    public void LoadNextStage()
    {
        GM.LoadStage(GM.PlayingStage.GetComponent<Stage>().NextStage);
    }

    public void UISound(AudioClip sound)
    {
        AM.PlayOneShot(sound);
    }

    public void ExitAppication()
    {
        Application.Quit();
    }

    public void RetryGameStage()
    {
        GM.RetryStage();
    }

    public void ShowShopList()
    {
        if (UsingUI == null)
        {
            ContentRT.GetChild(0).GetComponent<ChildUI>().ParentButton.GetComponent<ButtonFunction>()
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

    public void ExitGameStage()
    {
        GM.ExitStage();
    }

    public void ChangeGMState(int i)
    {
        GM.ChangeGameState((GameManager.GAMESTATE)i);
    }

    public void DeleteUI()
    {
        if (GM.UsingWhiteScreen != null) Destroy(GM.UsingWhiteScreen);
        Destroy(transform.parent.gameObject);  
    }

    public void ShowSoldImg()
    {
        GameObject obj = Instantiate(SoldImg);
        obj.transform.SetParent(GoodsImgTR);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    public void ShowUI()
    {
        if (UsingUI == null)
        {
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void ShowStageList()
    {
        if (UsingUI == null)
        {
            GM.ClearChild(StageListRT);
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(StageListRT);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;
        }
    }

    public void LoadStage()
    {
        if (stage != null)
            GM.LoadStage(stage.gameObject);
    }

    public void LoadShopStage()
    {
        GM.LoadShopStage(shop.BackGroundForStage,shop.GroundsForStage, shop.ShopBallSprite, shop.ShopBallSE, shop.ShopBallEffect);
    }

    public void PauseGame()
    {
        GM.Pause();
    }

    public void ResumeGame()
    {
        GM.Resume();
    }

    public void ChangeBackGround()
    {
        BGI.texture = BGIofButton;
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

    public void Apply()
    {
        Shop shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();
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
        Shop shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();
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

    public void BuyGoods()
    {
        Shop shop = GameObject.Find("Shop(Clone)").GetComponent<Shop>();
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
                GI.ParentButton.GetComponent<ButtonFunction>().ChangeGoodsStage(GOODSSTATE.SOLD);
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
                GI.ParentButton.GetComponent<ButtonFunction>().ChangeGoodsStage(GOODSSTATE.SOLD);
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
                GI.ParentButton.GetComponent<ButtonFunction>().ChangeGoodsStage(GOODSSTATE.SOLD);
            }
        }
    }

    public void ShowGoodsInfo()
    {
        if (UsingUI == null)
        {
            GM.CreateWhiteScreen();
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.localPosition = Vector3.zero;
            UsingUI.transform.localScale = Vector3.one;

            GoodsInfo GI = UsingUI.GetComponent<GoodsInfo>();

            switch (goodsState)
            {
                case GOODSSTATE.UNSOLD:
                    GameObject btn1 = Instantiate(TestApplyButton);
                    btn1.transform.SetParent(UsingUI.transform);
                    btn1.transform.position = GI.TestApplyButtonTR.position;
                    btn1.transform.localScale = Vector3.one;
                    GameObject btn2 = Instantiate(BuyButton);
                    btn2.transform.SetParent(UsingUI.transform);
                    btn2.transform.position = GI.BuyButtonTR.position;
                    btn2.transform.localScale = Vector3.one;
                    break;
                case GOODSSTATE.SOLD:
                    GameObject btn3 = Instantiate(ApplyButton);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonState == BUTTONSTATE.UNLOCK)
        {
            transform.localScale = ButtonScale * 1.1f;
            AM.PlayOneShot(OnMouseSE);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonState == BUTTONSTATE.UNLOCK)
        {
            transform.localScale = ButtonScale;
        }
    }

    public void SaveButtonState(int state)
    {
        ButtonData BD = new ButtonData(state);
        string jsonData = DataManager.ObjectToJson(BD);
        DataManager.CreateJsonFile(Application.dataPath, gameObject.name, "/JsonData/Button", jsonData);
    }

    public void LoadButtonData()
    {
        var data = DataManager.LoadJsonFile<ButtonData>(Application.dataPath, gameObject.name, "/JsonData/Button");
        buttonState = (BUTTONSTATE)data.buttonState;
    }
}
