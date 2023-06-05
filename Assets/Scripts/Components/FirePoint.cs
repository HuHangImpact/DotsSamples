using Unity.Entities;

public struct FirePoint : IComponentData
{
    // 子弹发射点
    public Entity BulletPoint;
    
    // 子弹预制体
    public Entity BulletPrefab;
}