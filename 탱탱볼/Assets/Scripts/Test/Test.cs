using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject ball;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject obj = Instantiate(ball);
            obj.transform.position = Vector3.zero;
            obj.name = "Ball";
        }
    }
}
