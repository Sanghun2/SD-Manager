using UnityEngine;

namespace BilliotGames
{
    public abstract class ImageSDBase : SDBase
    {
        public virtual Sprite Image => image;

        [SerializeField] protected Sprite image;
    }
}
