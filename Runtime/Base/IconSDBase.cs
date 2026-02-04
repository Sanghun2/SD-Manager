using UnityEngine;

namespace BilliotGames
{
    public abstract class IconSDBase : SDBase
    {
        public Sprite IconImage => iconImage;

        [SerializeField] Sprite iconImage;
    }
}
