
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    
    // 怪物的体积
    public float Size;
    
    class PlayerBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<EnemyMove>(entity);

            AddComponent(entity, new EnemyTag
            {
                Size = authoring.Size,
            });
        }
    }
}

// 怪物的标识
public struct EnemyTag : IComponentData
{
    // 怪物的体积
    public float Size;
}

public struct EnemyMove : IComponentData
{
    
}
