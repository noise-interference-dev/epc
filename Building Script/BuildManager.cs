using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {
    [Header("Переменные")] 
    public float gridCur;
    public float spawnDistance;

    [Header("Настройки Строительства")] 
    // public Camera mainCamera;
    public Transform pointBuildSpawn;
    public Transform parent;
    public GameObject sphere;
    public Vector3 pos;
    public Vector3 rot;
    public GameObject prefab;
    public GameObject spawned;
    
    [Header("Настройки Скриптов")] 
    public Inventory Inventory;
    public Asembler Asembler;
    // Quaternion trot;

    public void Awake() {
        Inventory = Transform.FindAnyObjectByType<Inventory>();
        Asembler = Transform.FindAnyObjectByType<Asembler>();
    }

    public void setGrid() {
        if (gridCur == 1.5f) gridCur = 0.1f;
        else gridCur = 1.5f;
    }

    public void propSet(GameObject prop) {
        prefab = prop;
        Destroy(spawned);
        spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
    }
    
    public void spawn() {   
        if(prefab != null) {
            Asembler.prop_spawn(spawned, pos, rot);
            spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
            /*if (Asembler.prop_checker()) {
                if (spawned.GetComponent<BuildPrefab>().Place(pos, rot)) {
                    spawned = null;
                    Asembler.props += 1;
                    return;
                }
                spawned.transform.rotation = trot;
                spawned.transform.position = pos;
            }*/
        }
    }

    public void SetTor(Vector3 rote) => rot += rote;

    public void SetDistance(float dist) {
        if (spawnDistance > 30f) spawnDistance = 30f; 
        else if (spawnDistance < 3f) spawnDistance = 3f;
        spawnDistance += dist;
        sphere.transform.localPosition = new Vector3(0, 0, spawnDistance);
    }

    public void propCancel() {
        if(spawned != null) Destroy(spawned);
        prefab = null;
        spawnDistance = 3f;
        rot = new Vector3(0f,0f,0f);
    }

    public void Update() {
        // if (spawned) spawned.transform.rotation = Quaternion.Euler(spawned.GetComponent<BuildPrefab>().prot);
        // trot = Quaternion.Lerp(spawned.transform.rotation, Quaternion.Euler(rot + spawned.GetComponent<BuildPrefab>().prot), 0.5f);
        // spawned.transform.rotation = trot;
        if (prefab) {
            if (spawned != null)
            {
                if (spawned.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
                {
                    Vector3 pose = bp.ppos;
                    pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCur) * gridCur,
                                    Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCur) * gridCur,
                                    Mathf.Round((sphere.transform.position.z + pose.z) / gridCur) * gridCur);
                    spawned.transform.position = Vector3.Lerp(spawned.transform.position, pos, 10f * Time.deltaTime);
                }
                else
                {   
                    spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
                }
            }
        }
        /*if(Inventory.usedGunAll != 1) {
            if(spawned.gameObject != null) {
                Destroy(spawned.gameObject);
            }
            prefab = null;
            spawnDistance = 3f;
            rot = new Vector3(0f,0f,0f);
        }
        // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        
    }
    public void OnValidate() {*/
    }
}
