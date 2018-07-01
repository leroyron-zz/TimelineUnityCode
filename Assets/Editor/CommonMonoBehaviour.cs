using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(TimelineCode))]
public class CommonMonoBehaviourInspector : Editor {
    void OnSceneGUI()
    {
        Handles.BeginGUI();
        ((CommonMonoBehaviour)this.target).OnSceneCallGUI();
        Handles.EndGUI();
    }
}