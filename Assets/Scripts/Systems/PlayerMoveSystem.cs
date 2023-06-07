using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
            
            foreach (var (transform, playerInput) in SystemAPI.Query<RefRW<LocalTransform>, PlayerInput>().WithAll<PlayerTag>())
            {
                var input = new float3(playerInput.Horizontal, 0, playerInput.Vertical) * SystemAPI.Time.DeltaTime * config.MoveSpeed;

                if (input.Equals(float3.zero))
                {
                    continue;
                }
                
                // 根据player的移动范围，限制player的移动
                var position = transform.ValueRW.Position;
                var newPosition = position + input;
                newPosition.x = math.clamp(newPosition.x, config.MoveRangeX.x, config.MoveRangeX.y);
                newPosition.z = math.clamp(newPosition.z, config.MoveRangeZ.x, config.MoveRangeZ.y);
                transform.ValueRW.Position = newPosition;
            }
        }
    } 
}