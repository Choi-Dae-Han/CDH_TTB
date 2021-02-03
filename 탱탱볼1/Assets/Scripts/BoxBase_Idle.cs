using UnityEngine;

public class BoxBase_Idle : MonoBehaviour
{
    public bool bArrowBox = false;
    public bool bControlBox = false;
    public bool bDestroyable = false;

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

    private void Start()
    {
        SwitchDir();
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();

        if (!bArrowBox) fAddBouncePower *= Time.fixedDeltaTime;
        else
        {
            fWeightlessSpeed *= Time.fixedDeltaTime;
            Dir = Right.position - Left.position;
            if (transform.rotation.eulerAngles.z == 180f)
            {
                Vector3 Temp = Right.position;
                Right.position = Left.position;
                Left.position = Temp;
                MuzzlePos = Left;
            }
            else MuzzlePos = Right;
            Dir.Normalize();
        }

        if (bDestroyable) ShowNum.text = nDestroyCount - nCrashedCount + "";
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
    private void AddVelo(ref bool crashPoint, ref float kindOfVelo, float value, AudioClip sound = null)
    {
        crashPoint = true;
        kindOfVelo = value;
        if (sound != null) AM.PlayOneShot(sound);
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Ball ball = CrashObj.gameObject.GetComponent<Ball>();
            Vector3 Contact = CrashObj.bounds.ClosestPoint(transform.position);
            float fDistLeft = Vector2.Distance(Contact, Left.position);
            float fDistRight = Vector2.Distance(Contact, Right.position);
            float fDistTop = Vector2.Distance(Contact, Top.position);
            float fDistBottom = Vector2.Distance(Contact, Bottom.position);

            if (ball.state == Ball.STATE.WEIGHTLESS) ball.ChangeState(Ball.STATE.LIVE);

            if (fDistTop < fDistLeft && fDistTop < fDistRight) // 위 충돌
            {
                if (Effect != null)
                {
                    GameObject obj = Instantiate(Effect);
                    obj.transform.position = Contact;
                }
                ball.BottomCrashed = true;
                if (!bArrowBox)
                {
                    AddVelo(ref ball.BottomCrashed, ref ball.fYVelocity, ball.fBouncePower + fAddBouncePower);
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
                        case BoxBase_Moving.STATE.MOVE:
                            GetComponent<SpriteRenderer>().color = ColorToChange;
                            BoxToChange.ChangeState(BoxBase_Moving.STATE.IDLE);
                            break;
                        case BoxBase_Moving.STATE.IDLE:
                            GetComponent<SpriteRenderer>().color = OriginColor;
                            BoxToChange.ChangeState(BoxBase_Moving.STATE.MOVE);
                            break;
                    }
                }
            }
            else if (fDistBottom <= fDistLeft && fDistBottom <= fDistRight)
                AddVelo(ref ball.TopCrashed, ref ball.fYVelocity, -ball.fHitPower, SE_Wall);
            else if (fDistLeft <= fDistTop && fDistLeft <= fDistBottom)
                AddVelo(ref ball.LeftCrashed, ref ball.fXVelocity, -ball.fHitPower, SE_Wall);
            else
                AddVelo(ref ball.RightCrashed, ref ball.fXVelocity, ball.fHitPower, SE_Wall);
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