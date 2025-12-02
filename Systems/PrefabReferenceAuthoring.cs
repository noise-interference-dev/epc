using Unity.Entities;
using UnityEngine;

public class PrefabReferenceAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

public class PrefabReferenceBaker : Baker<PrefabReferenceAuthoring>
{
    public override void Bake(PrefabReferenceAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent<PrefabSingletonTag>(entity);
        AddComponent(entity, new PrefabEntityHolder
        {
            PrefabEntity = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic)
        });
    }
}