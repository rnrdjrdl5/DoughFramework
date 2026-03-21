using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class EntityDataStorage : EntityDataStorageBase
{
    public string ToEncryptedJson(Entity entity, string keyBase64, string ivBase64)
    {
        var json = ToJson(entity);
        if (string.IsNullOrEmpty(json))
            return null;

        if (!TryDecodeKeyIv(keyBase64, ivBase64, out var key, out var iv))
            return null;

        return EncryptToBase64(json, key, iv);
    }

    public void Save(Entity entity, string saveKey, string keyBase64, string ivBase64, string relativeDirectory = null)
    {
        if (string.IsNullOrEmpty(saveKey))
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: saveKey is null or empty.");
            return;
        }

        var encrypted = ToEncryptedJson(entity, keyBase64, ivBase64);
        if (string.IsNullOrEmpty(encrypted))
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: encrypted json is null or empty.");
            return;
        }

        var path = BuildPath(saveKey, relativeDirectory);
        try
        {
            File.WriteAllText(path, encrypted, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.Save failed: {ex.Message}");
        }
    }

    public bool Load(Entity entity, string saveKey, string keyBase64, string ivBase64, string relativeDirectory = null)
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
            var encrypted = File.ReadAllText(path, Encoding.UTF8);
            return LoadFromEncryptedJson(entity, encrypted, keyBase64, ivBase64);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.Load failed: {ex.Message}");
            return false;
        }
    }

    public bool LoadFromEncryptedJson(Entity entity, string encryptedJson, string keyBase64, string ivBase64)
    {
        if (string.IsNullOrEmpty(encryptedJson))
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromEncryptedJson failed: encryptedJson is null or empty.");
            return false;
        }

        if (!TryDecodeKeyIv(keyBase64, ivBase64, out var key, out var iv))
            return false;

        try
        {
            var json = DecryptFromBase64(encryptedJson, key, iv);
            return LoadFromJson(entity, json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix}.LoadFromEncryptedJson failed: {ex.Message}");
            return false;
        }
    }

    bool TryDecodeKeyIv(string keyBase64, string ivBase64, out byte[] key, out byte[] iv)
    {
        key = null;
        iv = null;

        try
        {
            key = Convert.FromBase64String(keyBase64);
            iv = Convert.FromBase64String(ivBase64);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{LogPrefix} key/iv decode failed: {ex.Message}");
            return false;
        }

        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
        {
            Debug.LogWarning($"{LogPrefix} key length invalid: {key.Length} bytes.");
            return false;
        }

        if (iv.Length != 16)
        {
            Debug.LogWarning($"{LogPrefix} iv length invalid: {iv.Length} bytes.");
            return false;
        }

        return true;
    }

    static string EncryptToBase64(string plainText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs, Encoding.UTF8))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    static string DecryptFromBase64(string cipherBase64, byte[] key, byte[] iv)
    {
        var buffer = Convert.FromBase64String(cipherBase64);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);
        return sr.ReadToEnd();
    }

}
