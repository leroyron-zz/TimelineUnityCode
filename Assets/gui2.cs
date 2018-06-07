using UnityEngine;
using System.Collections;

// Makes this button go back in depth over the example1 class one.

public class gui2 : MonoBehaviour
{
    public int guiDepth = 1;
    public gui1 gui1;

    void OnGUI()
    {
        GUI.depth = guiDepth;

        if (GUI.RepeatButton(new Rect(50, 50, 100, 100), "Bring Forward"))
        {
            guiDepth = 0;
            gui1.guiDepth = 1;
        }
    }
}