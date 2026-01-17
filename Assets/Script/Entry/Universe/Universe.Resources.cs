// using Cysharp.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.U2D;
// using UnityEngine.UI;
//
// // TODO : 추후 Addressable로 교체 필요
// public partial class Universe
// {
//     public static ResourcesType LoadResources<ResourcesType> (string resourcePath) where ResourcesType : UnityEngine.Object
//     {
//         return Resources.Load<ResourcesType>(resourcePath);
//     }
//
//     public static async UniTask<ResourcesType> LoadResourceAsync<ResourcesType>(string resourcePath) where ResourcesType : UnityEngine.Object
//     {
//         var resource = await Resources.LoadAsync<ResourcesType>(resourcePath);
//         
//         return resource as ResourcesType;
//     }
//
//     public static SpriteAtlas LoadAtlas(string atlasPath)
//     {
//         return Resources.Load<SpriteAtlas>(atlasPath);
//     }
//     
//     public static Sprite LoadImageFromAtlas(string atlasPath, string imagePath)
//     {
//         var atlas = LoadAtlas(atlasPath);
//
//         return atlas != null ? atlas.GetSprite(imagePath) : null;
//     }
//     
//     
// }
