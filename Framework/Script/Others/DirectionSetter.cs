using UnityEngine;

public class DirectionSetter : MonoBehaviour, IDirectionSetter
{
    [SerializeField] GameObject targetObject;
    [SerializeField] Direction resourceDirection = Direction.Right;
    [SerializeField] RefreshTimingType refreshTimingType = RefreshTimingType.Enable;

    Transform TargetTransform => targetObject != null ? targetObject.transform : transform;

    IDirectionProvider directionProvider;
    Vector3 cachedDirection;
    bool hasCachedDirection;

    void Awake()
    {
        directionProvider = GetComponentInParent<IDirectionProvider>();
    }

    void OnEnable()
    {
        refreshTimingType.RunOnEnable(RefreshDirection);
    }

    void Update()
    {
        refreshTimingType.RunOnUpdate(RefreshDirection);
    }

    void LateUpdate()
    {
        refreshTimingType.RunOnLateUpdate(RefreshDirection);
    }

    public void SetDirection(Vector3 direction)
    {
        var direction2D = new Vector2(direction.x, direction.y);
        if (direction2D.sqrMagnitude <= 0f)
        {
            return;
        }

        var targetAngle = direction2D.ToAngle();
        var resourceAngle = resourceDirection.ToAngle();
        var rotateAngle = targetAngle - resourceAngle;

        TargetTransform.rotation = Quaternion.Euler(0f, 0f, rotateAngle);
    }

    void RefreshDirection()
    {
        if (directionProvider == null)
        {
            return;
        }

        var direction = directionProvider.Direction;
        if (hasCachedDirection && cachedDirection == direction)
        {
            return;
        }

        cachedDirection = direction;
        hasCachedDirection = true;
        SetDirection(direction);
    }
}
