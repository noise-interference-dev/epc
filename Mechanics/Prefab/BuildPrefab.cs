using System.Collections.Generic;
using UnityEngine;

public class BuildPrefab : MonoBehaviour {
    public Collider[] colliders;
    public MeshFilter[] meshes;
    public Component[] rends;
    public List<Material> materials = new List<Material>();
    public Material canM;
    public Vector3 ppos;
    public Vector3 prot;

    private void Start() {
        colliders = GetComponentsInChildren<Collider>();
        rends = GetComponentsInChildren<Renderer>();
        meshes = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }
        for (int i = 0; i < rends.Length; i++) {
            materials.Add(rends[i].GetComponent<Renderer>().material);
            meshes[i].GetComponent<Renderer>().material = canM;
        }
    }
    public bool Place(Vector3 pos, Vector3 local) {
        transform.position = pos + ppos;
        transform.localEulerAngles = local + prot;
        // meshes.GetComponent<Renderer>().material = materials[i];
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = true;
        }
        for (int i = 0; i < materials.Count; i++) {
            meshes[i].GetComponent<Renderer>().material = materials[i];
        }
        Destroy(this);
        return true;
    }
}
