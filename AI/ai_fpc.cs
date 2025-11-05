using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Random;

public class ai_fpc : MonoBehaviour
{
    public bool idle, agressive, walking;
    public bool can_m;
    public bool sword, gun;
    public float speed;
    public List<GameObject> enemys = new List<GameObject>();
    [SerializeField] private Vector3 move;
    public int mult;
    public Vector3 dir;
    [SerializeField] float time_wait = 0f;
    [SerializeField] float time_idle = 2;

    private CharacterController ch_controller;

    public void Start() {
        ch_controller = GetComponent<CharacterController>();
    }

    public void Update() {
        if (time_idle >= 0f) {
            if (time_wait <= 0f) {
                time_wait = Random.Range(2, 5);
                time_idle = Random.Range(-3, -1);
                mult = Random.Range(-1, 1);
                dir = Vector3.zero;
            }
            else {
                time_wait -= Time.deltaTime;
                if (can_m) {
                    dir = (transform.right * move.x * mult + transform.forward * move.z * mult) * speed + transform.up * move.y; 
                    transform.LookAt(new Vector3(dir.x, 0, dir.z));
                    ch_controller.Move(dir * Time.deltaTime);
                }
            }
        }
        else {
            time_idle += Time.deltaTime;
        }
    }
}
