﻿using System.Collections;
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

    public GameObject UsingLockImg;
    public GameObject UI;
    public GameObject UsingUI;
    public AudioClip OnMouseSE;
    public RawImage BGI;
    public Texture2D BGIofButton;
    public RectTransform ContentRT;
    public RectTransform StageListRT;
    public Stage stage;
    AudioSource AM;
    GameManager GM;
    Vector2 ButtonScale = Vector2.zero;

    private void Start()
    {

        GameObject GM_Obj = GameObject.Find("GameManager");
        AM = GM_Obj.GetComponent<AudioSource>();
        GM = GM_Obj.GetComponent<GameManager>();
        ButtonScale = new Vector2(transform.localScale.x, transform.localScale.y);

        if (stage != null)
        {
            LoadButtonData();
            if (stage.LoadStageData(stage.AreaName, stage.StageNum).Opened)
            {
                ChangeButtonState(BUTTONSTATE.UNLOCK);
                SaveButtonState(2);
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
            UsingUI = Instantiate(UI);
            UsingUI.GetComponent<ChildUI>().ParentButton = gameObject;
            UsingUI.transform.SetParent(ContentRT);
            UsingUI.transform.localPosition = ContentRT.rect.center;
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

    public void ShowUI()
    {
        if (UsingUI == null)
        {
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.localPosition = Vector3.zero;
        }
    }

    public void ShowStageList()
    {
        GM.ClearChild(StageListRT);
        UsingUI = Instantiate(UI);
        UsingUI.transform.SetParent(StageListRT);
        UsingUI.transform.localPosition = Vector3.zero;
    }

    public void LoadStage()
    {
        GM.LoadStage(stage.gameObject);
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

    public void ChangeBallEffect(int i)
    {
        GM.Ball_Obj.GetComponent<Ball>().ChangeBallEffect(i);
    }

    public void ChangeBallSkin(Sprite ballSkin)
    {
        GM.Ball_Obj.GetComponent<Ball>().UsingSkin = ballSkin;
    }

    public void ChangeBallSE(AudioClip SE)
    {
        GM.Ball_Obj.GetComponent<Ball>().BounceSound = SE;
    }

    public void ShowGoodsInfo()
    {
        if (UsingUI == null)
        {
            GM.CreateWhiteScreen();
            UsingUI = Instantiate(UI);
            UsingUI.transform.SetParent(transform.root);
            UsingUI.transform.position = Vector3.zero;

            GoodsInfo GI = UsingUI.GetComponent<GoodsInfo>();
            GI.GoodsImage.texture = gameObject.transform.GetChild(0).GetComponent<RawImage>().texture;
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
        string jsonData = ObjectToJson(BD);
        CreateJsonFile(Application.dataPath, gameObject.name, "/JsonData/Button", jsonData);
    }

    public void LoadButtonData()
    {
        var data = LoadJsonFile<ButtonData>(Application.dataPath, gameObject.name, "/JsonData/Button");
        buttonState = (BUTTONSTATE)data.buttonState;
    }

    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    T JsonToObject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    void CreateJsonFile(string createPath, string fileName, string path, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}" + path + "/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    T LoadJsonFile<T>(string loadPath, string fileName, string path)
    {
        FileStream fileStream = new FileStream(string.Format("{0}" + path + "/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
