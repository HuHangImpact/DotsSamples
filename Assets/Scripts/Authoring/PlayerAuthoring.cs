using Unity.Entities;
using UnityEngine;


    class PlayerAuthoring : MonoBehaviour
    {
        
        class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<Shooting>(entity);

                AddComponent<PlayerMove>(entity);
                
                AddComponent<PlayerInput>(entity);
                
                AddComponent<PlayerTag>(entity);
            }
        }
    }
    

    
    public struct Shooting : IComponentData, IEnableableComponent
    {
    }

    public struct PlayerMove : IComponentData
    {
    }
