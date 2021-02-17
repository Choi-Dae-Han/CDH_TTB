using UnityEngine;

public class ClearUI : MonoBehaviour
{
    public GameObject NextStageButton;
    public Vector3 NextStageButtonPos = new Vector3(140f, -120f, 0f);

    private void Awake()
    {
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GM.UsingStage.GetComponent<Stage>().NextStage != null)
        {
            GameObject obj = Instantiate(NextStageButton);
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = NextStageButtonPos;
        }
    }
}
