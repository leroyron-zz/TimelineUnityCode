using UnityEngine;
using UnityEditor;

/*
[CustomEditor(typeof(controlpoints))]
public class controlpointsInspector : Editor {
 
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
*/

[CustomPropertyDrawer(typeof(ControlPointAttribute))]
public class ControlPointDrawer : PropertyDrawer
{
    GUIContent timeValueLabel = new GUIContent("time:value");
    float[] times;
    float[] values;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label and calculate new position
        position = EditorGUI.PrefixLabel(position,
                                GUIUtility.GetControlID(FocusType.Passive),
                                new GUIContent(((ControlPointAttribute)attribute).names[pos]));

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate positions and dimensions for the GUI elements
        //  - min-field       ... X-pos:  0%, width: 40%  \
        //  - mirror-checkbox ... X-pos: 40%, width: 20%   |> 100% of available width
        //  - max-field       ... X-pos: 60%, width: 40%  /
        float p1 = position.x;
        float w1 = position.width * 0.25f;
        Rect timeRect = new Rect(p1,
                                position.y,
                                w1,
                                position.height);
        float p2 = p1 + w1;
        float w2 = 70;
        Rect timeValueRect = new Rect(p2,
                                position.y,
                                w2,
                                position.height);
        float p3 = p2 + w2;
        float w3 = position.width * 0.25f;
        Rect valueRect = new Rect(p3,
                                position.y,
                                w3,
                                position.height);

        // Get properties by exactly passing the names of the interval's attributes
        SerializedProperty timeProp = property.FindPropertyRelative("time");
        SerializedProperty valueProp = property.FindPropertyRelative("value");
        SerializedProperty indexProp = property.FindPropertyRelative("index");

        SerializedProperty firstProp = property.FindPropertyRelative("first");
        SerializedProperty lastProp = property.FindPropertyRelative("last");

        controlpoints cpTarget = (controlpoints)property.serializedObject.targetObject;

        bool first = pos == 0;
        bool last = pos == cpTarget.controlPoints.Length - 1;

		timeProp.floatValue = timeProp.floatValue > 1f ? 1f : timeProp.floatValue < 0f ? 0f : timeProp.floatValue;
        timeProp.floatValue = first ? 0f : last ? 1f : timeProp.floatValue;
        valueProp.floatValue = valueProp.floatValue > 1f ? 1f : valueProp.floatValue < 0f ? 0f : valueProp.floatValue;

        // Draw minimum-field - pass GUIContent.none to not draw the
        // label of the property
        EditorGUI.PropertyField(timeRect, timeProp, GUIContent.none);
        EditorGUI.PrefixLabel(timeValueRect, timeValueLabel);
        // Draw maximum-field
        EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
        int cpLength = cpTarget.controlPoints.Length;
        if (first) {
            cpTarget.update();
            times = new float[cpLength];
            values = new float[cpLength];
            cpTarget.curve = new AnimationCurve();
        }
        if (pos < times.Length) { 
            times[pos] = timeProp.floatValue;
            values[pos] = valueProp.floatValue;
        }
        if (last) {
            for (int l = 0; l < cpLength; l++) {
                if (l < cpLength-1)
                    cpTarget.curve.AddKey(times[l], values[l]);
                else
                    cpTarget.curve.AddKey(times[cpLength-1], values[cpLength-1]);
            }
            cpTarget.list = cpTarget.CurveToString(cpTarget.curve);
            cpTarget.sample();
        }
    }
}