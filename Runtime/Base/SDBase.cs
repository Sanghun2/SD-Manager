using UnityEngine;


public abstract class SDBase : ScriptableObject
{
    public string ID => id;

    [SerializeField] string id;
}
