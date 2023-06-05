using Unity.Entities;
using UnityEngine;

public class EnemyConfigAuthoring : MonoBehaviour
{
    
    // 敌人移动速度
    public float MoveSpeed = 10f;
    
    // 敌人子弹发射频率
    public float FireRate = 0.25f;
    
    // 敌人子弹预制体
    public GameObject BulletPrefab;
    
    // 敌人的预制体
    public GameObject EnemyPrefab;
    
    // 敌人的体积
    public float EnemySize = 1f;
    
    // 敌人子弹的碰撞体积
    public float BulletSize = 0.5f;

    class EnemyConfigBaker : Baker<EnemyConfigAuthoring>
    {
        public override void Bake(EnemyConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyConfig
            {
                FireRate = authoring.FireRate,
                MoveSpeed = authoring.MoveSpeed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                EnemySize = authoring.EnemySize,
                BulletSize = authoring.BulletSize
            });
        }
    }
        
}
