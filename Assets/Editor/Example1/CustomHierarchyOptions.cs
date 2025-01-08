using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomHierarchyOptions
{
    static CustomHierarchyOptions()
    {
        EditorApplication.hierarchyWindowItemOnGUI +=
            HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int id, Rect rect)
    {
        Debug.Log("Editor script code called!");

        DrawActiveToggleButton(id, rect);
    }

    static void DrawActiveToggleButton(int id, Rect rect)
    {
        DrawButtonWithToggle(id, rect.x - 20, rect.y + 3, 10);
    }

    static void DrawButtonWithToggle(int id, float x, float y, float size)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;

        Rect rect = GetDrawRect(x, y, size);

        gameObject.SetActive(
            GUI.Toggle(
                rect, // where to draw the toggle
                gameObject.activeSelf, // the current value of enabled/disabled
                string.Empty)); // string for our toggle (not needed now)
    }

    static Rect GetDrawRect(float x, float y, float size)
    {
        return new Rect(x, y, size, size);
    }
}