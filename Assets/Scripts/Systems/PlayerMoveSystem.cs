using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct PlayerMoveSystem: ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerMove>();
            state.RequireForUpdate<PlayerConfig>();
        }

        [BurstCompile]
        public void OnDestroy()
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<PlayerConfig>();
            
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (transform, playerInput) in SystemAPI.Query<RefRW<LocalTransform>, PlayerInput>().WithAll<PlayerTag>())
            {
                
                var move = new float3
                {
                    x = playerInput.Horizontal,
                    y = 0,
                    z = playerInput.Vertical
                };  
                
                var input = new float3(playerInput.Horizontal, 0, playerInput.Vertical) * SystemAPI.Time.DeltaTime * config.MoveSpeed;

                if (input.Equals(float3.zero))
                {
                    continue;
                }
                
                transform.ValueRW.Position += move * deltaTime * config.MoveSpeed;
            }
            
        }
    }
}