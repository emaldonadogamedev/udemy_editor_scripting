using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class FavoritesMenu : EditorWindow
{
    const string pathToFavoritesFolder = "Assets/Favorites";
    static readonly List<GameObject> favoritedObjects = new();

    public static void AddToFavorites(GameObject gameObject)
    {
        bool doesFavoritesFolderExist =
            AssetDatabase.IsValidFolder(pathToFavoritesFolder);

        if (!doesFavoritesFolderExist)
            AssetDatabase.CreateFolder("Assets", "Favorites");

        string prefabName = gameObject.name + ".prefab";
        string prefabPath = pathToFavoritesFolder + "/" + prefabName;

        AssetDatabase.DeleteAsset(prefabName);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
        favoritedObjects.Add(prefab);
    }

    [MenuItem("Custom Tools/Favorites/Favorite Object 1", false, 50)]
    public static void HandleFavoriteIteml1Clicked()
    {
        HandleFavoriteItemClicked(0);

    }
    [MenuItem("Custom Tools/Favorites/Favorite Object 1", true)]
    public static bool ValidateFavoriteIteml1Option()
    {
        return favoritedObjects.Count >= 1;
    }

    [MenuItem("Custom Tools/Favorites/Favorite Object 2", false, 50)]
    public static void HandleFavoriteItem2lClicked()
    {
        HandleFavoriteItemClicked(1);

    }
    [MenuItem("Custom Tools/Favorites/Favorite Object 2", true)]
    public static bool ValidateFavoriteIteml2Option()
    {
        return favoritedObjects.Count >= 2;
    }

    [MenuItem("Custom Tools/Favorites/Favorite Object 3", false, 50)]
    public static void HandleFavoriteItem3lClicked()
    {
        HandleFavoriteItemClicked(2);

    }
    [MenuItem("Custom Tools/Favorites/Favorite Object 3", true)]
    public static bool ValidateFavoriteItem3Option()
    {
        return favoritedObjects.Count >= 3;
    }

    [InitializeOnLoadMethod]
    static void Setup()
    {
        favoritedObjects.Clear();
    }

    [MenuItem("Custom Tools/Favorites/Clear Favorites", false, 100)]
    public static void ClearFavoritesMenu()
    {
        favoritedObjects.Clear();
        EditorPrefs.DeleteAll();
    }

    public static void HandleFavoriteItemClicked(int index)
    {
        GameObject gameObject = favoritedObjects[index];

        GameObject cloneOfGameObject =
            PrefabUtility.InstantiatePrefab(gameObject) as GameObject;

        Camera currentSceneViewCamera = SceneView.lastActiveSceneView.camera;

        Vector3 position =
            currentSceneViewCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        cloneOfGameObject.transform.position = position;

        Selection.activeObject = cloneOfGameObject;

        SceneView currentSceneView = SceneView.lastActiveSceneView;
        currentSceneView.AlignViewToObject(cloneOfGameObject.transform);

        EditorGUIUtility.PingObject(cloneOfGameObject);
    }
}