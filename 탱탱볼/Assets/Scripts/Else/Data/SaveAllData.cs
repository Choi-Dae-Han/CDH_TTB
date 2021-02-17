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
    public List<BasicButton> Buttons = new List<BasicButton>();

    public void SaveData()
    {
        // 스테이지 데이터 초기화
        Stages[0].SaveStageData(true, 0, false);
        for (int i = 1; i < Stages.Count; ++i)
        {
            Stages[i].SaveStageData(false, 0, false);
        }

        // 버튼 데이터 초기화
        for (int i = 0; i < Buttons.Count; ++i)
        {
            Buttons[i].SaveButtonState(0);
        }

        // 공 데이터 초기화
        BallData BD = new BallData(Effect, SE, Skin);
        string jsonData = DataManager.ObjectToJson(BD);
        DataManager.CreateJsonFile(Application.dataPath, "BallData", "/JsonData/Player/", jsonData);

        // 플레이어 데이터 초기화
        PlayerData PD = new PlayerData(0);
        string jsonData1 = DataManager.ObjectToJson(PD);
        DataManager.CreateJsonFile(Application.dataPath, "PlayerData", "/JsonData/Player/", jsonData1);

        // 스킨 상품 데이터 초기화
        for (int i = 0; i < SkinGoods.Length; ++i)
        {
            if (i != 0)
                SkinGoods[i].GetComponent<SkinGoods>().SaveGoodsData(false);
            else
                SkinGoods[i].GetComponent<SkinGoods>().SaveGoodsData(true);
        }

        // 이펙트 상품 데이터 초기화
        for (int i = 0; i < EffectGoods.Length; ++i)
        {
            EffectGoods[i].GetComponent<EffectGoods>().SaveGoodsData(false);
        }

        // 효과음 상품 데이터 초기화
        for (int i = 0; i < SoundEffectGoods.Length; ++i)
        {
            if (i != 0)
                SoundEffectGoods[i].GetComponent<SoundEffectGoods>().SaveGoodsData(false);
            else
                SoundEffectGoods[i].GetComponent<SoundEffectGoods>().SaveGoodsData(true);
        }
    }
}
