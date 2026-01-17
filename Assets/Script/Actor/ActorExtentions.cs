using System.Collections.Generic;
using System.Linq;

public static class ActorExtensions
{
    public static DataSet ToDataSet(this Actor actor)
    {
        var datas = actor.ToData();
        
        var dataSet = new DataSet();
        dataSet.SetTargetDatas(datas);
        
        return dataSet;
    }

    public static IEnumerable<IData> ToData(this Actor actor)
    {
        return actor.Traits
            .Select(trait => trait as IData)
            .Concat(actor.ActorDatas.Select(actorData => actorData as IData))
            .Where(d => d != null);
    }
}