using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum STATE
    {
        CREATE, LIVE, WEIGHTLESS, DEAD
    }
    public STATE state = STATE.CREATE;

    [SerializeField] private float fGravityScale = 6.25f;
    [SerializeField] private float fMoveSpeed = 6f;
    public float fBouncePower = 450f;
    public float fHitPower = 120f;
    public float fXVelocity = 0f;
    public float fYVelocity = 0f;
    public float fRadius = 0f;
    public float fRotateSpeed = 0f;
    public bool LeftCrashed = false;
    public bool RightCrashed = false;
    public bool TopCrashed = false;
    public bool BottomCrashed = false;
    public Sprite UsingSkin;

    [SerializeField] private float fMaxXVelocity = 200f;
    [SerializeField] private float fMaxYVelocity = 800f;
    [SerializeField] private float fDecreaseXVelocity = 2f;

    [SerializeField] private AudioClip DeadSound;
    public AudioClip BounceSound;
    [SerializeField] private GameObject DeadEffect;
    private AudioSource AM;
    private GameManager GM;

    private void Start()
    {
        FollowCamera camera = GameObject.Find("Main Camera").GetComponent<FollowCamera>();
        camera.BallRT = gameObject.GetComponent<RectTransform>();
        camera.ChangeState(FollowCamera.CAMERASTATE.FOLLOWING);

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        fRadius = GetComponent<CircleCollider2D>().radius;
        fDecreaseXVelocity *= Time.fixedDeltaTime;
        fMaxXVelocity *= Time.fixedDeltaTime;
        fMaxYVelocity *= Time.fixedDeltaTime;
        fGravityScale *= Time.fixedDeltaTime;
        fBouncePower *= Time.fixedDeltaTime;
        fMoveSpeed *= Time.fixedDeltaTime;
        fHitPower *= Time.fixedDeltaTime;
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
                ChangeState(STATE.LIVE);
                break;
            case STATE.LIVE:
                Gravitypull();
                Move();
                break;
            case STATE.WEIGHTLESS:
                WeightlessMove();
                break;
            case STATE.DEAD:
                break;
        }
    }

    public void ChangeState(STATE s, float XVelo = 0.0f, float YVelo = 0.0f)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.LIVE:
                break;
            case STATE.WEIGHTLESS:
                fXVelocity = XVelo;
                fYVelocity = YVelo;
                break;
            case STATE.DEAD:                
                Dead();
                break;
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.A) && fXVelocity > -fMaxXVelocity) // 방향키 입력 및 최고 속도 제한
            fXVelocity -= fMoveSpeed;
        if (Input.GetKey(KeyCode.D) && fXVelocity < fMaxXVelocity)
            fXVelocity += fMoveSpeed;

        if (fXVelocity > fDecreaseXVelocity) // 항상 X축 속도 감소
            fXVelocity -= fDecreaseXVelocity;
        else if (fXVelocity < -fDecreaseXVelocity)
            fXVelocity += fDecreaseXVelocity;
        else
            fXVelocity = 0.0f;

        transform.position += new Vector3(fXVelocity, 0f);
        Rotation();
    }

    private void Rotation()
    {
        transform.Rotate(0f, 0f, -fXVelocity * fRotateSpeed);
    }

    private void Gravitypull()
    {
        fYVelocity -= fGravityScale;

        if (fYVelocity > fMaxYVelocity) fYVelocity = fMaxYVelocity; // Y축 최고 속도 제한
        else if (fYVelocity < -fMaxYVelocity) fYVelocity = -fMaxYVelocity;

        transform.position += new Vector3(0.0f, fYVelocity);
    }

    private void WeightlessMove()
    {
        if (Input.anyKey) ChangeState(STATE.LIVE);
        transform.position += new Vector3(fXVelocity, fYVelocity);//계산 후 최종 이동
        Rotation();
    }

    public void Dead()
    {
        if (AM != null) AM.PlayOneShot(DeadSound);
        if (DeadEffect != null)
        {
            GameObject obj = Instantiate(DeadEffect);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }

        GM.DelayAndReset(); // Test시 비활성화

        Destroy(gameObject);
    }
}
