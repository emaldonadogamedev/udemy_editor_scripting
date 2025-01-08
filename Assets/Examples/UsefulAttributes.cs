using UnityEngine;
using UnityEditor;

public class UsefulAttributes : MonoBehaviour
{
    [InitializeOnLoadMethod]
    public static void Setup()
    {
        Debug.Log("--- Hit2 ---");
    }
}
