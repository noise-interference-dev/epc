using System.Collections;
using UnityEngine;

public class BuildPrefab : MonoBehaviour 
{
    private Collider[] colliders;
    private Renderer[] renderers;
    private Material[] materials;
    [SerializeField] private  Material canM;
    [SerializeField] private Vector3 ppos;
    [SerializeField] private Vector3 prot;

    public bool Place(Vector3 _pos, Vector3 _rot)
    {
        transform.position = _pos + ppos;
        transform.localEulerAngles = _rot + prot;
        
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = materials[i];
        }
        
        Destroy(this);
        return true;
    }

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            renderers[i].material = canM;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}