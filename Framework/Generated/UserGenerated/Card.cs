using System.Linq;
using UnityEngine;

namespace Tables
{
    public partial class Card
    {
        // Add your custom logic here.
        public Sprite GetSprite() => Realm.LoadImageFromAtlas(atlasPath, iconPath);
        public Skill SkillData => Skill.Get(skillKey);
    }
}
