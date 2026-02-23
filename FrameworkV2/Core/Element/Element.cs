// Director에 의해 조립되는 기능 단위
public abstract class Element
{
    public Director Director => director;
    public Entity Host => director != null ? director.Host : null;

    Director director;

    // Director를 연결합니다.
    public void Attach(Director director)
    {
        this.director = director;
        OnAttached();
    }

    // Ability를 조회합니다.
    protected T GetAbility<T>() where T : Ability
    {
        return director != null ? director.GetAbility<T>() : null;
    }

    // Ability 보유 여부를 조회합니다.
    protected bool HasAbility<T>() where T : Ability
    {
        return director != null && director.HasAbility<T>();
    }

    // Element 초기화를 수행합니다.
    public virtual void Initialize() { }

    // Element 준비 단계를 수행합니다.
    public virtual void Ready() { }

    // Element 종료 단계를 수행합니다.
    public virtual void Uninitialize() { }

    // Director 연결 직후 훅을 제공합니다.
    protected virtual void OnAttached() { }
}
