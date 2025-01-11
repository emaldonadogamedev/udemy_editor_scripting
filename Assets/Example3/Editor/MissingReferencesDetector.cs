using UnityEditor;
using UnityEngine;

public class MissingReferencesDetector : EditorWindow
{
    [MenuItem("Custom Tools/Missing References")]
    public static void ShowWindow()
    {
        var window = GetWindow<MissingReferencesDetector>();

        window.maxSize = new Vector2(250f, 100f);
        window.minSize = window.maxSize;

        var guiContent = new GUIContent
        {
            text = "Find Missing References"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        if(GUILayout.Button("Find Missing References"))
        {
            GameObject[] gameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

            foreach(var gameObject in gameObjects)
            {
                Component[] components = gameObject.GetComponents<Component>();

                foreach(var component in components)
                {
                    SerializedObject serializedObject = new SerializedObject(component);
                    SerializedProperty serializedProperty = serializedObject.GetIterator();

                    while (serializedProperty.NextVisible(true))
                    {
                        if(serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            if(serializedProperty.objectReferenceValue == null)
                            {
                                Debug.Log("<color=red><b>Missing Reference: </b></color>" +
                                    serializedProperty.displayName + " on " +
                                    gameObject.name,
                                    gameObject);
                            }
                        }
                    }
                }
            }
        }

        EditorGUILayout.Space();
        Repaint();
    }
}
