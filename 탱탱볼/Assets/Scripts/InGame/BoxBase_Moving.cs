using UnityEngine;
using System.Collections.Generic;

public class BoxBase_Moving : MonoBehaviour
{
    public bool bArrowBox = false;
    public bool bMovingBox = false;
    public bool bControlBox = false;
    public bool bDestroyable = false;

    public enum STATE
    {
        CREATE, IDLE, MOVE
    }
    public STATE state = STATE.CREATE;

    public enum KINDOFMOVE
    {
        NONE, ROUNDTRIP, STOPAFTERMOVE, CIRCLE
    }
    [DrawIf("bMovingBox", true)] public KINDOFMOVE KofM = KINDOFMOVE.NONE;

    private delegate void MoveFunc();
    [DrawIf("bMovingBox", true)] [SerializeField] private MoveFunc MF;
    [DrawIf("bMovingBox", true)] [SerializeField] public float fXVelocity = 0f;
    [DrawIf("bMovingBox", true)] [SerializeField] public float fYVelocity = 0f;
    [DrawIf("bMovingBox", true)] [SerializeField] private float fMoveSpeed = 0f;
    [DrawIf("bMovingBox", true)] [SerializeField] private float StartRadian = 0f;
    [DrawIf("bMovingBox", true)] [SerializeField] private float fRadian = 0f;
    [DrawIf("bMovingBox", true)] [SerializeField] private float fRadius = 1f;
    [DrawIf("bMovingBox", true)] [SerializeField] private int TargetNum = 0;
    [DrawIf("bMovingBox", true)] [SerializeField] private Vector3 CurTargetPos = Vector3.zero;
    [DrawIf("bMovingBox", true)] [SerializeField] private Vector3 OriginPos = Vector3.zero;
    [DrawIf("bMovingBox", true)] [SerializeField] private List<Vector3> TargetPos = new List<Vector3>();
    [DrawIf("bArrowBox", true)] [SerializeField] private Vector3 Dir = Vector3.zero;
    [DrawIf("bArrowBox", true)] [SerializeField] private float fWeightlessSpeed = 0f;
    [DrawIf("bArrowBox", true)] [SerializeField] private AudioClip SoundEffect;
    [DrawIf("bArrowBox", true)] [SerializeField] private RectTransform MuzzlePos;
    [DrawIf("bArrowBox", false)] [SerializeField] private AudioClip SE_Ground;
    [DrawIf("bArrowBox", false)] [SerializeField] public float fAddBouncePower = 0f;
    [DrawIf("bControlBox", true)] [SerializeField] private Color OriginColor = Color.white;
    [DrawIf("bControlBox", true)] [SerializeField] private Color ColorToChange = Color.white;
    [DrawIf("bControlBox", true)] [SerializeField] private BoxBase_Moving BoxToChange;
    [DrawIf("bDestroyable", true)] [SerializeField] private int nDestroyCount = 0;
    [DrawIf("bDestroyable", true)] [SerializeField] private int nCrashedCount = 0;
    [DrawIf("bDestroyable", true)] [SerializeField] private TMPro.TMP_Text ShowNum;
    [SerializeField] private RectTransform Left;
    [SerializeField] private RectTransform Right;
    [SerializeField] private RectTransform Top;
    [SerializeField] private RectTransform Bottom;
    [SerializeField] private AudioClip SE_Wall;
    private AudioSource AM;

    public GameObject Effect;

