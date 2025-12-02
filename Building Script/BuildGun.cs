// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class BuildGun : MonoBehaviour {
    // [Header("Переменные")]

    // [Header("Настройки Строительства")] 
    // public Transform cameraTransform;
    // public Transform pointBuildSpawn;
    // public Transform parent;
    // public GameObject sphere;
    // public Vector3 pos;
    // public Vector3 rot;
    // public GameObject prefab;
    // public GameObject spawned;
    
    // [Header("Настройки Скриптов")] 
    // public Inventory inventory;
    // public UnitAsembler asembler;
    // // Quaternion trot;
    
    // private void Awake() 
    // {
    //     if (inventory == null) inventory = FindAnyObjectByType<Inventory>();
    //     if (asembler == null) asembler = FindAnyObjectByType<UnitAsembler>();
    // }


    // public void Update() {
    //     // if (spawned) spawned.transform.rotation = Quaternion.Euler(spawned.GetComponent<BuildPrefab>().prot);
    //     // trot = Quaternion.Lerp(spawned.transform.rotation, Quaternion.Euler(rot + spawned.GetComponent<BuildPrefab>().prot), 0.5f);
    //     // spawned.transform.rotation = trot;
    //     if (prefab) {
    //         if (spawned != null)
    //         {
    //             if (spawned.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
    //             {
    //                 // Vector3 pose = bp.ppos;
    //                 // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
    //                 //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
    //                 //                 Mathf.Round((sphere.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
    //                 // spawned.transform.position = Vector3.Lerp(spawned.transform.position, pos, 10f * Time.deltaTime);
    //             }
    //             // else
    //             // {   
    //             //     spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
    //             // }
    //         }
    //     }
    //     /*if(inventory.usedGunAll != 1) {
    //         if(spawned.gameObject != null) {
    //             Destroy(spawned.gameObject);
    //         }
    //         prefab = null;
    //         spawnDistance = 3f;
    //         rot = new Vector3(0f,0f,0f);
    //     }
    //     // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
    //     // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        
    // }
    // public void OnValidate() {*/
    // }
}
