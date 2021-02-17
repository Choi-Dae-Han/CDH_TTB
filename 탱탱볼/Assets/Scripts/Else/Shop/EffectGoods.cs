using UnityEngine;

public class EffectGoods : MonoBehaviour
{
    public bool IsHaving = false;
    public string NameForData = "";
    public GameObject SellingEffect;

    public void SaveGoodsData(bool having)
    {
        GoodsData GD = new GoodsData(having);
        string jsonData = DataManager.ObjectToJson(GD);
        DataManager.CreateJsonFile(Application.dataPath, NameForData, "/JsonData/Goods/Effect", jsonData);
    }
}
