using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsInfo : MonoBehaviour
{
    public RawImage GoodsImage;
    public GameObject ParentButton;
    public Transform TestApplyButtonTR;
    public Transform BuyButtonTR;
    public Transform ApplyButtonTR;

    public TMPro.TMP_Text GoodsName; 
    public TMPro.TMP_Text MoneyPrice; 
    public TMPro.TMP_Text CoinPrice;

    public GameObject SellingEffect;
    public GameObject SellingSE;
    public GameObject SellingSkin;
}
