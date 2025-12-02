using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections;

public class MassObjectSpawner : MonoBehaviour
{
    public int count = 250000;
    public void Start()
    {
        StartCoroutine(Starte());
    }
    private IEnumerator Starte()
    {
        yield return new WaitForSeconds(100f);
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        var query = em.CreateEntityQuery(ComponentType.ReadOnly<PrefabSingletonTag>());
        var holderEntity = query.GetSingletonEntity();

        var prefabHolder = em.GetComponentData<PrefabEntityHolder>(holderEntity);
        var prefabEntity = prefabHolder.PrefabEntity;

        for (int i = 0; i < count; i++)
        {
            var instance = em.Instantiate(prefabEntity);

            float3 pos = new float3(
                UnityEngine.Random.Range(-400f, 400f),
                UnityEngine.Random.Range(20f, 300f),
                UnityEngine.Random.Range(-400f, 400f));

            em.SetComponentData(instance, LocalTransform.FromPosition(pos));
        }

        Destroy(gameObject);
    }
}