using UnityEngine;


namespace BilliotGames
{
    public abstract class SDBase : ScriptableObject
    {
        public string ID => id;
        public string DisplayName => displayName;
        public string Description => descripiton;

        [SerializeField] protected string id;
        [SerializeField] protected string displayName;
        [TextArea(1, 10)]
        [SerializeField] protected string descripiton;
    }
}
