using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject != null)
        {
            if (CrashObj.CompareTag("Player"))
                CrashObj.gameObject.GetComponent<Ball>().ChangeState(Ball.STATE.DEAD);
            else
                Destroy(CrashObj.gameObject);
        }
    }
}
