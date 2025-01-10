using UnityEditor;
using UnityEngine;

public class SavingEditorData : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void SaveData()
    {
        EditorPrefs.SetBool("boolValue", true);
        EditorPrefs.SetInt("intValue", 5);
        EditorPrefs.SetString("stringValue", "example");
        EditorPrefs.SetFloat("floatValue", 3.456f);

        ReadAndPrintData();
    }

    static void ReadAndPrintData()
    {
        Debug.Log("Bool value is " + EditorPrefs.GetBool("boolValue"));
        Debug.Log("int value is " + EditorPrefs.GetInt("intValue"));
        Debug.Log("string value is " + EditorPrefs.GetString("stringValue"));
        Debug.Log("float value is " + EditorPrefs.GetFloat("floatValue"));
    }
}
