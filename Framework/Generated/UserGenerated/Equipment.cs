using UnityEngine;

namespace Tables
{
    public partial class Equipment
    {
        public Sprite GetSprite() => Universe.LoadImageFromAtlas(atlasPath, iconPath);
    }
}
