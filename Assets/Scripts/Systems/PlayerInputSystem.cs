using Unity.Entities;
public partial class PlayerInputSystem: SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        Entities.ForEach((ref PlayerInput input) =>
        {
            input.Horizontal = UnityEngine.Input.GetAxis("Horizontal");
            input.Vertical = UnityEngine.Input.GetAxis("Vertical");
            input.Fire = UnityEngine.Input.GetButton("Fire1"); 
            
        }).Run();
    }
}

struct PlayerInput : IComponentData
{
   public float Horizontal;
    
   public float Vertical;
    
   public bool Fire;
}
