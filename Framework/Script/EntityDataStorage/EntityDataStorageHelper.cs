using System;

public static class EntityDataStorageHelper
{
    static readonly EntityDataStorage AesStorage = new EntityDataStorage();
    static readonly EntityDataObfuscationStorage ObfuscationStorage = new EntityDataObfuscationStorage();

    public static string ToJson(Entity entity) => AesStorage.ToJson(entity);

    public static string ToEncryptedJson(Entity entity, string keyBase64, string ivBase64)
        => AesStorage.ToEncryptedJson(entity, keyBase64, ivBase64);

    public static void SaveEncrypted(Entity entity, string saveKey, string keyBase64, string ivBase64, string relativeDirectory = null)
        => AesStorage.Save(entity, saveKey, keyBase64, ivBase64, relativeDirectory);

    public static bool LoadEncrypted(Entity entity, string saveKey, string keyBase64, string ivBase64, string relativeDirectory = null)
        => AesStorage.Load(entity, saveKey, keyBase64, ivBase64, relativeDirectory);

    public static bool LoadFromJson(Entity entity, string json)
        => AesStorage.LoadFromJson(entity, json);

    public static bool LoadFromEncryptedJson(Entity entity, string encryptedJson, string keyBase64, string ivBase64)
        => AesStorage.LoadFromEncryptedJson(entity, encryptedJson, keyBase64, ivBase64);

    public static void SetObfuscationKey(string key)
        => EntityDataObfuscationStorage.SetKey(key);

    public static string ToObfuscatedJson(Entity entity, string key = null)
        => ObfuscationStorage.ToObfuscatedJson(entity, key);

    public static void SaveObfuscated(Entity entity, string saveKey, string key = null, string relativeDirectory = null)
        => ObfuscationStorage.Save(entity, saveKey, key, relativeDirectory);

    public static bool LoadObfuscated(Entity entity, string saveKey, string key = null, string relativeDirectory = null)
        => ObfuscationStorage.Load(entity, saveKey, key, relativeDirectory);

    public static bool LoadFromObfuscatedJson(Entity entity, string obfuscatedJson, string key = null)
        => ObfuscationStorage.LoadFromObfuscatedJson(entity, obfuscatedJson, key);

    public static string ToBase64(byte[] bytes)
        => Convert.ToBase64String(bytes);
}
