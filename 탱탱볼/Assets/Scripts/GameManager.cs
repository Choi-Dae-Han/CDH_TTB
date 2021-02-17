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

    // 시간, 볼륨
    public float fVolume = 1f;
    public float fResetTime = 2f;
    public float fSec = 0f;
    public int nSec = 0;
    public int nMin = 0;

    // 공 오브젝트
    public GameObject Ball_Obj;
    public GameObject Ball_Obj_Shop;
    public GameObject UsingBall;

    // 일시정지 UI
    public GameObject PauseButtonUI;
    public GameObject PauseUI;
    public GameObject UsingPauseUI;

    // UI
    public GameObject BackButtonUI;
    public GameObject ClearUI;
    public GameObject TimeUI;

    // 상태에 따른 큰 메뉴
    public GameObject TitleMenu;
    public GameObject StageMenu;
    public GameObject ShopMenu;
    public GameObject UsingShopMenu;

    //판넬
    public GameObject WhiteScreen;
    public GameObject UsingWhiteScreen;

    //스테이지
    public GameObject PlayingStage;
    public GameObject shopStage;
    public GameObject UsingStage;

    // 캔버스
    public RectTransform MainScreenTr;
    public Transform ObjectUIScreenTr;

    // 메인 카메라
    public Transform MainCameraTr;
    public FollowCamera FC;
    public Camera camera1;
    public Vector3 ResetCameraPos = new Vector3(0f, 0f, -100f);

    //UI 카메라
    public Camera UICamera;

    //기타
    public Transform StageTR;
    public Text TimeText;
    public ClearObj CO;
    public int nAddScoreObj = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        camera1 = MainCameraTr.gameObject.GetComponent<Camera>();
        FC = MainCameraTr.gameObject.GetComponent<FollowCamera>();
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
                CountTime();
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
            case GAMESTATE.TITLEMENU:
                LoadTitleMenu();
                break;
            case GAMESTATE.STAGEMENU:
                LoadStageMenu();
                break;
            case GAMESTATE.SHOP:
                LoadShopMenu();
                break;
            case GAMESTATE.SHOPSTAGE:
                LoadShopStage();
                break;
            case GAMESTATE.PLAYING:
                LoadStage();
                break;
            case GAMESTATE.CLEAR:
                StageClear();
                break;
        }
    }

    public void Pause()
    {
        if (UsingPauseUI == null)
        {
            Time.timeScale = 0.0f;
            UsingWhiteScreen = CreateUI(WhiteScreen, Vector3.zero);
            UsingWhiteScreen.GetComponent<RectTransform>().sizeDelta = MainScreenTr.sizeDelta;
            UsingPauseUI = CreateUI(PauseUI, Vector3.zero);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        if (UsingWhiteScreen != null) Destroy(UsingWhiteScreen);
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

        if (TimeText != null) TimeText.text = nMin + " : " + nSec;
    }

    public void ResetTime()
    {
        nSec = 0;
        fSec = 0f;
        nMin = 0;
    }

    public void ResetCamera()
    {
        MainCameraTr.position = ResetCameraPos;
        MainCameraTr.gameObject.GetComponent<FollowCamera>().GoingTarget = ResetCameraPos;
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

    public void ClearChild(Transform tr)
    {
        int ChildCount = tr.childCount;
        for (int i = 0; i < ChildCount; ++i)
        {
            Destroy(tr.GetChild(i).gameObject);
        }
    }

    public void LoadTitleMenu()
    {
        UICamera.clearFlags = CameraClearFlags.Depth;
        ClearChild(MainScreenTr);
        CreateUI(TitleMenu, Vector3.zero);
    }

    public void LoadStageMenu()
    {
        ClearChild(MainScreenTr);
        CreateUI(StageMenu, Vector3.zero);
    }

    public void LoadShopMenu()
    {
        if (UsingBall != null) UsingBall.tag = "Untagged";
        ClearChild(MainCameraTr);
        ClearChild(MainScreenTr);
        ClearChild(StageTR);
        if (UsingShopMenu == null) UsingShopMenu = CreateUI(ShopMenu, Vector3.zero);
        else UsingShopMenu.transform.SetParent(MainScreenTr);
        StopAllCoroutines();
        ResetCamera();
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);
    }

    public void LoadStage()
    {
        if (PlayingStage != null)
        {
            if (UsingBall != null) UsingBall.tag = "Untagged";
            nAddScoreObj = 0;
            ClearChild(MainCameraTr);
            ClearChild(MainScreenTr);
            ClearChild(StageTR);
            StopAllCoroutines();
            ResetTime();
            GameObject Temp = CreateUI(PauseButtonUI, Vector3.zero);
            GameObject Temp1 = CreateUI(TimeUI, Vector3.zero);
            Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-130f, -80f);
            Temp1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
            TimeText = Temp1.GetComponent<Text>();
            UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);
            UsingBall = CreateObject(Ball_Obj, Vector3.zero, StageTR);
            Resume();
        }
    }

    public void LoadShopStage()
    {
        UsingShopMenu.transform.SetParent(null);
        ClearChild(MainScreenTr);

        GameObject Temp = CreateUI(BackButtonUI, Vector3.zero);
        Temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(130f, -130f);

        PlayingStage = shopStage;
        UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);
        UsingBall = CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
    }

    public void RetryStage()
    {
        if (gamestate != GAMESTATE.PLAYING)
            ChangeGameState(GAMESTATE.PLAYING);
        else
            LoadStage();
    }

    public void StageClear()
    {
        ClearChild(MainScreenTr);
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);

        Stage s = UsingStage.GetComponent<Stage>();

        int tempScore = 1;
        if (nAddScoreObj == 0) tempScore++;
        if (nMin * 60 + fSec < s.fLimitTime) tempScore++;

        if (tempScore > s.GotCoins)
        {
            CO.nScore = tempScore - s.GotCoins;
            s.SaveStageData(true, tempScore, true);
            SavePlayerData(tempScore);
        }
        if (s.NextStage != null) s.OpenNextStage();
        if (s.NextAreaName != "") s.OpenNextArea();

        CO.ChangeState(ClearObj.STATE.MOVE);
    }

    public void ExitStage()
    {
        if (UsingBall != null) UsingBall.tag = "Untagged";
        PlayingStage = null;
        TimeText = null;
        CO = null;
        nAddScoreObj = 0;
        FC.ChangeState(FollowCamera.CAMERASTATE.IDLE);
        ClearChild(MainCameraTr);
        ClearChild(StageTR);
        ResetCamera();
        ResetTime();
        Resume();
        ChangeGameState(GAMESTATE.STAGEMENU);
    }

    public void DelayAndReset()
    {
        StartCoroutine(DelayAndReset_Cor());
    }

    IEnumerator DelayAndReset_Cor()
    {
        float TempTime = 0f;
        while(true)
        {
            TempTime += Time.smoothDeltaTime;
            if (gamestate != GAMESTATE.PLAYING &&
                gamestate != GAMESTATE.SHOPSTAGE)
                yield break;
            else if (TempTime > fResetTime)
            {
                ClearChild(StageTR);
                ClearChild(MainCameraTr);
                UsingStage = CreateObject(PlayingStage, Vector3.zero, StageTR);

                switch (gamestate)
                {
                    case GAMESTATE.PLAYING:
                        ResetTime();
                        UsingBall = 
                            CreateObject(Ball_Obj, Vector3.zero, StageTR);
                        break;
                    case GAMESTATE.SHOPSTAGE:
                        UsingBall =
                            CreateObject(Ball_Obj_Shop, Vector3.zero, StageTR);
                        break;
                }
                yield break;
            }
            yield return null;
        }
    }

    public void SavePlayerData(int score)
    {
        var data = DataManager.LoadJsonFile<PlayerData>(
            Application.dataPath, "PlayerData", "/JsonData/Player/");
        data.OwnedCoin += score;
        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(
            Application.dataPath, "PlayerData", "/JsonData/Player/", jsonData);
    }
}