    private void Awake()
    {
        SwitchDir();
        GameObject GM = GameObject.Find("GameManager");
        AM = GM.GetComponent<AudioSource>();
        if (!bArrowBox) fAddBouncePower *= Time.fixedDeltaTime;
        else
        {
            fWeightlessSpeed *= Time.fixedDeltaTime;
            Dir = Right.position - Left.position;
            if (transform.rotation.y == 1f)
            {
                Vector3 Temp = Right.position;
                Right.position = Left.position;
                Left.position = Temp;
                MuzzlePos = Left;
            }
            else MuzzlePos = Right;
            Dir.Normalize();
        }
        if (bDestroyable)
        {
            transform.SetParent(GM.GetComponent<GameManager>().ObjectUIScreenTr);
            ShowNum.text = nDestroyCount - nCrashedCount + "";
        }
        if (bMovingBox)
        {
            fMoveSpeed *= Time.fixedDeltaTime;
            OriginPos = transform.position;
            TargetPos[0] += transform.position;
            CurTargetPos = TargetPos[0];

            switch (KofM)
            {
                case KINDOFMOVE.ROUNDTRIP:
                    MF = RoundTrip;
                    for (int i = 0; i < TargetPos.Count - 1; ++i)
                        TargetPos[i + 1] += TargetPos[i];
                    ChangeState(STATE.MOVE);
                    break;
                case KINDOFMOVE.STOPAFTERMOVE:
                    MF = RoundTrip;
                    ChangeState(STATE.IDLE);
                    break;
                case KINDOFMOVE.CIRCLE:
                    MF = CircleMove;
                    ChangeState(STATE.MOVE);
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch (state)
        {
            case STATE.CREATE:
                ChangeState(STATE.IDLE);
                break;
            case STATE.IDLE:
                break;
            case STATE.MOVE:
                MF();
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                fXVelocity = 0f;
                fYVelocity = 0f;
                break;
            case STATE.MOVE:
                switch (KofM)
                {
                    case KINDOFMOVE.ROUNDTRIP:
                        float DistX = Vector2.Distance(transform.position, new Vector2(CurTargetPos.x, transform.position.y));
                        float DistY = Vector2.Distance(transform.position, new Vector2(transform.position.x, CurTargetPos.y));

                        fXVelocity = fMoveSpeed;
                        fYVelocity = fMoveSpeed;
                        if (transform.position.x > CurTargetPos.x) fXVelocity *= -1f;
                        if (transform.position.y > CurTargetPos.y) fYVelocity *= -1f;

                        if (DistY > DistX) fXVelocity *= DistX / DistY;
                        else fYVelocity *= DistY / DistX;
                        break;
                    case KINDOFMOVE.CIRCLE:
                        break;
                }
                break;
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, CurTargetPos, fMoveSpeed);
    }

    private void CircleMove()
    {
        if (fRadian > 360f)
            fRadian = fRadian - 360f + StartRadian;
        fRadian += Time.fixedDeltaTime * fRadius;

        float deRad = fRadian * Mathf.Deg2Rad;
        fXVelocity = Mathf.Cos(deRad) * fMoveSpeed;
        fYVelocity = Mathf.Sin(deRad) * fMoveSpeed;
        CurTargetPos = transform.position + new Vector3(fXVelocity, fYVelocity, 0f);
        MoveToTarget();
    }

    private void RoundTrip()
    {
        if (transform.position == CurTargetPos)
        {
            if (TargetNum < TargetPos.Count - 1)
            {
                TargetNum++;
                CurTargetPos = TargetPos[TargetNum];
            }
            else
            {
                TargetNum = -1;
                CurTargetPos = OriginPos;
            }

            switch(KofM)
            {
                case KINDOFMOVE.ROUNDTRIP:
                    ChangeState(STATE.MOVE);
                    break;
                case KINDOFMOVE.STOPAFTERMOVE:
                    ChangeState(STATE.IDLE);
                    break;
            }
        }
        MoveToTarget();
    }

    void SwitchDir()
    {
        Vector2 Temp = Top.transform.position;
        switch (transform.rotation.eulerAngles.z)
        {
            case -90f:
                Top.transform.position = Left.transform.position;
                Left.transform.position = Bottom.transform.position;
                Bottom.transform.position = Right.transform.position;
                Right.transform.position = Temp;
                break;
            case 180f:
                Top.transform.position = Bottom.transform.position;
                Bottom.transform.position = Temp;
                Temp = Left.transform.position;
                Left.transform.position = Right.transform.position;
                Right.transform.position = Temp;
                break;
            case 90f:
                Top.transform.position = Right.transform.position;
                Right.transform.position = Bottom.transform.position;
                Bottom.transform.position = Left.transform.position;
                Left.transform.position = Temp;
                break;
        }
    }

    private void SendToDir(GameObject obj)
    {
        AM.PlayOneShot(SoundEffect);
        Ball ball = obj.GetComponent<Ball>();

        obj.transform.position = MuzzlePos.position + Dir * ball.fRadius;
        ball.ChangeState(Ball.STATE.WEIGHTLESS, fWeightlessSpeed * Dir.x);
    }

    private void Crash(ref bool crashPoint, ref float kindOfVelo, float value, AudioClip sound = null)
    {
        crashPoint = true;
        kindOfVelo = value;
        if(sound != null) AM.PlayOneShot(sound);
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Ball ball = CrashObj.gameObject.GetComponent<Ball>();
            Vector2 Contact = CrashObj.bounds.ClosestPoint(transform.position);
            float fDistLeft = Vector2.Distance(Contact, Left.position);
            float fDistRight = Vector2.Distance(Contact, Right.position);
            float fDistTop = Vector2.Distance(Contact, Top.position);
            float fDistBottom = Vector2.Distance(Contact, Bottom.position);

            if (ball.state == Ball.STATE.WEIGHTLESS) ball.ChangeState(Ball.STATE.LIVE);

            if (fDistTop <= fDistLeft && fDistTop <= fDistRight) // 위 충돌
            {
                if (Effect != null)
                {
                    GameObject obj = Instantiate(Effect);
                    obj.transform.position = Contact;
                }
                if (!bArrowBox)
                {
                    Crash(ref ball.BottomCrashed, ref ball.fYVelocity, ball.fBouncePower + fAddBouncePower + fYVelocity);
                    if (ball.TopCrashed) ball.ChangeState(Ball.STATE.DEAD);
                    if (SE_Ground == null) AM.clip = ball.BounceSound;
                    else AM.clip = SE_Ground;
                    AM.Play();
                }
                else SendToDir(CrashObj.gameObject);
                if (bDestroyable)
                {
                    ++nCrashedCount;
                    if (nCrashedCount >= nDestroyCount) Destroy(gameObject);
                    ShowNum.text = nDestroyCount - nCrashedCount + "";
                }
                if (bControlBox)
                {
                    switch (BoxToChange.state)
                    {
                        case STATE.MOVE:
                            GetComponent<SpriteRenderer>().color = ColorToChange;
                            BoxToChange.ChangeState(STATE.IDLE);
                            break;
                        case STATE.IDLE:
                            GetComponent<SpriteRenderer>().color = OriginColor;
                            BoxToChange.ChangeState(STATE.MOVE);
                            break;
                    }
                }
            }
            else if (fDistBottom <= fDistLeft && fDistBottom <= fDistRight)
            {
                Crash(ref ball.TopCrashed, ref ball.fYVelocity, -ball.fHitPower + fYVelocity, SE_Wall);
                if (ball.BottomCrashed) ball.ChangeState(Ball.STATE.DEAD);
            }
            else if (fDistLeft <= fDistTop && fDistLeft <= fDistBottom)
            {
                Crash(ref ball.LeftCrashed, ref ball.fXVelocity, -ball.fHitPower + fXVelocity, SE_Wall);
                if (ball.RightCrashed) ball.ChangeState(Ball.STATE.DEAD);
            }
            else
            {
                Crash(ref ball.RightCrashed, ref ball.fXVelocity, ball.fHitPower + fXVelocity, SE_Wall);
                if (ball.LeftCrashed) ball.ChangeState(Ball.STATE.DEAD);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D CrashObj)
    {
        Ball ball = CrashObj.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            if (ball.LeftCrashed) ball.LeftCrashed = false;
            if (ball.RightCrashed) ball.RightCrashed = false;
            if (ball.TopCrashed) ball.TopCrashed = false;
            if (ball.BottomCrashed) ball.BottomCrashed = false;
        }
    }
}
