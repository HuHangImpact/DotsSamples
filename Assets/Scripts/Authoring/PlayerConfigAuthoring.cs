using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerConfigAuthoring : MonoBehaviour
{
    // 玩家移动速度
    public float MoveSpeed = 10f;
    
    // 玩家子弹发射频率
    public float FireRate = 0.25f;
    
    // 玩家子弹预制体
    public GameObject BulletPrefab;
    
    // 玩家的预制体
    public GameObject PlayerPrefab;
    
    // 玩家的体积
    public float PlayerSize = 1f;
    
    // 玩家子弹的碰撞体积
    public float BulletSize = 0.5f;
    
    // 玩家的可移动范围
    public float2 MoveRangeX;
    
    // 玩家的可移动范围
    public float2 MoveRangeZ;

    class PlayerConfigBaker : Baker<PlayerConfigAuthoring>
    {
        public override void Bake(PlayerConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerConfig
            {
                FireRate = authoring.FireRate,
                MoveSpeed = authoring.MoveSpeed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                PlayerSize = authoring.PlayerSize,
                BulletSize = authoring.BulletSize,
                MoveRangeX = authoring.MoveRangeX,
                MoveRangeZ = authoring.MoveRangeZ
            });
        }
    }
}

public struct PlayerTag : IComponentData
{
}


