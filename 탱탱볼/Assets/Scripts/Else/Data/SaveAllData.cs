using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAllData : MonoBehaviour
{
    public List<Stage> Stages = new List<Stage>();
    public List<ButtonFunction> Buttons = new List<ButtonFunction>();
    public void SaveData()
    {
        for(int i = 0; i < Stages.Count; ++i)
        {
            Stages[i].SaveStageData(false, 0, false);
        }

        for(int i = 0; i < Buttons.Count; ++i)
        {
            Buttons[i].SaveButtonState(0);
        }
    }
}
