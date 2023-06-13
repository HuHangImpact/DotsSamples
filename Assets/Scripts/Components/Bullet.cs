using Unity.Entities;
using Unity.Mathematics;

public struct Bullet : IComponentData
{
    public float3 Velocity;

    public float moveSpeed;
}