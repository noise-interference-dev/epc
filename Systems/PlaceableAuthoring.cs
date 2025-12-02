using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class PlaceableAuthoring : MonoBehaviour
{
    public float mass = 1f;
    public float bounciness = 0.3f;
    public float friction = 0.6f;
}

public class PlaceableBaker : Baker<PlaceableAuthoring>
{
    public override void Bake(PlaceableAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<Placeable>(entity);
        AddComponent<PhysicsVelocity>(entity);
        AddComponent<PhysicsGravityFactor>(entity);
        AddComponent<PhysicsDamping>(entity);

        var mass = PhysicsMass.CreateDynamic(MassProperties.UnitSphere, authoring.mass);
        AddComponent(entity, mass);

        // Просто используем стандартный материал — отскок и трение работают по умолчанию
        // Если нужно кастомное — меняй в Physics Shape на префабе
    }
}