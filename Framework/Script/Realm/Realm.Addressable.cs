using Cysharp.Threading.Tasks;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

// TODO : 추후 Addressable로 교체 필요
public partial class Realm
{
    public static ResourcesType LoadResources<ResourcesType> (string resourcePath) where ResourcesType : UnityEngine.Object
    {
        return Addressables.LoadAssetAsync<ResourcesType>($"{resourcePath}").WaitForCompletion();
    }
    
    public static async UniTask<ResourcesType> LoadResourceAsync<ResourcesType>(string key)
    {
        AsyncOperationHandle<ResourcesType> handle = Addressables.LoadAssetAsync<ResourcesType>(key);
        await handle.Task;
        
        return handle.Result;
    }
    
    public static SpriteAtlas LoadAtlas(string atlasPath)
    {
        return Addressables.LoadAssetAsync<SpriteAtlas>(atlasPath).WaitForCompletion();
    }
    
    public static async UniTask<SpriteAtlas> LoadAtlasAsync(string atlasPath)
    {
        var handler = Addressables.LoadAssetAsync<SpriteAtlas>(atlasPath);
        await handler.Task;
        
        return handler.Result;
    }

    public static Sprite LoadImageFromAtlas(string atlasPath, string imagePath)
    {
        var atlas = LoadAtlas(atlasPath);

        return atlas != null ? atlas.GetSprite(imagePath) : null;
    }

    public static async UniTask<Sprite> LoadImageAsync(string imagePath)
    {
        var handler = Addressables.LoadAssetAsync<Sprite>(imagePath);
        await handler.Task;
        
        return handler.Result;
    }
    
    public static Sprite LoadImage(string imagePath)
    {
        return Addressables.LoadAssetAsync<Sprite>(imagePath).WaitForCompletion();
    }
    
    public static async UniTask<Sprite> LoadImageFromAtlasAsync(string atlasPath, string imagePath)
    {
        var atlas = await LoadAtlasAsync(atlasPath);

        return atlas != null ? atlas.GetSprite(imagePath) : null;
    }
}