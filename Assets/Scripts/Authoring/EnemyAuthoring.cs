
using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
      
    class PlayerBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<EnemyMove>(entity);

            AddComponent<EnemyTag>(entity);
        }
    }
}

// 怪物的标识
public struct EnemyTag : IComponentData
{
}

public struct EnemyMove : IComponentData
{
    
}
