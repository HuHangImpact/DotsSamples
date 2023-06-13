using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct BulletTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        NativeReference<int> numTriggerEvents = new NativeReference<int>(0, Allocator.TempJob);
        state.Dependency = new CountNumTriggerEvents
        {
            playerTagComponents = SystemAPI.GetComponentLookup<PlayerTag>(),
            ECB = ecb,
            NumTriggerEvents = numTriggerEvents,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        state.Dependency.Complete();
        
        int Source = numTriggerEvents.Value;
        foreach (var (player, entity) in SystemAPI.Query<PlayerSource>().WithAll<PlayerTag>().WithEntityAccess())
        {
            var playerEntity = SystemAPI.GetComponent<PlayerSource>(entity);
            playerEntity.Source += Source;
            SystemAPI.SetComponent(entity, playerEntity);
        }
        
        numTriggerEvents.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public partial struct CountNumTriggerEvents : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> playerTagComponents;
        public EntityCommandBuffer ECB;
        public NativeReference<int> NumTriggerEvents;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (playerTagComponents.HasComponent(entityA) || playerTagComponents.HasComponent(entityB))
            {
                return;
            }

            ECB.DestroyEntity(entityA);
            ECB.DestroyEntity(entityB);
            NumTriggerEvents.Value++;
        }
    }
}