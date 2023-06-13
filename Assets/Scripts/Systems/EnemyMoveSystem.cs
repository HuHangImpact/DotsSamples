
using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
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
        var ecbSystemSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystemSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        var deltaTime = SystemAPI.Time.DeltaTime;
            
        foreach (var transform in SystemAPI.Query<EnemyAspect>().WithAll<EnemyTag>())
        {
            var move = new float3
            {
                x = 0,
                y = 0,
                z = -0.4f
            };

            transform.Position += move * deltaTime * config.MoveSpeed;
            
            transform.Rotation = quaternion.Euler(0, transform.Position.z ,0);
        }
        
        // 检测敌人移动到屏幕外销毁
        foreach (var enemy in SystemAPI.Query<EnemyAspect>().WithAll<EnemyTag>())
        {
            if (enemy.Position.z < -20)
            {
                ecb.DestroyEntity(enemy.Self);
                Debug.Log("Enemy Destroyed");
            }
        }
        
    }
}
