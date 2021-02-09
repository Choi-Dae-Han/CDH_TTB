using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectGoods : MonoBehaviour
{
    public bool IsHaving = false;
    public string NameForData = "";
    public AudioClip SellingSE;

    public void SaveGoodsData(bool having)
    {
        GoodsData GD = new GoodsData(having);
        string jsonData = DataManager.ObjectToJson(GD);
        DataManager.CreateJsonFile(Application.dataPath, NameForData, "/JsonData/Goods/SoundEffect", jsonData);
    }
}
