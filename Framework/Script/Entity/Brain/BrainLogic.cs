using UnityEngine;

public static class BrainLogic
{
    public static Brain CreateBrainAndEntity(Entity ownerEntity, string brainPath, string entityPath)
    {
        var brain = ownerEntity.AddEntity<Brain>(brainPath);
        var entity = ownerEntity.AddEntity<Entity>(entityPath);
        brain.AttachControll(entity);

        return brain;
    }
}
