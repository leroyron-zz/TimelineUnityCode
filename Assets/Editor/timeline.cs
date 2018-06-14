using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(timeline))]
public class timelineInspector : Editor {
    public GUIStyle whiteText = new GUIStyle();
    
    void OnSceneGUI() {
        whiteText.normal.textColor = Color.white;

        Handles.BeginGUI();
        
        GUILayout.BeginArea(new Rect(5, 5, Screen.width-10, 60));
        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.black;
        GUI.Box(new Rect(0, 0, Screen.width-10, 20), GUIContent.none);
        GUI.color = Color.grey;
        GUI.Box(new Rect(0, 20, Screen.width-10, 20), GUIContent.none);
        
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Rotate", whiteText);
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

[CustomPropertyDrawer(typeof(TIMELINEAttribute))]
public class TIMELINEDrawer : PropertyDrawer
{   
    GUIContent nameText = new GUIContent("name");
    GUIContent muteText = new GUIContent("mute");
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label and calculate new position
        position = EditorGUI.PrefixLabel(position,
                                            GUIUtility.GetControlID(FocusType.Passive),
                                            new GUIContent(((TIMELINEAttribute)attribute).names[pos]));

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate positions and dimensions for the GUI elements
        //  - min-field       ... X-pos:  0%, width: 40%  \
        //  - mirror-checkbox ... X-pos: 40%, width: 20%   |> 100% of available width
        //  - max-field       ... X-pos: 60%, width: 40%  /
        float p1 = position.x;
        float w1 = 35;
        Rect nameLabelRect = new Rect(p1,
                                position.y,
                                w1,
                                position.height);
        float p2 = p1 + w1;
        float w2 = position.width * 0.30F;
        Rect nameRect = new Rect(p2,
                                position.y,
                                w2,
                                position.height);
        float p3 = p2 + w2;
        float w3 = 35;
        Rect muteLabelRect = new Rect(p3,
                                        position.y,
                                        w3,
                                        position.height);
        float p4 = p3 + w3;
        float w4 = position.width * 0.20F;
        Rect muteRect = new Rect(p4,
                                position.y,
                                w4,
                                position.height);

        // Get properties by exactly passing the names of the interval's attributes
        SerializedProperty nameProp = property.FindPropertyRelative("name");
        SerializedProperty muteProp = property.FindPropertyRelative("mute");

        timeline cpTarget = (timeline)property.serializedObject.targetObject;

        bool first = pos == 0;
        bool last = pos == cpTarget.timeLines.Length - 1;

        // Draw minimum-field - pass GUIContent.none to not draw the
        // label of the property
        EditorGUI.PrefixLabel(nameLabelRect, nameText);
        EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);
        EditorGUI.PrefixLabel(muteLabelRect, muteText);
        // Draw maximum-field
        EditorGUI.PropertyField(muteRect, muteProp, GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
        int cpLength = cpTarget.timeLines.Length;
        if (first) {
            //cpTarget.update();
        }
        if (last) {
            
        }
    }
}