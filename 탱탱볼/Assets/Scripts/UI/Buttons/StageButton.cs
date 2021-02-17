using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : BasicButton
{
    public Stage stage;
    public RawImage BGI;
    public Texture2D BGIofButton;
    public RectTransform StageListRT;
    public Sprite CoinImage;
    public Image[] CoinUIPos;

    private new void Awake()
    {
        base.Awake();

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
    }

    public void LoadStage()
    {
        if (stage != null)
        {
            GM.PlayingStage = stage.gameObject;
            ChangeGMState(5);
        }
    }

    public void PauseGame()
    {
        GM.Pause();
    }

    public void ResumeGame()
    {
        GM.Resume();
    }

    public void RetryGameStage()
    {
        GM.RetryStage();
    }

    public void LoadNextStage()
    {
        GM.PlayingStage = GM.PlayingStage.GetComponent<Stage>().NextStage;
        GM.ResetCamera();
        ChangeGMState(5);
    }

    public void ExitGameStage()
    {
        GM.ExitStage();
    }

    public void ChangeBackGround()
    {
        BGI.texture = BGIofButton;
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
}
