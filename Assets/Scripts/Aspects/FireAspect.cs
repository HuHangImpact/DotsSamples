using Unity.Entities;

public readonly partial struct FireAspect : IAspect
{
    readonly RefRO<FirePoint> m_Fire;
    
    public Entity BulletFirePoint => m_Fire.ValueRO.BulletPoint;
    public Entity BulletPrefab => m_Fire.ValueRO.BulletPrefab;
}


