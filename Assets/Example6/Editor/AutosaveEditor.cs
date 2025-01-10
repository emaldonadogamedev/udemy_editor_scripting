using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AutosaveEditor : EditorWindow
{
    const string menuOption = "File/Autosave";

    static EditorWindow window;

    int choice;

    const string ONE_SECOND = "1 second";
    const string THIRTY_SECONDS = "30 seconds";
    const string ONE_MINUTE = "1 minute";
    const string FIVE_MINUTES = "5 minutes";

    string[] choices =
    {
        ONE_SECOND,
        THIRTY_SECONDS,
        ONE_MINUTE,
        FIVE_MINUTES
    };

    float saveTime = 1f;
    float nextSave = 0f;

    public static bool IsEnabled
    {
        get { return EditorPrefs.GetBool(menuOption, false); }
        set { EditorPrefs.SetBool(menuOption, value); }
    }

    [MenuItem(menuOption, false, 175)]
    public static void ToggleAutosave()
    {
        IsEnabled = !IsEnabled;

        if(IsEnabled)
        {
            ShowWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    [MenuItem(menuOption, true)]
    private static bool IsToggleActionValid()
    {
        Menu.SetChecked(menuOption, IsEnabled);
        return true;
    }

    private static void ShowWindow()
    {
        window = GetWindow<AutosaveEditor>();

        var guiContent = new GUIContent
        {
            text = "Autosave Settings"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    private static void CloseWindow()
    {
        if(window != null)
        {
            window.Close();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Interval: ");
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        choice = EditorGUILayout.Popup("", choice, choices);
        if(EditorGUI.EndChangeCheck())
        {
            switch(choices[choice])
            {
                case ONE_SECOND:
                    saveTime = 1f;
                    break;
                case THIRTY_SECONDS:
                    saveTime = 30f;
                    break;
                case ONE_MINUTE:
                    saveTime = 60f;
                    break;
                case FIVE_MINUTES:
                    saveTime = 300f;
                    break;
            }

            nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
        }

        if (IsEnabled)
        {
            float timeToSave = nextSave - (float)EditorApplication.timeSinceStartup;

            if (timeToSave <= 0f)
            {
                string[] path =
                    EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));

                bool saveSuccess = EditorSceneManager.SaveScene(
                    EditorSceneManager.GetActiveScene(),
                    string.Join("/", path));

                nextSave = (float)EditorApplication.timeSinceStartup + saveTime;

                Debug.Log("Was save successful? " + saveSuccess);
            }

            EditorGUILayout.LabelField($"Time until Next Save: {timeToSave}");
        }

        Repaint();
    }
}
