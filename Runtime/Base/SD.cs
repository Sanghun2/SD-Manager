using UnityEngine;

[CreateAssetMenu(fileName = "SD", menuName = "Scriptable Objects/SD")]
public class SD : ScriptableObject
{
    public string ID => id;

    [SerializeField] string id;
}
