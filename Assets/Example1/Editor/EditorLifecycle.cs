using UnityEditor;
using UnityEngine;

public class EditorLifecycle : EditorWindow
{

    [MenuItem("Window/Editor Script Lifecycle")]
    public static void ShowExample()
    {
        EditorLifecycle wnd = GetWindow<EditorLifecycle>();
        wnd.titleContent = new GUIContent("Editor Script Lifecycle");
    }

    public void Awake()
    {
        Debug.Log($"{nameof(Awake)}() function called");
    }

    public void CreateGUI()
    {
        Debug.Log($"{nameof(CreateGUI)}() function called");
    }

    public void OnBecameVisible()
    {
        Debug.Log($"{nameof(OnBecameVisible)}() function called");
    }

    public void OnFocus()
    {
        Debug.Log($"{nameof(OnFocus)}() function called");
    }

    public void OnGUI()
    {
        Debug.Log($"{nameof(OnGUI)}() function called");
    }

    public void OnHierarchyChange()
    {
        Debug.Log($"{nameof(OnHierarchyChange)}() function called");
    }

    public void OnInspectorUpdate()
    {
        Debug.Log($"{nameof(OnInspectorUpdate)}() function called");
    }
}
