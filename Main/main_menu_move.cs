using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_menu_move : MonoBehaviour {
    [SerializeField] private GameObject cam;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 rot;
    [SerializeField] private float speed;
    [SerializeField] private float speed_r;
    [SerializeField] private GameObject panel_single;
    [SerializeField] private GameObject panel_settings;
    [SerializeField] private GameObject panel_shop;
    public float intr;

    [SerializeField] private Animator anim;

    public void Start() {
        anim = FindAnyObjectByType<Animator>();//.GetComponent<Animator>();
        set_menu();
        anim.SetBool("open", true);
    }

    public void FixedUpdate() {
        if (cam.transform.position != pos && cam.transform.localRotation != Quaternion.Euler(rot)) {
            cam.transform.position = Vector3.Lerp(cam.transform.position, pos, speed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(rot), speed_r * Time.deltaTime);
        }
    }

    private IEnumerator waiting(GameObject got, float waite) {
        yield return new WaitForSeconds(waite);
        got.SetActive(true);
    }

    public void set_menu() {
        pos = new Vector3(4.25f, 4.5f, -4.25f);
        rot = new Vector3(27f, -31f, 0);
        anim.SetBool("open", true);
        panel_single.SetActive(false);
        panel_settings.SetActive(false);
        panel_shop.SetActive(false);
    }

    public void set_singleplayer() {
        pos = new Vector3(2f, 2.5f, 1f);
        rot = new Vector3(35f, -50f, 0);
        anim.SetBool("open", false);
        StartCoroutine(waiting(panel_single, intr));
    }

    public void set_settings() {
        pos = new Vector3(4f, 1.825f, 2.78f);
        rot = new Vector3(14f, -95, 0);
        anim.SetBool("open", false);
        StartCoroutine(waiting(panel_settings, intr));
    }

    public void set_shop() {
        pos = new Vector3(1.5f, 2.2f, 0.5f);
        rot = new Vector3(25f, -36f, 0);
        anim.SetBool("open", false);
        StartCoroutine(waiting(panel_shop, intr));
    }
}
