using UnityEngine;
using System.Collections;

// Makes this button go back in depth over the example1 class one.

public class gui1 : MonoBehaviour
{
    public int guiDepth = 1;
    public gui2 gui2;

    void OnGUI()
    {
        GUI.depth = guiDepth;

        if (GUI.RepeatButton(new Rect(0, 0, 100, 100), "Bring Forward"))
        {
            guiDepth = 0;
            gui2.guiDepth = 1;
        }
    }
}