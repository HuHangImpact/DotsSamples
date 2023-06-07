using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct EnemyAspect : IAspect
{
    public readonly Entity Self;
        
    readonly RefRW<LocalTransform> Transform;
    
    readonly RefRW<EnemyTag> enemy;
    
    public float3 Position
    {
        get => Transform.ValueRW.Position;
        set => Transform.ValueRW.Position = value;
    }
    
    public quaternion Rotation
    {
        get => Transform.ValueRW.Rotation;
        set => Transform.ValueRW.Rotation = value;
    }
    
    // 怪物的体积
    public float Size
    {
        get => enemy.ValueRW.Size;
        set => enemy.ValueRW.Size = value;
    }
}