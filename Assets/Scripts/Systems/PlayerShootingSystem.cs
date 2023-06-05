
using Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(PlayerMoveSystem))]
[BurstCompile]
public partial struct PlayerShootingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Shooting>();
        
        state.RequireForUpdate<PlayerConfig>();
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
        
        var turretShootJob = new PlayerTurretShoot
        {
            LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
            ECB = ecb,
            PlayerConfig = SystemAPI.GetSingleton<PlayerConfig>(),
        };
        
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        turretShootJob.Schedule();
    }
}

[WithAll(typeof(Shooting))] 
[BurstCompile]
public partial struct PlayerTurretShoot : IJobEntity
{
    [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
    public EntityCommandBuffer ECB;
    
    public PlayerConfig PlayerConfig;
    
    public void Execute(LocalToWorld localToWorld)
    {
        for (var i = 0; i < 10; i++)
        {
            var instance = ECB.Instantiate(PlayerConfig.BulletPrefab);
        
            var spawnLocalToWorld = localToWorld;
        
            ECB.SetComponent(instance, new LocalTransform
            {
                Position = spawnLocalToWorld.Position + localToWorld.Forward,
                // 根据i值一个扇形角度
                Rotation = quaternion.Euler(0, 0, i * 36.0f),
                Scale = 1.0f,
            });
        
            ECB.SetComponent(instance, new Bullet
            { 
                // 根据i值一个扇形角度
                Velocity = localToWorld.Forward * 20.0f,
                moveSpeed = 3.0f,
            });
        }
        
    }
}

