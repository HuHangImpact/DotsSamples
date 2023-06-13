using Unity.Entities;

public struct EnemyConfig : IComponentData
{
    // 怪物移动速度
    public float MoveSpeed;
    
    // 怪物的预制体
    public Entity EnemyPrefab;
    
}