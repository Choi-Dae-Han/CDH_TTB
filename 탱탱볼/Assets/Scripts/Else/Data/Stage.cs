using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Stage : MonoBehaviour
{
    public int StageNum = 0;
    public string NextAreaName = "";
    public string AreaName = "";
    public float fLimitTime = 30f;
    public int GotCoins = 0;
    public bool Cleared = false;
    public bool Opened = false;
    public GameObject NextStage;

    private void Start()
    {
        LoadStageData(AreaName, StageNum);
    }

    public void SaveStageData(bool opend, int gotCoin, bool cleared)
    {
        StageData SD = new StageData(opend, gotCoin, cleared);
        string jsonData = DataManager.ObjectToJson(SD);
        DataManager.CreateJsonFile(Application.dataPath, AreaName + " " + StageNum, "/JsonData/Stage/" + AreaName, jsonData);
    }

    public void OpenNextArea()
    {
        var data = LoadStageData(NextAreaName, 1);
        data.Opened = true;
        string jsonData = DataManager.ObjectToJson(data);
        DataManager.CreateJsonFile(Application.dataPath, NextAreaName + " " + 1, "/JsonData/Stage/" + NextAreaName, jsonData);
    }

    public void OpenNextStage()
    {
        var NextStateData = LoadStageData(AreaName, StageNum + 1);
        NextStateData.Opened = true;
        string jsonData = DataManager.ObjectToJson(NextStateData);
        DataManager.CreateJsonFile(Application.dataPath, AreaName + " " + (StageNum + 1), "/JsonData/Stage/" + AreaName, jsonData);
    }

    public StageData LoadStageData(string areaName, int stageNum)
    {
        var data = DataManager.LoadJsonFile<StageData>(Application.dataPath, areaName + " " + stageNum, "/JsonData/Stage/" + areaName);
        GotCoins = data.GotCoins;
        Cleared = data.Cleared;

        return data;
    }
}
