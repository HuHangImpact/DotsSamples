using System;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

[BurstCompile]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<EnemyTag>();
    }

    [BurstCompile]
    public void OnDestroy()
    {
    }

    [BurstCompile]
    [Obsolete("Obsolete")]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystemSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystemSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var entityManager = state.EntityManager;
        
        var bulletMoveJob = new BulletMove
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecb,
        };
        

        foreach (var bullet in SystemAPI.Query<BulletAspect>().WithAll<Bullet>())
        {
            if (bullet.Position.z > 20)
            {
                ecb.DestroyEntity(bullet.Self);
            }
        }

        foreach (var bullet in SystemAPI.Query<BulletAspect>().WithAll<Bullet>())
        {
            foreach (var enemy in SystemAPI.Query<EnemyAspect>().WithAll<EnemyTag>())
            {
                var bulletPosition = bullet.Position;
                var enemyPosition = enemy.Position;
                var enemySize = enemy.Size;
              
                // 判断子弹是否在敌人范围内
                var distance = math.distance(bulletPosition, enemyPosition);
                if (distance < enemySize)
                {
                    ecb.DestroyEntity(bullet.Self);
                    ecb.DestroyEntity(enemy.Self);
                    Debug.Log("Bullet and Enemy Destroyed");
                }
            }
        }
        
        bulletMoveJob.Schedule();
    }
    
}

[BurstCompile]
public partial struct BulletMove : IJobEntity
{
    public float DeltaTime;
    
    public EntityCommandBuffer ECB;
    public void Execute(BulletAspect bullet)
    {
        var position = bullet.Position;
        position += bullet.Velocity * bullet.MoveSpeed * DeltaTime;
        bullet.Position = position;
    }
}
