using UnityEngine;

public class AwakeGame : MonoBehaviour
{
    private enum STATE
    {
        APPEARING, SHOWING, DISAPPEARING
    }
    private STATE state = STATE.APPEARING;

    [SerializeField] private TMPro.TMP_Text Text1;
    [SerializeField] private TMPro.TMP_Text Text2;
    [SerializeField] private float fSpeed = 0.5f;
    private GameManager GM;

    private void Start()
    {
        Text1.alpha = 0.0f;
        Text2.alpha = 0.0f;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        GM.fSec += Time.smoothDeltaTime;
        if (GM.fSec >= 10f || Input.anyKeyDown && GM.fSec >= 2f)
        {
            GM.fSec = 0.0f;
            GM.ChangeGameState(GameManager.GAMESTATE.TITLEMENU);
        }

        switch (state)
        {
            case STATE.APPEARING:
                if (GM.fSec > 1f) ShowText(Text1);
                if (Text1.alpha >= 1f) ShowText(Text2);
                if (Text2.alpha >= 1f) ChangeState(STATE.SHOWING);
                break;
            case STATE.SHOWING:
                if (GM.fSec >= 7f) ChangeState(STATE.DISAPPEARING);
                break;
            case STATE.DISAPPEARING:
                DisappearText(Text1);
                DisappearText(Text2);
                break;
        }
    }

    private void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.APPEARING:
                break;
            case STATE.SHOWING:
                break;
            case STATE.DISAPPEARING:
                break;
        }
    }

    private void ShowText(TMPro.TMP_Text text)
    {
        if (text.alpha < 1f)
            text.alpha += fSpeed * Time.smoothDeltaTime;
    }

    private void DisappearText(TMPro.TMP_Text text)
    {
        if (text.alpha > 0f)
            text.alpha -= Time.smoothDeltaTime;
    }
}
