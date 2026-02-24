using System.Linq;
using UnityEngine;

namespace Tables
{
    public partial class Cost
    {
        public static Sprite GetSprite(JobType jobType)
        {
            return Table.FirstOrDefault(kv => kv.Value.jobType == jobType).Value.GetSprite();
        }
        
        public Sprite GetSprite() => Realm.LoadImageFromAtlas(atlasPath, iconPath);
    }
}
