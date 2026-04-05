using UnityEngine;

[Processor(typeof(PhysicalTokenRouterProcessor))]
public abstract class FrameworkInputProcessorAbility : LayerProcessorAbility<PhysicalInputTokenEvent>
{
}
