using Unity.Entities;using UnityEngine;


public class ParticleSystemConfigAuthoring : MonoBehaviour
{
    public GameObject MonsterExplosionPrefab;
}

public class ParticleSystemConfigBaker : Baker<ParticleSystemConfigAuthoring>
{
    public override void Bake(ParticleSystemConfigAuthoring authoring)
    {
        ParticleSystemManager component = default(ParticleSystemManager);
        component.MonsterExplosionPrefab = GetEntity(authoring.MonsterExplosionPrefab, TransformUsageFlags.None);
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, component);
    }
}

