using Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
[UpdateBefore(typeof(PlayerMoveSystem))]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[BurstCompile]
public partial struct PlayerShootingSystem : ISystem
{
    public float _nextFireTime;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<PlayerConfig>();
    }

    [BurstCompile]
    public void OnDestroy()
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playConfig = SystemAPI.GetSingleton<PlayerConfig>();
        // 添加一个计时器，以便在攻击间隔时间内不发射子弹
        _nextFireTime -= SystemAPI.Time.DeltaTime;

        if (!Input.GetButton("Fire1") || _nextFireTime > 0f)
        {
            return;
        }

        _nextFireTime = playConfig.FireRate;

        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var turretShootJob = new PlayerTurretShoot
        {
            ECB = ecb,
            PlayerConfig = SystemAPI.GetSingleton<PlayerConfig>(),
        };

        turretShootJob.ScheduleParallel();
    }
}

[WithAll(typeof(Shooting))]
public partial struct PlayerTurretShoot : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public PlayerConfig PlayerConfig;

    public void Execute([ChunkIndexInQuery] int index, ref LocalTransform FireLocalTransform,
        ref LocalToWorld localToWorld)
    {
        var instance = ECB.Instantiate(index, PlayerConfig.BulletPrefab);

        float3 firePosition = new float3((localToWorld.Position + localToWorld.Forward).x, 0,
            (localToWorld.Position + localToWorld.Forward).z);

        LocalTransform localTransform = LocalTransform.FromPositionRotationScale(
            firePosition,
            FireLocalTransform.Rotation,
            FireLocalTransform.Scale);

        ECB.SetComponent(index, instance, localTransform);

        ECB.SetComponent(index, instance, new PhysicsVelocity
        {
            Linear = localToWorld.Forward * 20f,
            Angular = float3.zero,
        });

        ECB.AddComponent<Bullet>(index, instance);
    }
}