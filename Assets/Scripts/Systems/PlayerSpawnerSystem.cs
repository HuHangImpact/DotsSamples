using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<PlayerConfig>();

        var playerEntity = state.EntityManager.Instantiate(config.PlayerPrefab);

        state.EntityManager.AddComponentData(playerEntity, new PlayerTag());

        // 设置玩家的初始位置
        state.EntityManager.SetComponentData(playerEntity, new LocalTransform
        {
            Position = new float3
            {
                x = 0,
                y = -4,
                z = 0,
            },
            Scale = 1,
            Rotation = quaternion.identity
        });

        state.Enabled = false;
    }
}
