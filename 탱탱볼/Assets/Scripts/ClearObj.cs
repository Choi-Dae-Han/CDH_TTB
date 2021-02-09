using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObj : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, MOVE, SHOWINGANI, DISAPPEAR
    }
    public STATE state = STATE.CREATE;

    GameManager GM;
    AudioSource AM;
    public Vector2 Target = Vector2.zero;
    public AudioClip ClearSound;
    public float fMoveSpeed = 0f;
    public float Dist1 = 0f;
    public float Dist2 = 0f;

    GameObject Obj_1;
    GameObject Obj_2;

    public Vector2 DisappearCoinPos = Vector2.zero;
    public Vector2 FirstCoinPos = Vector2.zero;
    public Vector2 SecondCoinPos = Vector2.zero;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        fMoveSpeed *= Time.fixedDeltaTime;
        ChangeState(STATE.IDLE);
    }

    private void Update()
    {
        StateProcess();
    }

    public void StateProcess()
    {
        switch (state)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                break;
            case STATE.MOVE:
                MoveToTarget();
                break;
            case STATE.SHOWINGANI:
                ShowAni();
                break;
            case STATE.DISAPPEAR:
                DisappearCoin();
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;
        switch(s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                break;
            case STATE.MOVE:
                if (GM.nScore == 0)
                {
                    ShowClearUI();
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.transform.SetParent(GM.MainCameraTr);
                    Target = GM.MainCameraTr.position;
                }
                break;
            case STATE.SHOWINGANI:
                DisappearCoinPos = new Vector2(GM.MainCameraTr.position.x - 600f, GM.MainCameraTr.position.y + 450f);
                switch (GM.nScore)
                {
                    case 1:
                        break;
                    case 2:
                        Obj_1 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr);
                        Obj_1.transform.localScale = transform.localScale;
                        FirstCoinPos = new Vector2(GM.MainCameraTr.position.x - 150f, transform.position.y);
                        SecondCoinPos = new Vector2(GM.MainCameraTr.position.x + 150f, transform.position.y);
                        break;
                    case 3:
                        Obj_1 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr);
                        Obj_1.transform.localScale = transform.localScale;
                        Obj_2 = GM.CreateObject(gameObject, transform.position, GM.MainCameraTr);
                        Obj_2.transform.localScale = transform.localScale;
                        FirstCoinPos = new Vector2(transform.position.x - 300f, transform.position.y);
                        SecondCoinPos = new Vector2(transform.position.x + 300f, transform.position.y);
                        break;
                }
                break;
            case STATE.DISAPPEAR:
                switch(GM.nScore)
                {
                    case 1:
                        break;
                    case 2:
                        Dist1 = Vector2.Distance(Obj_1.transform.position, DisappearCoinPos) * 0.66f;
                        break;
                    case 3:
                        Dist1 = Vector2.Distance(Obj_1.transform.position, DisappearCoinPos) * 0.66f;
                        Dist2 = Vector2.Distance(transform.position, DisappearCoinPos) * 0.66f;
                        break;
                }
                break;
        }
    }

    public void MoveToTarget()
    {
        LerpToTarget(transform, Target);

        if (transform.localScale.x <= 3f)
            transform.localScale *= 1.015f;

        if (Vector2.Distance(transform.localPosition, Vector2.zero) < 2f &&
            transform.localScale.x >= 3f)
        {
            ChangeState(STATE.SHOWINGANI);
        }
    }

    public void ShowAni()
    {
        switch (GM.nScore)
        {
            case 1:
                ChangeState(STATE.DISAPPEAR);
                break;
            case 2:
                LerpToTarget(Obj_1.transform, FirstCoinPos);
                LerpToTarget(transform, SecondCoinPos);

                if (Vector2.Distance(Obj_1.transform.position, FirstCoinPos) <= 2f)
                    ChangeState(STATE.DISAPPEAR);
                break;
            case 3:
                LerpToTarget(Obj_1.transform, FirstCoinPos);
                LerpToTarget(Obj_2.transform, SecondCoinPos);

                if (Vector2.Distance(Obj_1.transform.position, FirstCoinPos) <= 2f)
                    ChangeState(STATE.DISAPPEAR);
                break;
        }
    }

    public void DisappearCoin()
    {
        switch (GM.nScore)
        {
            case 1:
                LerpToTarget(transform, DisappearCoinPos);

                if (Vector2.Distance(transform.position, DisappearCoinPos) <= 2f)
                {
                    ShowClearUI();
                    Destroy(gameObject);
                }
                break;
            case 2:
                LerpToTarget(Obj_1.transform, DisappearCoinPos);
                if(Vector2.Distance(Obj_1.transform.position, DisappearCoinPos) < Dist1)
                    LerpToTarget(transform, DisappearCoinPos);

                if (Vector2.Distance(transform.position, DisappearCoinPos) <= 2f)
                {
                    ShowClearUI();
                    Destroy(Obj_1);
                    Destroy(gameObject);
                }
                break;
            case 3:
                LerpToTarget(Obj_1.transform, DisappearCoinPos);
                if (Vector2.Distance(Obj_1.transform.position, DisappearCoinPos) < Dist1)
                    LerpToTarget(transform, DisappearCoinPos);
                if (Vector2.Distance(transform.position, DisappearCoinPos) < Dist2)
                    LerpToTarget(Obj_2.transform, DisappearCoinPos);

                if (Vector2.Distance(Obj_2.transform.position, DisappearCoinPos) <= 2f)
                {
                    ShowClearUI();
                    Destroy(Obj_1);
                    Destroy(Obj_2);
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void ShowClearUI()
    {
        GM.CreateUI(GM.ClearUI, Vector2.zero);
    }

    public void LerpToTarget(Transform objTR, Vector2 target)
    {
        objTR.position = Vector2.Lerp(objTR.position, target, fMoveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.CompareTag("Player"))
        {
            AM.PlayOneShot(ClearSound);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GM.StageClear(GetComponent<ClearObj>());
        }
    }
}
