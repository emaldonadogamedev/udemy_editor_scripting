using UnityEngine;

public class GameObjectInfo : MonoBehaviour
{
    [Header("Description of this GameObject")]
    [SerializeField]
    private string gameObjectInfo;

    public string GameObjectInfoText => gameObjectInfo;
}