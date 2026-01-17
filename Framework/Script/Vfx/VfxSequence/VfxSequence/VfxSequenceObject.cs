using UnityEngine;

// Sequence의 정보를 저장하는 곳
public class VfxSequenceObject : MonoBehaviour
{
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }
    public Vector3 ProgressPosition { get; private set; }
    public Transform TargetTransform => targetTransform != null ? targetTransform : transform;

    [SerializeField] Transform targetTransform;

    public void UpdateProgressPosition()
    {
        ProgressPosition = TargetTransform.position;
    }

}
