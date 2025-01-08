using UnityEditor;
using UnityEngine;

public class Example : EditorWindow
{
    [MenuItem("Window/Example")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(Example));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Hello there!...");
        EditorGUILayout.Space();
        GUILayout.Button("This button dude!");
    }
}
