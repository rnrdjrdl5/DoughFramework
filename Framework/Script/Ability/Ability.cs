using UnityEngine;

public abstract class Ability : MonoBehaviour, IData
{
    public Entity Entity => entity;
    
    Entity entity;

    public virtual void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Ready()
    {
        
    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
    }
}
