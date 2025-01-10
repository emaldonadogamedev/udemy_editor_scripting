using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class FavoritesMenu : EditorWindow
{
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
    }
}