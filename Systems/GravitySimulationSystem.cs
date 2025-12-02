using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

[BurstCompile]
public partial struct GravitySimulationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        float3 gravity = new(0, -9.81f, 0);

        foreach (var (transform, velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PhysicsVelocity>>().WithNone<HeldByPlayer>())
        {
            velocity.ValueRW.Linear += gravity * dt;
            transform.ValueRW.Position += velocity.ValueRW.Linear * dt;

            if (transform.ValueRW.Position.y < 0.5f)
            {
                transform.ValueRW.Position.y = 0.5f;
                velocity.ValueRW.Linear.y = -velocity.ValueRW.Linear.y * 0.6f;
                velocity.ValueRW.Linear *= 0.9f;
            }
        }
    }
}