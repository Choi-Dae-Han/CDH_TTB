using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    GameManager GM;
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        float XRatio = GM.MainScreenTr.sizeDelta.x / 1280f;
        float YRatio = GM.MainScreenTr.sizeDelta.y / 720f;
        if (GM.gamestate == GameManager.GAMESTATE.PLAYING)
        {
            gameObject.transform.SetParent(GM.MainCameraTr);
            gameObject.transform.localPosition = new Vector3(0f, 0f, 100f);
            gameObject.transform.localScale = new Vector3(XRatio * GM.MainScreenTr.localScale.x, YRatio * GM.MainScreenTr.localScale.y, 1f); 
        }
        else
            GetComponent<RectTransform>().sizeDelta = GM.MainScreenTr.sizeDelta;
    }
}
