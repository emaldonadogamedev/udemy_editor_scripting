using UnityEditor;
using UnityEngine;

public class BatchRenameToolWindow : EditorWindow
{
    string batchName = string.Empty;
    string batchNumber = string.Empty;
    bool showOptions = true;

    [MenuItem("Window/Batch Rename")]
    public static void ShowWindow()
    {
        var window = GetWindow<BatchRenameToolWindow>();

        window.maxSize = new Vector2(500f, 150f);
        window.minSize = window.maxSize;

        var guiContent = new GUIContent
        {
            text = "Batch Rename"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    public void OnGUI()
    {
        DrawStep1();

        DrawStep2();

        DrawStep3();

        Repaint();
    }

    private void DrawStep1()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(
            "Step 1: Select obects in the hierarchy",
            EditorStyles.boldLabel);
    }

    private void DrawStep2()
    {
        EditorGUILayout.Space();

        GUIStyle guiStyle = new(EditorStyles.foldout);
        guiStyle.fontStyle = FontStyle.Bold;
        showOptions = EditorGUILayout.Foldout(
            showOptions,
            "Step 2: Enter rename info",
            guiStyle);

        if (!showOptions)
            return;
         
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("\tEnter name for batch");
        batchName = EditorGUILayout.TextField(batchName);
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("\tEnter starting number");
        batchNumber = EditorGUILayout.TextField(batchNumber);
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

    }

    private void DrawStep3()
    {
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Step 3: Click the rename button",
            EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if(GUILayout.Button("Rename"))
        {
            if(!int.TryParse(batchNumber, out int number) || number < 0)
            {
                number = 1;
            }

            foreach(GameObject go in Selection.objects)
            {
                go.name = $"{batchName}_{number}";
                number++;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
