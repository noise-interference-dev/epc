// // using System.Collections;
// // using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class BuildManager : MonoBehaviour {
//     // [Header("Переменные")]
//     // public float gridCurrent;
//     // private float gridLarge = 1.5f;
//     // private float gridSmall = 0.1f;
//     // public float spawnDistance;

//     // [Header("Настройки Строительства")] 
//     // public Transform cameraTransform;
//     // public Transform pointBuildSpawn;
//     // public Transform parent;
//     // public GameObject sphere;
//     // public Vector3 pos;
//     // public Vector3 rot;
//     // public GameObject prefab;
//     // public GameObject spawned;
    
//     // [Header("Настройки Скриптов")] 
//     // public InventoryController inventory;
//     // public MapAsembler asembler;
//     // Quaternion trot;
    
//     // private void Awake() 
//     // {
//     //     if (inventory == null) inventory = FindAnyObjectByType<InventoryController>();
//     //     if (asembler == null) asembler = FindAnyObjectByType<MapAsembler>();
//     //     cameraTransform = Camera.main.transform;
//     //     gridCurrent = gridLarge;
//     // }

//     // public void ToggleGridSize() 
//     // {
//     //     gridCurrent = (gridCurrent == gridLarge) ? gridSmall : gridLarge;
//     // }

//     // public void setGrid() {
//     //     if (gridCurrent == 1.5f) gridCurrent = 0.1f;
//     //     else gridCurrent = 1.5f;
//     // }

//     // public void propSet(GameObject prop) {
//     //     prefab = prop;
//     //     Destroy(spawned);
//     //     // spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
//     // }
    
//     // public void propSpawn() {   
//     //     if(prefab != null) {
//     //         asembler.propSpawn(spawned, pos, rot);
//     //         // spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
//     //         /*if (asembler.prop_checker()) {
//     //             if (spawned.GetComponent<BuildPrefab>().Place(pos, rot)) {
//     //                 spawned = null;
//     //                 asembler.props += 1;
//     //                 return;
//     //             }
//     //             spawned.transform.rotation = trot;
//     //             spawned.transform.position = pos;
//     //         }*/
//     //     }
//     // }

//     // public void SetRotation(Vector3 rote) => rot += rote;

//     // public void SetDistance(float dist) {
//     //     if (spawnDistance > 30f) spawnDistance = 30f; 
//     //     else if (spawnDistance < 3f) spawnDistance = 3f;
//     //     spawnDistance += dist;
//     //     sphere.transform.localPosition = new Vector3(0, 0, spawnDistance);
//     // }

//     // public void propCancel() {
//     //     if(spawned != null) Destroy(spawned);
//     //     prefab = null;
//     //     spawnDistance = 3f;
//     //     rot = new Vector3(0f,0f,0f);
//     // }

//     // public void Update() {
//     //     // if (spawned) spawned.transform.rotation = Quaternion.Euler(spawned.GetComponent<BuildPrefab>().prot);
//     //     // trot = Quaternion.Lerp(spawned.transform.rotation, Quaternion.Euler(rot + spawned.GetComponent<BuildPrefab>().prot), 0.5f);
//     //     // spawned.transform.rotation = trot;
//     //     if (prefab) {
//     //         if (spawned != null)
//     //         {
//     //             if (spawned.TryGetComponent<BuildPrefab>(out BuildPrefab bp))
//     //             {
//     //                 // Vector3 pose = bp.ppos;
//     //                 // pos = new Vector3(Mathf.Round((pointBuildSpawn.transform.position.x + pose.x) / gridCurrent) * gridCurrent,
//     //                 //                 Mathf.Round((pointBuildSpawn.transform.position.y + pose.y) / gridCurrent) * gridCurrent,
//     //                 //                 Mathf.Round((sphere.transform.position.z + pose.z) / gridCurrent) * gridCurrent);
//     //                 // spawned.transform.position = Vector3.Lerp(spawned.transform.position, pos, 10f * Time.deltaTime);
//     //             }
//     //             // else
//     //             // {   
//     //             //     spawned = Instantiate(prefab, pos, Quaternion.Euler(rot + prefab.GetComponent<BuildPrefab>().prot), parent);  
//     //             // }
//     //         }
//     //     }
//     //     /*if(inventory.usedGunAll != 1) {
//     //         if(spawned.gameObject != null) {
//     //             Destroy(spawned.gameObject);
//     //         }
//     //         prefab = null;
//     //         spawnDistance = 3f;
//     //         rot = new Vector3(0f,0f,0f);
//     //     }
//     //     // Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
//     //     // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        
//     // }
//     // public void OnValidate() {*/
//     // }
// }
