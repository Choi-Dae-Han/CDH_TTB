using System.Collections.Generic;
using UnityEngine;

public class DecreaseLifeObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D CrashObj)
    {
        if (CrashObj.gameObject.CompareTag("Player"))
            CrashObj.gameObject.GetComponent<Ball>().ChangeState(Ball.STATE.DEAD);
    }
}
