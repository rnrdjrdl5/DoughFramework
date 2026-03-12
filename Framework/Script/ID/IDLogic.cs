using System;
using System.Security.Cryptography;

public static class IDLogic
{
    // NOTE : 0은 초기화가 안되었음을 의미한다.
    public static long NewUniqueId()
    {
        Span<byte> buffer = stackalloc byte[8];
        RandomNumberGenerator.Fill(buffer);
        
        var id = BitConverter.ToInt64(buffer) & long.MaxValue;
        if (id == 0) id = 1;
        
        return id;
    }
}
