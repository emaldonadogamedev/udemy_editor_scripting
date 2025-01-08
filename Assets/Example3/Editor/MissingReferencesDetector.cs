using UnityEditor;
using UnityEngine;

public class MissingReferencesDetector : EditorWindow
{
    [MenuItem("Window/Missing References")]
    public static void ShowWindow()
    {
        var window = GetWindow<MissingReferencesDetector>();

        window.maxSize = new Vector2(250f, 100f);
        window.minSize = window.maxSize;

        var guiContent = new GUIContent
        {
            text = "Find Missing References"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    public void OnGUI()
    {

    }
}
