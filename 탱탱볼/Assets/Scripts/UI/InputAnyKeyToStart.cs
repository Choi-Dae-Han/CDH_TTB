﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAnyKeyToStart : MonoBehaviour
{
    public void ChangeGMState(int state)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().ChangeGameState((GameManager.GAMESTATE)state);
    }
}
