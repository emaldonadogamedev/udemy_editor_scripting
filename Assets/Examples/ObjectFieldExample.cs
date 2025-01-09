using UnityEditor;
using UnityEngine;

public class ObjectFieldExample : EditorWindow
{
    Object obj;

    [MenuItem("Window/Object Field Example")]
    public static void ShowWindow()
    {
        var window = GetWindow<ObjectFieldExample>();

        var guiContent = new GUIContent
        {
            text = "Object Field Example"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    private void OnGUI()
    {
        obj = EditorGUILayout.ObjectField(obj, typeof(GameObject),true);
    }
}
