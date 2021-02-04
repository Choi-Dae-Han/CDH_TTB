using UnityEngine;

public class ArrowBox : MonoBehaviour
{
    [SerializeField] private Vector3 Dir = Vector3.zero;
    [SerializeField] private float fWeightlessSpeed = 0f;

    [SerializeField] private RectTransform MuzzlePos;
    [SerializeField] private RectTransform Left;
    [SerializeField] private RectTransform Right;
    [SerializeField] private RectTransform Top;
    [SerializeField] private RectTransform Bottom;

    [SerializeField] private AudioClip SoundEffect;
    [SerializeField] private AudioClip SE_Wall;
    private AudioSource AM;

    private void Awake()
    {
        fWeightlessSpeed *= Time.fixedDeltaTime;
        AM = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }

    private void SendToDir(GameObject obj)
    {
        AM.PlayOneShot(SoundEffect);
        Ball ball = obj.GetComponent<Ball>();

        obj.transform.position = MuzzlePos.position + Dir * ball.fRadius;
        ball.ChangeState(Ball.STATE.WEIGHTLESS, fWeightlessSpeed * Dir.x);
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
                SendToDir(CrashObj.gameObject);
            }
            else if (fDistBottom < fDistLeft && fDistBottom < fDistRight)
            {
                ball.TopCrashed = true;
                ball.fYVelocity = -ball.fHitPower;
                AM.PlayOneShot(SE_Wall);
            }
            else if (fDistLeft < fDistTop && fDistLeft < fDistBottom)
            {
                ball.LeftCrashed = true;
                ball.fXVelocity = -ball.fHitPower;
                AM.PlayOneShot(SE_Wall);
            }
            else
            {
                ball.RightCrashed = true;
                ball.fXVelocity = ball.fHitPower;
                AM.PlayOneShot(SE_Wall);
            }
        }
    }
}
