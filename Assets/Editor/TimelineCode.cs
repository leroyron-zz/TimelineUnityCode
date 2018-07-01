using UnityEngine;
using UnityEditor;
using System;
using FFE;

[CustomPropertyDrawer(typeof(TimelineCodeAttribute))]
public class TimelineDrawer : PropertyDrawer
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
                                            new GUIContent(((TimelineCodeAttribute)attribute).names[pos]));

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
        float w2 = position.width * 0.30f;
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
        float w4 = position.width * 0.20f;
        Rect muteRect = new Rect(p4,
                                position.y,
                                w4,
                                position.height);

        // Get properties by exactly passing the names of the interval's attributes
        SerializedProperty nameProp = property.FindPropertyRelative("name");
        SerializedProperty muteProp = property.FindPropertyRelative("mute");

        TimelineCode tlTarget = (TimelineCode)property.serializedObject.targetObject;

        bool first = pos == 0;
        bool last = pos == tlTarget.Timelines.Length - 1;

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
        int cpLength = tlTarget.Timelines.Length;
        if (first) {
            //tlTarget.Update();
        }
        if (last) {
            
        }
    }
}


[InitializeOnLoad]
public class SingleEntryPoint
{
	static bool debugOp = false;
	
	static SingleEntryPoint()
	{
		TimelineCode.Log("SingleEntryPoint. Up and running");
		EditorPlayMode.PlayModeChanged += OnPlayModeChanged;
	}
	
	private static void OnPlayModeChanged(PlayModeState currentMode, PlayModeState changedMode)
	{
		// DO your stuff here...
        if (changedMode == PlayModeState.AboutToStop || changedMode == PlayModeState.Stopped) 
            TimelineCode.running = false;
        //if (changedMode == PlayModeState.Playing)   
            //TimelineCode.running = true;
		TimelineCode.Log(currentMode.ToString() + " => " + changedMode.ToString());
		
		if (debugOp) {
			TimelineCode.Log(EditorApplication.isCompiling);
			TimelineCode.Log(EditorApplication.isPaused);
			TimelineCode.Log(EditorApplication.isPlaying);
			TimelineCode.Log(EditorApplication.isPlayingOrWillChangePlaymode);
			TimelineCode.Log(EditorApplication.isUpdating);
			Debug.LogWarning("-------------------------------------");
		}
	}
}

namespace FFE {

	public enum PlayModeState
	{
		Stopped,
		Playing,
		Paused,
		AboutToStop,
		AboutToPlay
	}
	
	[InitializeOnLoad]
	public class EditorPlayMode
	{
		private static PlayModeState _currentState = PlayModeState.Stopped;
		
		static EditorPlayMode()
		{
			EditorApplication.playmodeStateChanged = OnUnityPlayModeChanged;
			if (EditorApplication.isPaused) 
				_currentState = PlayModeState.Paused;
		}
		
		static int Bool2Int(bool b) {if (b) return 1; else return 2;}
		
		static int GetEditorAppStateBoolComb() {
			int b1 = Bool2Int(EditorApplication.isUpdating);
			int b2 = Bool2Int(EditorApplication.isPlayingOrWillChangePlaymode);
			int b3 = Bool2Int(EditorApplication.isPlaying);
			int b4 = Bool2Int(EditorApplication.isPaused);
			int b5 = Bool2Int(EditorApplication.isCompiling);
			return b1 + b2 * 10 + b3 * 100 + b4 * 1000 + b5 * 10000;
		}
		public static event Action<PlayModeState, PlayModeState> PlayModeChanged;
		
		private static void OnPlayModeChanged(PlayModeState currentState, PlayModeState changedState)
		{
			if (PlayModeChanged != null)
				PlayModeChanged(currentState, changedState);
		}
		
		private static void OnUnityPlayModeChanged()
		{
			
			var changedState = PlayModeState.Stopped;
			
			//Stoped -> Playing : 22112
			//playing -> abouttostop : 22122
			//about2stop -> stopped : 22222
			//playing -> paused : 21112
			//paused -> playing : 22112
			//paused -> abouttostop : 21122
			//stoped -> paused in editor (stoped) : 21222
			//editor paused - > play(paused) : 21112
			//stoped -> abouttoplay in editor: 22212
			//editor paused -> play : 21212 //paused anyway
			
			int state = GetEditorAppStateBoolComb();
			switch (state) {
			case (22112):
				changedState = PlayModeState.Playing;
				break;
			case (21112):
				changedState = PlayModeState.Paused;
				break;
			case (22222):
				changedState = PlayModeState.Stopped;
				break;
			case (22122):
				changedState = PlayModeState.AboutToStop;
				break;
			case (21122):
				changedState = PlayModeState.AboutToStop;
				break;
			case (21222):
				changedState = PlayModeState.Stopped;
				break;
			case 22212:
				changedState = PlayModeState.Stopped;
				break;
			case 21212:
				changedState = PlayModeState.Paused;
				break;
			default:
				throw new SystemException("No such state combination defined: "+state);
			}
			
			// Fire PlayModeChanged event.
			if (_currentState!=changedState)
				OnPlayModeChanged(_currentState, changedState);
			
			// Set current state.
			_currentState = changedState;
			
			
		}
		
	}

}
