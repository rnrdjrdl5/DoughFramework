using UnityEngine;

public static class BrainLogic
{
    public static Brain CreateBrainAndEntity(Entity ownerEntity, string brainPath, string entityPath, IInitData brainData = null, IInitData entityData = null)
    {
        var brain = ownerEntity.AddEntity<Brain>(brainPath, brainData);
        var entity = ownerEntity.AddEntity<Entity>(entityPath, entityData);
        brain.AttachControll(entity);

        return brain;
    }
}
