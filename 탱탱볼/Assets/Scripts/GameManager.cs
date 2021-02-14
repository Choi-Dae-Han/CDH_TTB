using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        CREATE, TITLEMENU, STAGEMENU, SHOP, SHOPSTAGE, PLAYING, CLEAR
    }
    public GAMESTATE gamestate = GAMESTATE.CREATE;

    public float fVolume = 1f;
    public float fResetTime = 2f;
    public float fSec = 0f;
    public int nSec = 0;
    public int nMin = 0;
    public int Coins = 0;

    public GameObject Ball_Obj;
    public GameObject Ball_Obj_Shop;
    public GameObject PauseUI;
    public GameObject BackButtonUI;
    public GameObject ClearUI;
    public GameObject PauseButtonUI;
    public GameObject TimeUI;
    public GameObject UsingUI;
    public GameObject TitleMenu;
    public GameObject StageMenu;
    public GameObject ShopMenu;
    public GameObject UsingShopMenu;
    public GameObject WhiteScreen;
    public GameObject UsingWhiteScreen;
    public GameObject PlayingStage;
    public GameObject ShopStage;
    public GameObject OnStage;
    public RectTransform MainScreenTr;
    public Transform MainCameraTr;
    public Transform ObjectUIScreenTr;
    public Vector3 ResetCameraPos = new Vector3(0f, 0f, -100f);
    public Camera camera1;
    public Transform StageTR;
    public Text TimeText;

    public int nScore = 0;
    public int nAddScoreObj = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        camera1 = MainCameraTr.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        GameStateProcess();
    }

    void GameStateProcess()
    {
        switch(gamestate)
        {
            case GAMESTATE.CREATE:
                break;
            case GAMESTATE.TITLEMENU:
                break;
            case GAMESTATE.STAGEMENU:
                break;
            case GAMESTATE.SHOP:
                break;
            case GAMESTATE.SHOPSTAGE:
                break;
            case GAMESTATE.PLAYING:
                CountTime(); // Test에서는 주석처리
                break;
            case GAMESTATE.CLEAR:
                break;
        }
    }

    public void ChangeGameState(GAMESTATE s)
    {
        if (gamestate == s) return;
        gamestate = s;

        switch (s)
        {
            case GAMESTATE.CREATE:
                break;
            case GAMESTATE.TITLEMENU:
                ClearChild(MainScreenTr);
                GameObject.Find("UI Camera").GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
                CreateUI(TitleMenu, Vector3.zero);
                break;
            case GAMESTATE.STAGEMENU:
                ClearChild(MainScreenTr);
                CreateUI(StageMenu, Vector3.zero);
                break;
            case GAMESTATE.SHOP:
                StopAllCoroutines();
                ClearChild(MainCameraTr);
                Ball shopBall = Ball_Obj_Shop.GetComponent<Ball>();
                shopBall.SR.sprite = null;
                shopBall.BallEffect = null;
                shopBall.BounceSound = null;
                if (GameObject.Find("Ball_Shop(Clone)") != null)
                    GameObject.Find("Ball_Shop(Clone)").tag = "Untagged";
                ClearChild(StageTR);
                ClearChild(MainScreenTr);
                if (UsingShopMenu == null) UsingShopMenu = CreateUI(ShopMenu, Vector3.zero);
                else UsingShopMenu.transform.SetParent(MainScreenTr);
                MainCameraTr.position = ResetCameraPos;
                MainCameraTr.gameObject.GetComponent<FollowCamera>().GoingTarget = ResetCameraPos;
                break;
            case GAMESTATE.SHOPSTAGE:
                UsingShopMenu.transform.SetParent(null);
                ClearChild(MainScreenTr);
                break;
            case GAMESTATE.PLAYING:
                ClearChild(MainScreenTr);
                ClearChild(StageTR);
                ClearChild(MainCameraTr);
                StopAllCoroutines();
                break;
            case GAMESTATE.CLEAR:
                ClearChild(MainScreenTr);
                break;
        }
    }

    public void CountTime()
    {
        fSec += Time.smoothDeltaTime;
        nSec = (int)fSec;
        if (fSec >= 60f)
        {
            nSec = 0;
            fSec -= 60f;
            ++nMin;
        }

        if(TimeText != null)
            TimeText.text = nMin + " : " + nSec;
    }

    public void Pause()
    {
        if (UsingUI == null)
        {
            CreateWhiteScreen();
            if (gamestate == GAMESTATE.PLAYING)
                Time.timeScale = 0.0f;
            UsingUI = CreateUI(PauseUI, Vector3.zero);
        }
    }

    public void Resume()
    {
        if (UsingWhiteScreen != null)
            Destroy(UsingWhiteScreen);
        if (gamestate == GAMESTATE.PLAYING)
            Time.timeScale = 1.0f;
    }

    public void RetryStage()
    {
        ClearChild(MainScreenTr);
        StopAllCoroutines();
        if (GameObject.Find("Ball(Clone)") != null)
            GameObject.Find("Ball(Clone)").tag = "Untagged";
        ClearChild(StageTR);
        ChangeGameState(GAMESTATE.PLAYING);
        GameObject Temp = CreateUI(PauseButtonUI, Vector3.zero);
        GameObject Temp1 = CreateUI(TimeUI, Vector3.zero);
        Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-130f, -80f);
        Temp1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        TimeText = Temp1.GetComponent<Text>();
        ClearChild(MainCameraTr);
        nAddScoreObj = 0;
        OnStage = Instantiate(PlayingStage);
        OnStage.transform.SetParent(StageTR);
        OnStage.transform.localPosition = Vector3.zero;
        CreateObject(Ball_Obj, Vector3.zero, StageTR);      
        ResetTime();
        Resume();
    }

    public void ResetTime()
    {
        nSec = 0;
        fSec = 0f;
        nMin = 0;
    }

    public void ExitStage()
    {
        if (GameObject.Find("Ball(Clone)") != null)
            GameObject.Find("Ball(Clone)").tag = "Untagged";
        ClearChild(StageTR);
        ClearChild(MainCameraTr);
        PlayingStage = null;
        TimeText = null;
        nAddScoreObj = 0;
        MainCameraTr.position = new Vector3(0f, 0f, -100f);
        MainCameraTr.gameObject.GetComponent<FollowCamera>().GoingTarget = new Vector3(0f, 0f, -100f);
        ResetTime();
        Time.timeScale = 1.0f; 
        ChangeGameState(GAMESTATE.STAGEMENU);
    }

    public void ClearChild(Transform tr)
    {
        int ChildCount = tr.childCount;
        for (int i = 0; i < ChildCount; ++i)
        {
            Destroy(tr.GetChild(i).gameObject);
        }
    }

    public GameObject CreateObject(GameObject obj, Vector3 pos, Transform parentTR = null)
    {
        GameObject Temp = Instantiate(obj);
        if (parentTR != null)
            Temp.transform.SetParent(parentTR);
        Temp.transform.localScale = Vector3.one;
        Temp.transform.position = pos;

        return Temp;
    }

    public GameObject CreateUI(GameObject obj, Vector3 pos)
    {
        GameObject Temp = Instantiate(obj);
        Temp.transform.SetParent(MainScreenTr);
        Temp.transform.localScale = Vector3.one;
        Temp.transform.localPosition = pos;

        return Temp;
    }

    public void CreateWhiteScreen()
    {
        if (UsingWhiteScreen == null)
        {
            UsingWhiteScreen = Instantiate(WhiteScreen);
            UsingWhiteScreen.transform.SetParent(MainScreenTr);
            UsingWhiteScreen.GetComponent<RectTransform>().sizeDelta = MainScreenTr.sizeDelta;
            UsingWhiteScreen.transform.localPosition = Vector3.zero;
            UsingWhiteScreen.transform.localScale = Vector3.one;
        }
    }

    public void StageClear(ClearObj clearObj)
    {
        ChangeGameState(GAMESTATE.CLEAR);
        MainCameraTr.gameObject.GetComponent<FollowCamera>().ChangeState(FollowCamera.CAMERASTATE.IDLE);

        Stage s = OnStage.GetComponent<Stage>();

        s.Opened = true;
        s.Cleared = true;
        nScore = 0;
        int tempScore = 1;
        if (nAddScoreObj == 0) tempScore++;
        if (nMin * 60 + fSec < s.fLimitTime) tempScore++;
        if (tempScore > s.GotCoins)
        {
            nScore = tempScore - s.GotCoins;
            s.GotCoins = tempScore;
            s.SaveStageData(s.Opened, s.GotCoins, s.Cleared);

            var data = DataManager.LoadJsonFile<PlayerData>(Application.dataPath, "PlayerData", "/JsonData/Player/");
            data.OwnedCoin += nScore;
            string jsonData = DataManager.ObjectToJson(data);
            DataManager.CreateJsonFile(Application.dataPath, "PlayerData", "/JsonData/Player/", jsonData);
        }
        s.LoadStageData(s.AreaName, s.StageNum);
        if (s.NextStage != null)
            s.OpenNextStage();
        if (s.NextAreaName != "")
            s.OpenNextArea();

        clearObj.ChangeState(ClearObj.STATE.MOVE);
    }

    public void LoadStage(GameObject stage)
    {
        PlayingStage = stage;

        if (PlayingStage != null)
        {
            ResetTime();
            MainCameraTr.position = ResetCameraPos;
            MainCameraTr.gameObject.GetComponent<FollowCamera>().GoingTarget = ResetCameraPos;
            if (GameObject.Find("Ball(Clone)") != null)
                GameObject.Find("Ball(Clone)").tag = "Untagged";
            ChangeGameState(GAMESTATE.PLAYING);
            GameObject Temp = CreateUI(PauseButtonUI, Vector3.zero);
            GameObject Temp1 = CreateUI(TimeUI, Vector3.zero);
            Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-130f, -80f);
            Temp1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
            TimeText = Temp1.GetComponent<Text>();
            OnStage = Instantiate(PlayingStage);
            OnStage.transform.SetParent(StageTR);
            OnStage.transform.localPosition = Vector3.zero;
            CreateObject(Ball_Obj, Vector3.zero, StageTR);
        }
    }

    public void LoadShopStage(Sprite backGround, Sprite grounds, Sprite skin, AudioClip se, GameObject effect = null)
    {
        ChangeGameState(GAMESTATE.SHOPSTAGE);
        GameObject Temp = CreateUI(BackButtonUI, Vector3.zero);
        Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f, -130f);
        ShopStage SS = ShopStage.GetComponent<ShopStage>();
        SS.BackGroundToApply = backGround;
        SS.GounrdSkinToApply = grounds;
        OnStage = Instantiate(ShopStage);
        OnStage.transform.SetParent(StageTR);
        OnStage.transform.localPosition = Vector3.zero;

        Ball shopBall = Ball_Obj_Shop.GetComponent<Ball>();
        shopBall.SR.sprite = skin;
        shopBall.BounceSound = se;
        shopBall.BallEffect = effect; // 수정해야함
        CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
    }

    public void ChangeVolume(float soundScale)
    {
        GetComponent<AudioSource>().volume = soundScale;
    }

    public void DelayAndReset()
    {
        StartCoroutine(DelayAndReset_Cor());
    }

    IEnumerator DelayAndReset_Cor()
    {
        if (gamestate != GAMESTATE.PLAYING &&
            gamestate != GAMESTATE.SHOPSTAGE)
            yield break;

        float TempTime = 0f;
        while(true)
        {
            TempTime += Time.smoothDeltaTime;
            if (TempTime > fResetTime)
            {
                ClearChild(StageTR);
                ClearChild(MainCameraTr);

                switch(gamestate)
                {
                    case GAMESTATE.PLAYING:
                        ResetTime();
                        CreateObject(PlayingStage.gameObject, Vector3.zero, StageTR);
                        CreateObject(Ball_Obj, Vector3.zero, StageTR);
                        break;
                    case GAMESTATE.SHOPSTAGE:
                        CreateObject(ShopStage, Vector3.zero, StageTR);
                        CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
                        break;
                }
                yield break;
            }
            yield return null;
        }
    }
}
