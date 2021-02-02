using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveAllData))]
public class StageInputButton : Editor
{
    SaveAllData SD;

    void OnEnable()
    {
        SD = target as SaveAllData;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("Save Data"))
        {
            SD.SaveData();
        }
    }
}
