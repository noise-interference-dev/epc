using Unity.Entities;
using Unity.Mathematics;

public struct Placeable : IComponentData { }
public struct HeldByPlayer : IComponentData { }
public struct HoldOffset : IComponentData { public float3  Value; }