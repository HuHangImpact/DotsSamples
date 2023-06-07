using Unity.Entities;
using Unity.Mathematics;

public struct PlayerConfig : IComponentData
{
    // 玩家移动速度
    public float MoveSpeed;
    
    // 玩家子弹发射频率
    public float FireRate;
    
    // 玩家子弹预制体
    public Entity BulletPrefab;
    
    // 玩家的预制体
    public Entity PlayerPrefab;
    
    // 玩家的体积
    public float PlayerSize;
    
    // 玩家子弹的碰撞体积
    public float BulletSize;
    
    // 玩家的可移动范围
    public float2 MoveRangeX;
    
    // 玩家的可移动范围
    public float2 MoveRangeZ;
    
}