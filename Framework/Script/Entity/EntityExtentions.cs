using System.Collections.Generic;
using System.Linq;

public static class EntityExtensions
{
    public static DataSet ToDataSet(this Entity entity)
    {
        var datas = entity.ToData();
        
        var dataSet = new DataSet();
        dataSet.SetTargetDatas(datas);
        
        return dataSet;
    }

    public static IEnumerable<IData> ToData(this Entity entity)
    {
        return entity.AbilitySet.Abilities
            .Select(ability => ability as IData)
            .Concat(entity.EntityDatas.Select(entityData => entityData as IData))
            .Where(d => d != null);
    }
}