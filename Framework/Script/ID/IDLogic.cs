using System;
using System.Security.Cryptography;

public static class IDLogic
{
    public static long NewUniqueId()
    {
        Span<byte> buffer = stackalloc byte[8];
        RandomNumberGenerator.Fill(buffer);
        
        var id = BitConverter.ToInt64(buffer) & long.MaxValue;
        if (id == 0) id = 1;
        
        return id;
    }
    
    public static int NewUniqueIntId()
    {
        Span<byte> buffer = stackalloc byte[4];
        RandomNumberGenerator.Fill(buffer);
    
        var id = BitConverter.ToInt32(buffer) & int.MaxValue;
        if (id == 0) id = 1;
    
        return id;
    }
}
