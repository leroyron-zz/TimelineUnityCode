using UnityEngine;
using UnityEditor;
 
[CustomEditor(typeof(sgui))]
public class sguiInspector : Editor {
 
    void OnSceneGUI() {
        Handles.BeginGUI();
        
        GUILayout.BeginArea(new Rect(20, 20, 150, 60));
        
        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.yellow;
        GUI.Box(rect, GUIContent.none);
        
        GUI.color = Color.white;
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Rotate");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.red;
        
        if (GUILayout.Button("Left")) {
            RotateLeft();
        }
        
        if (GUILayout.Button("Right")) {
            RotateRight();
        }
        
        GUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        
        
        GUILayout.EndArea();
        
        Handles.EndGUI();
    }
    
    void RotateLeft() {
        (target as Component).transform.Rotate(Vector3.down, 15);
    }
    
    void RotateRight() {
        (target as Component).transform.Rotate(Vector3.down, -15);
    }
 
}