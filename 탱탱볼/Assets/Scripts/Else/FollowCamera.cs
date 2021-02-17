using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public enum CAMERASTATE
    {
        CREATE, IDLE, FOLLOWING
    }
    public CAMERASTATE cameraState = CAMERASTATE.CREATE;

    public enum VSTATE
    {
        VIDLE, UP, DOWN
    }
    public VSTATE vState = VSTATE.VIDLE;

    public RectTransform BallRT;
    [SerializeField] private float Speed = 2f;
    [SerializeField] private float StayRangeX = 300f;
    [SerializeField] private float StayRangeY = 120f;
    [SerializeField] private float HighestY = 0f;
    [SerializeField] private float LowestY = 0f;
    public Vector3 LastBallPos = Vector3.zero;
    public Vector3 GoingTarget = Vector3.zero;

    private void Awake()
    {
        GoingTarget = new Vector3(0f, 0f, -100f);
    }

    void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch (cameraState)
        {
            case CAMERASTATE.CREATE:
                ChangeState(CAMERASTATE.FOLLOWING);
                break;
            case CAMERASTATE.IDLE:
                break;
            case CAMERASTATE.FOLLOWING:
                if (BallRT != null)
                    LastBallPos = BallRT.position;
                FollowObject();
                break;
        }
    }

    public void ChangeState(CAMERASTATE s)
    {
        if(cameraState == s) return;
        cameraState = s;

        switch (s)
        {
            case CAMERASTATE.CREATE:
                break;
            case CAMERASTATE.IDLE:
                ChangeVState(VSTATE.VIDLE);
                break;
            case CAMERASTATE.FOLLOWING:
                break;
        }
    }

    public void ChangeVState(VSTATE s)
    {
        if (vState == s) return;
        vState = s;

        switch (s)
        {
            case VSTATE.VIDLE:
                GoingTarget = transform.position;
                break;
            case VSTATE.UP:
                LowestY = 0f;
                HighestY = transform.position.y + StayRangeY;
                break;
            case VSTATE.DOWN:
                HighestY = 0f;
                LowestY = transform.position.y - StayRangeY;
                break;
        }
    }

    private void FollowObject()
    {
        if (LastBallPos.x > transform.position.x + StayRangeX)
            GoingTarget.x = LastBallPos.x - StayRangeX;
        else if (LastBallPos.x < transform.position.x - StayRangeX)
            GoingTarget.x = LastBallPos.x + StayRangeX;

        if (LastBallPos.y > transform.position.y + StayRangeY)
            ChangeVState(VSTATE.UP);
        else if (LastBallPos.y < transform.position.y - StayRangeY)
            ChangeVState(VSTATE.DOWN);

        switch (vState)
        {
            case VSTATE.UP:
                if (LastBallPos.y > HighestY) HighestY = LastBallPos.y;
                GoingTarget.y = HighestY - StayRangeY;
                break;
            case VSTATE.DOWN:
                if (LastBallPos.y < LowestY) LowestY = LastBallPos.y;
                GoingTarget.y = LowestY + StayRangeY;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, GoingTarget, Speed * Time.smoothDeltaTime);
    }
}
