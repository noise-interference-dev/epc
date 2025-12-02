using Unity.Entities;

public struct PrefabSingletonTag : IComponentData { }
public struct PrefabEntityHolder : IComponentData
{
    public Unity.Entities.Entity PrefabEntity;
}