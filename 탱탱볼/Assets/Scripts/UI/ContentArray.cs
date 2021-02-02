using System.Collections.Generic;
using UnityEngine;

public class ContentArray : MonoBehaviour
{
    [SerializeField] private List<GameObject> StageButtonList;
    [SerializeField] private float fFirstInterval = 80f;
    [SerializeField] private float fInterval = 10f;

    private void Start()
    {
        RectTransform RT = GetComponent<RectTransform>();
        float ButtonHeight = StageButtonList[0].GetComponent<RectTransform>().rect.height;
        float ContentYMax = RT.rect.yMax;
        float ButtonPosX = RT.rect.width / 2;

        RT.sizeDelta = new Vector2(RT.sizeDelta.x, fFirstInterval + StageButtonList.Count * (ButtonHeight + fInterval));

        for (int i = 0; i < StageButtonList.Count; ++i)
        {
            StageButtonList[i].transform.localPosition =
                new Vector3(ButtonPosX, ContentYMax - (fFirstInterval + (ButtonHeight + fInterval) * i), 0f);
        }
    }
}
