using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    [BurstCompile]
    public void OnDestroy()
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystemSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystemSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        var bulletMoveJob = new BulletMove
        {
            ECB = ecb.AsParallelWriter(),
            DeltaTime = SystemAPI.Time.DeltaTime,
        };
        
        bulletMoveJob.Schedule();
    }
    
}

[BurstCompile]
public partial struct BulletMove : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public float DeltaTime;
    
    public void Execute(BulletAspect bullet)
    {
        var position = bullet.Position;
        position += bullet.Velocity * bullet.MoveSpeed * DeltaTime;
        bullet.Position = position;
    }
}
