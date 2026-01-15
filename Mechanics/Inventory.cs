// using System;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Inventory : MonoBehaviour
// {
//     public GameObject player;
//     // [SerializeField] private
//     public int gun_used;
//     public Camera mainCamera;
//     public GameObject st_bullet;

//     [SerializeField] private weapons guns;
//     [SerializeField] private doings does;

//     /*public GameObject objectBuildTool;
//     public GameObject panelBuildDoing;
//     public GameObject PanelBuildDopDoing;
    
//     public struct weapon {
//     }

//     public GameObject objectFisGun;
//     public GameObject panelFisgunDoing;
//     public GameObject PanelFisgunDopDoing;

//     public GameObject ObjectPistol;
//     public GameObject panelPistolDoing;

//     public GameObject obj_perl;
//     public Rigidbody perl_rb;
//     public float throwForce;*/


//     public bool cheker(int int_goted) {
//         if (guns.weaps[int_goted].is_goted) return true;
//         return false;
//     }

//     private void rend_guns(int int_goted) {
//         for (int i = 0; i < int_goted; i++) {
//             if (i == int_goted) guns.weaps[int_goted].weap.SetActive(true);
//             else guns.weaps[int_goted].weap.SetActive(false);
//         }
//     }

//     public void gun_set(int gun_chosed) {
//         if (!cheker(gun_chosed)) return;
//         rend_guns(gun_chosed);
//         if (gun_used == gun_chosed) gun_used = 0;
//         else gun_used = gun_chosed;
//     }

//     public void perl_throw() {
//         // obj_perl.GetComponent<prefab_perl>().coll.enabled = true;
//         // obj_perl.GetComponent<prefab_perl>().rend.enabled = true;
//         GameObject perl = Instantiate(guns.weaps[1].weap, this.transform.position, Quaternion.Euler(Vector3.zero), this.transform);
//         perl.transform.localPosition += guns.weaps[1].pos;
//         Rigidbody perl_rb = perl.GetComponent<Rigidbody>();
//         perl.GetComponent<prefab_perl>().player = player;
//         perl_rb.isKinematic = false;
//         perl_rb.AddForce(mainCamera.transform.forward * guns.weaps[1].force, ForceMode.VelocityChange);
//     }

//     /*public void FFixedUpdate() {
//         if (gun_used == 0) {
//             objectBuildTool.SetActive(false);
//             panelBuildDoing.SetActive(false);
//             PanelBuildDopDoing.SetActive(false);

//             objectFisGun.SetActive(false);
//             panelFisgunDoing.SetActive(false);
//             PanelFisgunDopDoing.SetActive(false);

//             ObjectPistol.SetActive(false);
//             panelPistolDoing.SetActive(false);
//         }
//         if (gun_used == 1) {
//             objectBuildTool.SetActive(true);
//             panelBuildDoing.SetActive(true);
//             PanelBuildDopDoing.SetActive(true);

//             objectFisGun.SetActive(false);
//             panelFisgunDoing.SetActive(false);
//             PanelFisgunDopDoing.SetActive(false);

//             ObjectPistol.SetActive(false);
//             panelPistolDoing.SetActive(false);
//         }
//         if (gun_used == 2) {
//             objectFisGun.SetActive(true);
//             panelFisgunDoing.SetActive(true);
//             PanelFisgunDopDoing.SetActive(true);

//             objectBuildTool.SetActive(false);
//             panelBuildDoing.SetActive(false);
//             PanelBuildDopDoing.SetActive(false);

//             ObjectPistol.SetActive(false);
//             panelPistolDoing.SetActive(false);
//         }
//         if (gun_used == 3) {
//             ObjectPistol.SetActive(true);
//             panelPistolDoing.SetActive(true);

//             objectBuildTool.SetActive(false);
//             panelBuildDoing.SetActive(false);
//             PanelBuildDopDoing.SetActive(false);

//             objectFisGun.SetActive(false);
//             panelFisgunDoing.SetActive(false);
//             PanelFisgunDopDoing.SetActive(false);
//         }
//     }*/
// }

// [Serializable]
// public class weapon {
//     public string name;
//     public GameObject weap;
//     public Vector3 pos;
//     public bool is_goted;
//     public bool is_raycast;
//     public GameObject bullet;
//     public Rigidbody rb;
//     public int force;
// }

// [Serializable]
// public class weapons {
//     public List<weapon> weaps = new List<weapon>();
// }

// [Serializable]
// public class doing {
//     public GameObject one;
//     public GameObject two;
//     public GameObject three;
// }

// [Serializable]
// public class doings {
//     public List<doing> doinges = new List<doing>();
// }
