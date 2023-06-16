using Unity.Entities;
using UnityEngine;

public struct PSTag : IComponentData
{
}

[DisallowMultipleComponent]
public class PSTagAuthoring : MonoBehaviour
{
    class JumpingSpherePSTagBaker : Baker<PSTagAuthoring>
    {
        public override void Bake(PSTagAuthoring authoring)
        {
            PSTag component = default(PSTag);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, component);
        }
    }
}
