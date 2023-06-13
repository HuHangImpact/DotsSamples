using Unity.Entities;
using UnityEngine;

public class EnemyConfigAuthoring : MonoBehaviour
{
    
    // 敌人移动速度
    public float MoveSpeed = 10f;
    
    // 敌人的预制体
    public GameObject EnemyPrefab;

    class EnemyConfigBaker : Baker<EnemyConfigAuthoring>
    {
        public override void Bake(EnemyConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyConfig
            {
                MoveSpeed = authoring.MoveSpeed,
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
        
}
