using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateAfter(typeof(BulletTriggerSystem))]
public partial struct PlayerTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<EnemyTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        state.Dependency = new PlayerTriggerEvents
        {
            EnemyComponents = SystemAPI.GetComponentLookup<EnemyTag>(),
            playerTagComponents = SystemAPI.GetComponentLookup<PlayerTag>(),
            ECB = ecb,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public partial struct PlayerTriggerEvents : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyComponents;
        [ReadOnly] public ComponentLookup<PlayerTag> playerTagComponents;
        public EntityCommandBuffer ECB;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (EnemyComponents.HasComponent(entityB) && playerTagComponents.HasComponent(entityA))
            {
                ECB.DestroyEntity(entityA);
                ECB.DestroyEntity(entityB);
            }
        }
    }
}