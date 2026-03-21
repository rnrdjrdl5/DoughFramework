using System.Security.Cryptography;

public static class EntityDataStorageKeyGenerator
{
    public static byte[] CreateKey()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    public static byte[] CreateIv()
    {
        var iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }
}
