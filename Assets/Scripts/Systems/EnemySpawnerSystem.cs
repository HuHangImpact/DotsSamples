using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateBefore(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(EnemyMoveSystem))]
[BurstCompile]
public partial class EnemySpawner : SystemBase
{
    private EntityQuery m_EnemyQuery;
    private float m_TimeSinceLastSpawn = 10;

    [BurstCompile]
    protected override void OnCreate()
    {
        m_EnemyQuery = GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
    }

    [BurstCompile]
    [Obsolete("Obsolete")]
    protected override void OnUpdate()
    {
        var enemyCount = m_EnemyQuery.CalculateEntityCount();

        // Spawn a new enemy if there are less than 10 enemies in the game world and the time interval has passed.
        if (enemyCount < 10 && m_TimeSinceLastSpawn >= 2f)
        {
            var enemyPrefab = GetSingleton<EnemyConfig>().EnemyPrefab;
            var random = new Random(((uint)enemyCount +1) * 0x9F6ABC1);
            var spawnPosition = new float3(random.NextFloat(-6,6), 0f, 16);

            var enemyEntity = EntityManager.Instantiate(enemyPrefab);
            EntityManager.SetComponentData(enemyEntity, new LocalTransform
            {
                Position = spawnPosition,
                Scale = 1,
                Rotation = quaternion.identity
            });
            m_TimeSinceLastSpawn = 0f;
        }

        m_TimeSinceLastSpawn += SystemAPI.Time.DeltaTime;
    }
}