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

        DrawZoomInButton(id, rect, "Frame this game object");

        DrawPrefabButton(id, rect, "save as prefab");

        DrawDeleteButton(id, rect, "delete GameObject from Hierarchy");
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

        GUIStyle guiStyle = new();
        guiStyle.fixedWidth = 0f;
        guiStyle.fixedHeight = 0f;
        guiStyle.stretchWidth = true;
        guiStyle.stretchHeight = true;

        Rect rect = GetDrawRect(x, y, size);
        Texture texture = Resources.Load(textureName) as Texture;

        GUIContent guiContent = new();
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

    static void DrawZoomInButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;

        DrawButtonWithTexture(
            rect.x + 175f,
            rect.y + 2,
            14,
            "zoom_in_icon",
            () =>
            {
                Selection.activeGameObject = gameObject;
                SceneView.FrameLastActiveSceneView();
            },
            gameObject,
            tooltip);
    }

    static void DrawPrefabButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;

        DrawButtonWithTexture(
            rect.x + 198f,
            rect.y,
            18,
            "prefab_icon",
            () =>
            {
                const string pathToPrefabsFolder = "Assets/Prefabs";

                bool doesPrefabsFolderExist =
                    AssetDatabase.IsValidFolder(pathToPrefabsFolder);

                if (!doesPrefabsFolderExist)
                    AssetDatabase.CreateFolder("Assets","Prefabs");

                string prefabName = $"{gameObject.name}.prefab";
                string prefabPath = $"{pathToPrefabsFolder}/{prefabName}";

                // to delete it, if it already exists
                AssetDatabase.DeleteAsset(prefabName);

                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);

                // to shwo the prefab once created and saved
                EditorGUIUtility.PingObject(prefab);
            },
            gameObject,
            tooltip);
    }

    static void DrawDeleteButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject =
            EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObject == null)
            return;

        DrawButtonWithTexture(
            rect.x + 225f,
            rect.y + 2f,
            14f,
            "delete_icon",
            () =>
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
            },
            gameObject,
            tooltip);
    }
}