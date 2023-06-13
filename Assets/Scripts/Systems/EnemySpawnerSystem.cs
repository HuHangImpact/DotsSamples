using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[UpdateBefore(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(EnemyMoveSystem))]
[BurstCompile]
public partial struct EnemySpawner : ISystem
{
    private EntityQuery m_EnemyQuery;
    private float m_TimeSinceLastSpawn;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<EnemyConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        m_EnemyQuery = state.EntityManager.CreateEntityQuery(typeof(EnemyTag));
        var enemyCount = m_EnemyQuery.CalculateEntityCount();
        var config = SystemAPI.GetSingleton<EnemyConfig>();
        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        if (enemyCount < 10 && m_TimeSinceLastSpawn >= 1f)
        {
            new EnemySpawnerJob
            {
                ECB = ecb.AsParallelWriter(),
                EnemyConfig = config,
                TimeSinceLastSpawn = m_TimeSinceLastSpawn,
                enemyCount = enemyCount
            }.ScheduleParallel();
            
            m_TimeSinceLastSpawn = 0f;
        }

        m_TimeSinceLastSpawn += SystemAPI.Time.DeltaTime;
    }
    [WithAll(typeof(Shooting))]
    public partial struct EnemySpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public EnemyConfig EnemyConfig;
        public float TimeSinceLastSpawn;
        public int enemyCount;
        
        public void Execute([ChunkIndexInQuery] int index, ref LocalTransform FireLocalTransform,
            ref LocalToWorld localToWorld)
        {
            var instance = ECB.Instantiate(index, EnemyConfig.EnemyPrefab);
            var random = new Random(((uint)enemyCount + 1) * (uint)TimeSinceLastSpawn * 1000);
            var spawnPosition = new float3(random.NextFloat(-6, 6), 0f, 16);

            LocalTransform localTransform = LocalTransform.FromPositionRotationScale(
                spawnPosition,
                FireLocalTransform.Rotation,
                FireLocalTransform.Scale);

            ECB.SetComponent(index, instance, localTransform);
            
            ECB.AddComponent<EnemyTag>(index, instance);
        }
    }
}

