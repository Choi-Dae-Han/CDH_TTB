using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinGoods : MonoBehaviour
{
    public bool IsHaving = false;
    public string NameForData = "";
    public Sprite SellingSkin;

    public void SaveGoodsData(bool having)
    {
        GoodsData SG = new GoodsData(having);
        string jsonData = DataManager.ObjectToJson(SG);
        DataManager.CreateJsonFile(Application.dataPath, NameForData, "/JsonData/Goods/Skin", jsonData);
    }
}
