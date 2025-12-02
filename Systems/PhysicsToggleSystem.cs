// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Mathematics;

// [BurstCompile]
// public partial struct PhysicsToggleSystem : ISystem
// {
//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         // Кинематические — когда держим
//         foreach (var velocity in SystemAPI.Query<RefRW<PhysicsVelocity>>().WithAll<HeldByPlayer>())
//         {
//             velocity.ValueRW.Linear = float3.zero;
//             velocity.ValueRW.Angular = float3.zero;
//         }
//     }
// }