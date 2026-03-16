using UnityEngine;

public abstract class Ability : MonoBehaviour, IData
{
    public Entity Entity => entity;
    
    Entity entity;
    bool isReady;

    public virtual void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Ready()
    {
        isReady = true;
    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
    }
}
