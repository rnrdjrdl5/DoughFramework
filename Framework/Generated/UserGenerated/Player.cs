using System.Linq;
using UnityEngine;

namespace Tables
{
    public partial class Player
    {
        // Add your custom logic here.

        public Sprite GetSprite()
        {
            return Universe.LoadImageFromAtlas(atlasPath, spritePath);
        }
    }
}
