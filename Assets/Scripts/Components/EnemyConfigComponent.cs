using Unity.Entities;

public struct EnemyConfig : IComponentData
{
    // 怪物移动速度
    public float MoveSpeed;
    
    // 怪物子弹发射频率
    public float FireRate;
    
    // 怪物子弹预制体
    public Entity BulletPrefab;
    
    // 怪物的预制体
    public Entity EnemyPrefab;
    
    // 怪物的体积
    public float EnemySize;
    
    // 怪物子弹的碰撞体积
    public float BulletSize;
    
    // 生成怪物的时间间隔
    public float SpawnInterval;
}