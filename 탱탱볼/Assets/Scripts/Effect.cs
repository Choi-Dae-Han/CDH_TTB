using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private enum STATE
    {
        CREATE,SHOW
    }
    private STATE state = STATE.CREATE;

    [SerializeField] private float fDeleteEffectTime = 0.0f;
    private float fTime = 0.0f;

    private void Awake()
    {
        ChangeState(STATE.SHOW);
    }

    private void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch(s)
        {
            case STATE.CREATE:
                break;
            case STATE.SHOW:
                StartCoroutine(ShowEffect());
                break;
        }
    }

    private IEnumerator ShowEffect()
    {
        while (true)
        {
            fTime += Time.smoothDeltaTime;
            if (fTime >= fDeleteEffectTime)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
