using UnityEngine;

public class VfxSequenceMoveCurve : VfxSequence
{
    [SerializeField] float orthogonalPercent;
    [SerializeField] float parallelPercent;
    [SerializeField] bool isRandom;
    
    protected override void ProcessSequence(float elapsedTime)
    {
        base.ProcessSequence(elapsedTime);
        
        var normalized = elapsedTime / duration;
        var currentPosition = MathUtils.Bezier(
            sequenceObject.ProgressPosition,
            GetBezierPosition(), 
            sequenceObject.EndPosition,
            normalized);

        sequenceObject.TargetTransform.position = currentPosition;
    }

    Vector3 GetBezierPosition()
    {
        var direction = sequenceObject.EndPosition - sequenceObject.StartPosition;

        var normalizedOrthogonal =
            new Vector3(-direction.y, direction.x, sequenceObject.TargetTransform.position.z).normalized;
        var normalizedParallel = direction.normalized;

        return sequenceObject.StartPosition + normalizedParallel * (parallelPercent * direction.magnitude) +
               normalizedOrthogonal * (orthogonalPercent * direction.magnitude);
    }
}