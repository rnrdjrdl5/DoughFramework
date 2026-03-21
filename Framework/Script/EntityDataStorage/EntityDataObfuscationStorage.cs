using System;
using System.IO;
using System.Text;
using UnityEngine;

public class EntityDataObfuscationStorage : EntityDataStorageBase
{
    const string DefaultKey = "EntityDataObfuscationKey";
    static string customKey;

    public static void SetKey(string key)
    {
        customKey = key;
    }

    static string GetKeyOrDefault(string key = null)
    {
        if (!string.IsNullOrEmpty(key))
            return key;

        return string.IsNullOrEmpty(customKey) ? DefaultKey : customKey;
    }

    public string ToObfuscatedJson(Entity entity, string key = null)
    {
        var json = ToJson(entity);
        if (string.IsNullOrEmpty(json))
            return null;

        if (!TryGetKeyBytes(GetKeyOrDefault(key), out var keyBytes))
            return null;

        return ObfuscateToBase64(json, keyBytes);
    }

    public void Save(Entity entity, string saveKey, string key = null, string relativeDirectory = null)
    {
        if (string.IsNullOrEmpty(saveKey))
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: saveKey is null or empty.");
            return;
        }

        var obfuscated = ToObfuscatedJson(entity, key);
        if (string.IsNullOrEmpty(obfuscated))
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: obfuscated json is null or empty.");
            return;
        }

        var path = BuildPath(saveKey, relativeDirectory);
        try
        {
            File.WriteAllText(path, obfuscated, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: {ex.Message}");
        }
    }

    public bool Load(Entity entity, string saveKey, string key = null, string relativeDirectory = null)
    {
        if (entity == null)
        {
            Debug.LogWarning($"{LogPrefix}.Load failed: entity is null.");
            return false;
        }

        if (string.IsNullOrEmpty(saveKey))
        {
            Debug.LogWarning($"{LogPrefix}.Load failed: saveKey is null or empty.");
            return false;
        }

        var path = BuildPath(saveKey, relativeDirectory);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"{LogPrefix}.Load failed: file not found at {path}");
            return false;
        }

        try
        {
            var obfuscated = File.ReadAllText(path, Encoding.UTF8);
            return LoadFromObfuscatedJson(entity, obfuscated, key);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.Load failed: {ex.Message}");
            return false;
        }
    }

    public bool LoadFromObfuscatedJson(Entity entity, string obfuscatedJson, string key = null)
    {
        if (string.IsNullOrEmpty(obfuscatedJson))
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromObfuscatedJson failed: obfuscatedJson is null or empty.");
            return false;
        }

        if (!TryGetKeyBytes(GetKeyOrDefault(key), out var keyBytes))
            return false;

        try
        {
            var json = DeobfuscateFromBase64(obfuscatedJson, keyBytes);
            return LoadFromJson(entity, json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromObfuscatedJson failed: {ex.Message}");
            return false;
        }
    }

    static bool TryGetKeyBytes(string key, out byte[] keyBytes)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("EntityDataObfuscationStorage key is null or empty.");
            keyBytes = null;
            return false;
        }

        keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length == 0)
        {
            Debug.LogWarning("EntityDataObfuscationStorage key length is zero.");
            return false;
        }

        return true;
    }

    static string ObfuscateToBase64(string plainText, byte[] key)
    {
        var input = Encoding.UTF8.GetBytes(plainText);
        var output = new byte[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (byte)(input[i] ^ key[i % key.Length]);
        }

        return Convert.ToBase64String(output);
    }

    static string DeobfuscateFromBase64(string obfuscatedBase64, byte[] key)
    {
        var input = Convert.FromBase64String(obfuscatedBase64);
        var output = new byte[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (byte)(input[i] ^ key[i % key.Length]);
        }

        return Encoding.UTF8.GetString(output);
    }
}
