using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class BulletAuthoring : MonoBehaviour
{
    
    public float moveSpeed;
    
    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                moveSpeed = 10f,
            });
        }
    }
}


public struct Bullet : IComponentData
{
    public float3 Velocity; 
    
    public float moveSpeed;
}
