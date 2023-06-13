using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;

public partial class SourceSystem : SystemBase
{
    public Action<int,float3> OnSourceSpawn;

    protected override void OnUpdate()
    {
        
    }
}