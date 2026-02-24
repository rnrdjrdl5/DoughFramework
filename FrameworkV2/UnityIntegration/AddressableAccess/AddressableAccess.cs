using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public static class AddressableAccess
{
    public static T LoadAsset<T>(string key) where T : Object
    {
        return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
    }

    public static async UniTask<T> LoadAssetAsync<T>(string key) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        return handle.Result;
    }

    public static SpriteAtlas LoadAtlas(string atlasKey)
    {
        return Addressables.LoadAssetAsync<SpriteAtlas>(atlasKey).WaitForCompletion();
    }

    public static async UniTask<SpriteAtlas> LoadAtlasAsync(string atlasKey)
    {
        var handle = Addressables.LoadAssetAsync<SpriteAtlas>(atlasKey);
        await handle.Task;
        return handle.Result;
    }

    public static Sprite LoadSpriteFromAtlas(string atlasKey, string spriteName)
    {
        var atlas = LoadAtlas(atlasKey);
        return atlas != null ? atlas.GetSprite(spriteName) : null;
    }

    public static async UniTask<Sprite> LoadSpriteFromAtlasAsync(string atlasKey, string spriteName)
    {
        var atlas = await LoadAtlasAsync(atlasKey);
        return atlas != null ? atlas.GetSprite(spriteName) : null;
    }
}

