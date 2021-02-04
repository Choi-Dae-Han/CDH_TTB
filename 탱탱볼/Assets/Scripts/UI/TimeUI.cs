using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().TimeText = GetComponent<Text>();
    }
}
