
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct BulletAspect : IAspect
{ 
    public readonly Entity Self;
        
    readonly RefRW<LocalTransform> Transform;
    
    readonly RefRW<Bullet> Bullet;
    
    public float MoveSpeed => Bullet.ValueRW.moveSpeed;
    
    public float3 Position
    {
        get => Transform.ValueRW.Position;
        set => Transform.ValueRW.Position = value;
    }
    
    public float3 Velocity
    {
        get => Bullet.ValueRW.Velocity;
        set => Bullet.ValueRW.Velocity = value;
    }
}
