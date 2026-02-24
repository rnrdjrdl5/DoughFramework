using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 미리 스테이지를 만들어서 캐싱해둘 수 있다.
// State 형태로 스테이지를 관리한다. 
// 스테이지 전환 시 전환 Function 호출

public class StageTrait : Trait
{
    public Stage TopStage => stages.Count == 0 ? null : stages[^1]; 
    
    List<Stage> stages = new();
    ObjectPoolTrait objectPoolTrait;

    public override void Ready()
    {
        base.Ready();
        
        objectPoolTrait = Actor.RootTraitSet.GetTrait<ObjectPoolTrait>();
    }

    public StageType GetStage<StageType>() where StageType : Stage
    {
        return stages.Where(trait => typeof(StageType).IsAssignableFrom(trait.GetType()))
            .Cast<StageType>()
            .FirstOrDefault();
    }

    public StageType AddStage<StageType>(string prefabPath) where StageType : Stage, new ()
    {
        var stagePrefab = Realm.LoadResources<GameObject>(prefabPath);
        var stageObject = objectPoolTrait.AllocateGameObject(stagePrefab);
        
        var stage = stageObject.GetComponent<StageType>();
        stage.Initialize(Actor.RootTraitSet);
        
        stages.Add(stage);
        
        return stage;
    }

    public void RemoveStage(Stage stage)
    {
        stage.Uninitialize();
        stages.Remove(stage);
        
        objectPoolTrait.DeallocateGameObject(stage.gameObject);
    }
}