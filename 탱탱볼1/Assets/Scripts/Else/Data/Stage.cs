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
        string jsonData = ObjectToJson(SD);
        CreateJsonFile(Application.dataPath, AreaName + " " + StageNum, "/JsonData/Stage/" + AreaName, jsonData);
    }

    public void OpenNextArea()
    {
        var data = LoadStageData(NextAreaName, 1);
        data.Opened = true;
        string jsonData = ObjectToJson(data);
        CreateJsonFile(Application.dataPath, NextAreaName + " " + 1, "/JsonData/Stage/" + NextAreaName, jsonData);
    }

    public void OpenNextStage()
    {
        var NextStateData = LoadStageData(AreaName, StageNum + 1);
        NextStateData.Opened = true;
        string jsonData = ObjectToJson(NextStateData);
        CreateJsonFile(Application.dataPath, AreaName + " " + (StageNum + 1), "/JsonData/Stage/" + AreaName, jsonData);
    }

    public StageData LoadStageData(string areaName, int stageNum)
    {
        var data = LoadJsonFile<StageData>(Application.dataPath, areaName + " " + stageNum, "/JsonData/Stage/" + areaName);
        GotCoins = data.GotCoins;
        Cleared = data.Cleared;

        return data;
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
