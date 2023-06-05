using Unity.Entities;
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
                BulletSize = authoring.BulletSize
            });
        }
    }
}

public struct PlayerTag : IComponentData
{
}


