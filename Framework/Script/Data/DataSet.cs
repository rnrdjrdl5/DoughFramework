using System.Collections.Generic;
using System.Linq;

public class DataSet
{
    public IEnumerable<IData> TargetDatas => targetDatas;
    
    IEnumerable<IData> targetDatas;
    
    public void SetTargetDatas(IEnumerable<IData> datas)
    {
        targetDatas = datas;
    }
    
    public bool TryUnsetTargetDatas()
    {
        if (targetDatas == null)
        {
            return false;
        }
        
        targetDatas = null;
        return true;
    }

    public TData GetTargetDatas<TData>() where TData : class
    {
        if (targetDatas == null)
        {
            return null;
        }
        
        return targetDatas.Where(element => typeof(TData).IsAssignableFrom(element.GetType()))
            .Cast<TData>()
            .FirstOrDefault();
    }
}