using UnityEngine;

public class DelayedCount
{
    public int ProgressCount => originCount - reservedCount;
    
    int reservedCount;
    int originCount;

    public void SetReservedCount(int reservedCount)
    {
        this.reservedCount = reservedCount;
    }

    public void SetOriginCount(int originCount)
    {
        this.originCount = originCount;
    }
}
