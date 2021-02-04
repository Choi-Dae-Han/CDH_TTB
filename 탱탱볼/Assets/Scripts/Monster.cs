using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum STATE
    {
        CREATE, IDLE, MOVE, DEAD
    }
    public STATE state = STATE.CREATE;

    public float fMoveSpeed = 50.0f;
    public float fTime = 0.0f;
    private int StepOfAngry = 0;
    [SerializeField] private float AngryTime = 4.0f;

    [SerializeField] private RectTransform Left;
    [SerializeField] private RectTransform Right;
    [SerializeField] private RectTransform Top;

    [SerializeField] private Transform TargetObj;

    [SerializeField] private AudioClip DeadSound;
    [SerializeField] private GameObject DeadEffect;

    private AudioSource AM;
    private GameManager GM;
    private Animator Ani;
    private Rigidbody2D RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GM.GetComponent<AudioSource>();
        Ani = GetComponent<Animator>();
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
                break;
            case STATE.MOVE:
                if(RB.velocity.y == 0) MoveToTarget();
                if (StepOfAngry < 3)
                {
                    fTime += Time.smoothDeltaTime;
                    if (fTime >= AngryTime)
                    {
                        StepOfAngry++;
                        fTime = 0f;
                        Angry();
                    }
                }
                break;
            case STATE.DEAD:
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (s)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                Ani.SetFloat("MoveSpeed", 0.0f);
                break;
            case STATE.MOVE:
                Ani.SetFloat("MoveSpeed", fMoveSpeed * 0.02f);
                break;
            case STATE.DEAD:
                Dead();
                break;
        }
    }

    private void MoveToTarget()
    {
        if (TargetObj.position.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            RB.velocity = new Vector2(fMoveSpeed, 0f);
        }
        else
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            RB.velocity = new Vector2(-fMoveSpeed, 0f);
        }
    }

    private void Angry()
    {       
        GetComponent<SpriteRenderer>().color -= new Color(0f, 0.2f, 0.2f, 0f);
        fMoveSpeed *= 1.3f;
        Ani.SetFloat("MoveSpeed", fMoveSpeed * 0.02f);
    }

    private void OnCollisionEnter2D(Collision2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            Vector3 Contact = CrashObj.contacts[0].point;
            float fDistLeft = Vector2.Distance(Contact, Left.position);
            float fDistRight = Vector2.Distance(Contact, Right.position);
            float fDistTop = Vector2.Distance(Contact, Top.position);

            if (fDistTop < fDistLeft && fDistTop < fDistRight) // 위 충돌
            {
                Ball ball = CrashObj.gameObject.GetComponent<Ball>();
                ball.fYVelocity = ball.fBouncePower;
                ChangeState(STATE.DEAD);
            }
            else
                CrashObj.gameObject.GetComponent<Ball>().ChangeState(Ball.STATE.DEAD);
        }
    }

    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            TargetObj = CrashObj.gameObject.transform;
            ChangeState(STATE.MOVE);
        }
    }

    private void OnTriggerExit2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
        {
            TargetObj = null;
            ChangeState(STATE.IDLE);
        }
    }

    public void Dead()
    {
        if (AM != null)
            AM.PlayOneShot(DeadSound);
        if (DeadEffect != null)
        {
            GameObject obj = Instantiate(DeadEffect);
            obj.transform.SetParent(GM.StageTR);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
        Destroy(gameObject);
    }
}
