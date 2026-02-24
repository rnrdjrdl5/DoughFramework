using UnityEngine;

namespace Tables
{
    public partial class Equipment
    {
        public Sprite GetSprite() => Realm.LoadImageFromAtlas(atlasPath, iconPath);
    }
}
