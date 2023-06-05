
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyMove>();
        state.RequireForUpdate<EnemyConfig>();
    }

    [BurstCompile]
    public void OnDestroy()
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<EnemyConfig>();
            
        var deltaTime = SystemAPI.Time.DeltaTime;
            
        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<EnemyTag>())
        {
                
            var move = new float3
            {
                x = 0,
                y = 0,
                z = -0.4f
            };

            transform.ValueRW.Position += move * deltaTime * config.MoveSpeed;
            
            transform.ValueRW.Rotation = quaternion.Euler(0, transform.ValueRW.Position.z ,0);
        }
            
    }
    
}
