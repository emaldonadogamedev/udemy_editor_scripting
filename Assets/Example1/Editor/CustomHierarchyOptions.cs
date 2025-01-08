using UnityEditor;
using UnityEngine;
using System;

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

        AddGameObjectInfoScriptToGameObject(id);
        DrawInfoButton(id, rect, string.Empty);
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

    static void DrawButtonWithTexture(
        float x,
        float y,
        float size,
        string textureName,
        Action action,
        GameObject gameObject,
        string tooltip)
    {
        if (gameObject == null)
            return;

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fixedWidth = 0f;
        guiStyle.fixedHeight = 0f;
        guiStyle.stretchWidth = true;
        guiStyle.stretchHeight = true;

        Rect rect = GetDrawRect(x, y, size);
        Texture texture = Resources.Load(textureName) as Texture;

        GUIContent guiContent = new GUIContent();
        guiContent.image = texture;
        guiContent.text = string.Empty;
        guiContent.tooltip = tooltip;


        bool isClicked = GUI.Button(rect, guiContent, guiStyle);
        if(isClicked)
        {
            action.Invoke();
        }
    }

    static void DrawInfoButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;

        if(gameObject.TryGetComponent<GameObjectInfo>(out var gameObjectInfo))
        {
            tooltip = gameObjectInfo.GameObjectInfoText;
        }

        DrawButtonWithTexture(
            rect.x + 150,
            rect.y + 2,
            14,
            "info_icon",
            () => { },
            gameObject,
            tooltip);
    }

    static void AddGameObjectInfoScriptToGameObject(int id)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;
        if (!gameObject.TryGetComponent<GameObjectInfo>(out _))
        {
            gameObject.AddComponent<GameObjectInfo>();
        }
    }
}