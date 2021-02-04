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
    public Vector3 GoingTarget = Vector3.zero;
    [SerializeField] private Transform BackGround;

    private void Awake()
    {
        GoingTarget.z = -100f;
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
                {
                    if (BallRT.position.y > transform.position.y + StayRangeY)
                        ChangeVState(VSTATE.UP);
                    else if (BallRT.position.y < transform.position.y - StayRangeY)
                        ChangeVState(VSTATE.DOWN);
                    FollowObject();
                }
                else ChangeState(CAMERASTATE.IDLE);
                break;
        }

        switch (vState)
        {
            case VSTATE.VIDLE:
                break;
            case VSTATE.UP:
                if (BallRT != null && BallRT.position.y < transform.position.y - StayRangeY)
                    ChangeVState(VSTATE.DOWN);
                break;
            case VSTATE.DOWN:
                if (BallRT != null && BallRT.position.y > transform.position.y + StayRangeY)
                    ChangeVState(VSTATE.UP);
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
        if (BallRT.position.x > transform.position.x + StayRangeX)
            GoingTarget.x = BallRT.position.x - StayRangeX;
        else if (BallRT.position.x < transform.position.x - StayRangeX)
            GoingTarget.x = BallRT.position.x + StayRangeX;
        if (BallRT.position.y > transform.position.y + StayRangeY)
        {
            if (BallRT.position.y > HighestY) HighestY = BallRT.position.y;
            GoingTarget.y = HighestY - StayRangeY;
        }
        else if (BallRT.position.y < transform.position.y - StayRangeY)
        {
            if (BallRT.position.y < LowestY) LowestY = BallRT.position.y;
            GoingTarget.y = LowestY + StayRangeY;
        }
       // Vector3 Temp = transform.position;
        transform.position = Vector3.Lerp(transform.position, GoingTarget, Speed * Time.smoothDeltaTime);
        //MovedDist = transform.position - Temp;
        //BackGround.transform.Translate(-MovedDist / 10f);
    }
}
