using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect_Root_01 : MonoBehaviour
{
    public float fTime = 0f;
    public float ShowCycle = 0.15f;

    public GameObject BallOBj;

    void Update()
    {
        ShowEffect(); 
    }

    void ShowEffect()
    {
        fTime += Time.smoothDeltaTime;

        if(fTime >= ShowCycle)
        {
            fTime -= ShowCycle;

            GameObject obj = Instantiate(BallOBj);
            obj.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            obj.transform.position = transform.position;
            obj.transform.rotation = gameObject.transform.rotation;
        }
    }
}
