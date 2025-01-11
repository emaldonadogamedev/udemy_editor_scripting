using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(TextureImporter), true)]
public class ImageOptimizer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Custom Optimize Tool!"))
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
            int width;
            int height;
            importer.GetSourceTextureWidthAndHeight(out width, out height);

            int maxDimensionSize = Mathf.Max(width, height);

            TextureImporterSettings textureImporterSettings = new();
            importer.ReadTextureSettings(textureImporterSettings);

            textureImporterSettings.maxTextureSize =
                (int)Math.Pow(2, (int)Math.Log(maxDimensionSize - 1, 2) + 1);

            importer.SetTextureSettings(textureImporterSettings);

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }
}