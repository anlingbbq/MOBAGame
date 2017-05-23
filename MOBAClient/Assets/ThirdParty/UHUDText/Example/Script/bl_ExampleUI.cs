using UnityEngine;
using System.Collections;

public class bl_ExampleUI : MonoBehaviour {

    public GUIStyle Style;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 0, 300, 100),"","box");
        GUILayout.Space(5);
        GUILayout.Label("  - click spheres to see the <color=orange>example</color>",Style);
        GUILayout.Space(20);
        GUILayout.Label("  - hold down the left click \n    to rotate with the mouse.",Style);
        GUILayout.EndArea();
    }
}
