using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOwnedCoin : MonoBehaviour
{
    public TMPro.TMP_Text textt;

    private void Awake()
    {
        var data = DataManager.LoadJsonFile<PlayerData>(Application.dataPath, "PlayerData", "/JsonData/Player/");
        textt.text = data.OwnedCoin + "";
    }
}
