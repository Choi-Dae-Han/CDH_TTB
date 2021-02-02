using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    GameManager GM;
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        RectTransform ScreenRT = GameObject.Find("Canvas").GetComponent<RectTransform>();
        GetComponent<RectTransform>().sizeDelta = new Vector2(ScreenRT.rect.width, ScreenRT.rect.height);

        if (GM.gamestate == GameManager.GAMESTATE.PLAYING)
        {
            gameObject.transform.SetParent(GM.MainCameraTr);
            gameObject.transform.localPosition = new Vector3(0f, 0f, 100f);
        }
    }
}
