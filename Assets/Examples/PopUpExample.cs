using UnityEditor;
using UnityEngine;

public class PopUpExample : EditorWindow
{
    int selectedOptionIndex;
    string[] exampleOptions = { "Option 1", "Option 2", "Option 3" };

    [MenuItem("Window/PopUp Example")]
    public static void ShowWindow()
    {
        var window = GetWindow<PopUpExample>();

        var guiContent = new GUIContent
        {
            text = "PopUp Example"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    private void OnGUI()
    {
        selectedOptionIndex = EditorGUILayout.Popup(
            selectedOptionIndex,
            exampleOptions);
    }

}
