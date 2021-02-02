using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOwnedCoin : MonoBehaviour
{
    public TMPro.TMP_Text textt;

    void Start()
    {
        textt.text = GameObject.Find("GameManager").GetComponent<GameManager>().nOwnedCoin + "";
    }
}
