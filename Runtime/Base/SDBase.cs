using UnityEngine;


namespace BilliotGames
{
    public abstract class SDBase : ScriptableObject
    {
        public string ID => id;
        public string Description => descripiton;
        public string DisplayName => displayName;

        [SerializeField] protected string id;
        [SerializeField] protected string descripiton;
        [SerializeField] protected string displayName;
    }
}
