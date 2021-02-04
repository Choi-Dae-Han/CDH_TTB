using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().TimeText = GetComponent<Text>();
    }
}
