using UnityEngine;

namespace BilliotGames
{
    public abstract class ImageSDBase : SDBase
    {
        public Sprite Image => image;

        [SerializeField] protected Sprite image;
    }
}
