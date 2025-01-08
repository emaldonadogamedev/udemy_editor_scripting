using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AutoLinker : Editor
{
    static Dictionary<string, GameObject> nameToGameObjectMap;
    static Dictionary<string, SerializedObject> fieldNameToSerliazedPropertyMap;

    [InitializeOnLoadMethod]
    public static void Setup()
    {
        nameToGameObjectMap = new Dictionary<string, GameObject>();
        SetupHierarchyMap();

        fieldNameToSerliazedPropertyMap =
            new Dictionary<string, SerializedObject>();
        SetupInspectorMap();

        HandleAutoLinking();
    }

    static void SetupHierarchyMap()
    {
        GameObject[] gameObjects =
            FindObjectsByType<GameObject>(FindObjectsSortMode.InstanceID);

        foreach(var gameObject in gameObjects)
        {
            Debug.Log($"{gameObject.name}");

            string key = gameObject.name.ToLower().Replace(" ", string.Empty);

            nameToGameObjectMap.Add(key, gameObject);
        }
    }

    static void SetupInspectorMap()
    {
        foreach(GameObject gameObject in nameToGameObjectMap.Values)
        {
            Component[] components = gameObject.GetComponents<Component>();

            foreach(Component component in components)
            {
                var serializedObject = new SerializedObject(component);
                var serializedProperty = serializedObject.GetIterator();

                while(serializedProperty.NextVisible(true))
                {
                    string key =
                        serializedProperty.displayName.ToLower().Replace(" ", string.Empty);

                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if(!fieldNameToSerliazedPropertyMap.ContainsKey(key))
                            fieldNameToSerliazedPropertyMap.Add(key, serializedObject);
                    }
                }
            }
        }
    }

    static void HandleAutoLinking()
    {
        foreach(string name in fieldNameToSerliazedPropertyMap.Keys)
        {
            string key = name.ToLower().Replace(" ", string.Empty);

            if(nameToGameObjectMap.ContainsKey(key))
            {
                var sp = fieldNameToSerliazedPropertyMap[key].FindProperty(key);
                sp.objectReferenceValue = nameToGameObjectMap[key];

                sp.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
