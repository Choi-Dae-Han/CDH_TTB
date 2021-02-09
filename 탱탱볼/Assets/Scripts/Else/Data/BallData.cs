using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData
{
    public AudioClip SE;
    public Sprite Skin;
    public GameObject Effect;

    public BallData(GameObject effect = null, AudioClip clip = null, Sprite skin = null)
    {
        SE = clip;
        Skin = skin;
        Effect = effect;
    }
}
