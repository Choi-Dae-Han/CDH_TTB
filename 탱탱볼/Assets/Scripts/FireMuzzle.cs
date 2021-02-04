using UnityEngine;
using System.Collections;

public class FireMuzzle : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, ATTACK
    }
    public STATE state = STATE.CREATE;

    private float fTime = 0f;
    [SerializeField] private float fDeleteTime = 0f;
    [SerializeField] private float fFireDelay = 0f;
    [SerializeField] private float ProjSpeed = 100f;
    [SerializeField] private GameObject ProjObj;
    [SerializeField] private AudioSource AM;
    [SerializeField] private AudioClip SoundEffect;

    private void Awake()
    {
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();
        ProjSpeed *= Time.fixedDeltaTime;
    }

    private void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch(state)
        {
            case STATE.CREATE:
                ChangeState(STATE.IDLE);
                break;
            case STATE.IDLE:
                fTime += Time.smoothDeltaTime;
                if (fTime >= fFireDelay)
                    ChangeState(STATE.ATTACK);
                break;
            case STATE.ATTACK:
                break;
        }
    }

    private void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                break;
            case STATE.ATTACK:
                fire();
                ChangeState(STATE.IDLE);
                break;
        }
    }

    private void fire()
    {
        AM.PlayOneShot(SoundEffect);
        GameObject proj = Instantiate(ProjObj);
        proj.transform.rotation = transform.rotation;
        proj.transform.position = transform.position;
        fTime = 0.0f;
        StartCoroutine(GoToTarget(proj));      
    }

    IEnumerator GoToTarget(GameObject proj)
    {
        float PassedTime = 0f;
        while(true)
        {
            PassedTime += Time.smoothDeltaTime;
            if (PassedTime >= fDeleteTime)
            {
                Destroy(proj);
                yield break;
            }

            proj.transform.Translate(-transform.right * ProjSpeed);
            yield return null;
        }
    }
}
