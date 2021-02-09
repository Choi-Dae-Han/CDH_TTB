using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAllData : MonoBehaviour
{
    public AudioClip SE;
    public Sprite Skin;
    public GameObject Effect;
    public GameObject[] SkinGoods;
    public GameObject[] EffectGoods;
    public GameObject[] SoundEffectGoods;
    public List<Stage> Stages = new List<Stage>();
    public List<ButtonFunction> Buttons = new List<ButtonFunction>();
    public void SaveData()
    {
        Stages[0].SaveStageData(true, 0, false);
        for (int i = 1; i < Stages.Count; ++i)
        {
            Stages[i].SaveStageData(false, 0, false);
        }

        for (int i = 0; i < Buttons.Count; ++i)
        {
            Buttons[i].SaveButtonState(0);
        }

        BallData BD = new BallData(Effect, SE, Skin);
        string jsonData = DataManager.ObjectToJson(BD);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player/", jsonData);

        PlayerData PD = new PlayerData(0);
        string jsonData1 = DataManager.ObjectToJson(PD);
        DataManager.CreateJsonFile(Application.dataPath, "PlayerData", "/JsonData/Player/", jsonData1);

        for (int i = 0; i < SkinGoods.Length; ++i)
        {
            if (i != 0)
                SkinGoods[i].GetComponent<SkinGoods>().SaveGoodsData(false);
            else
                SkinGoods[i].GetComponent<SkinGoods>().SaveGoodsData(true);
        }

        for (int i = 0; i < EffectGoods.Length; ++i)
        {
            EffectGoods[i].GetComponent<EffectGoods>().SaveGoodsData(false);
        }

        for (int i = 0; i < SoundEffectGoods.Length; ++i)
        {
            if (i != 0)
                SoundEffectGoods[i].GetComponent<SoundEffectGoods>().SaveGoodsData(false);
            else
                SoundEffectGoods[i].GetComponent<SoundEffectGoods>().SaveGoodsData(true);
        }
    }
}
