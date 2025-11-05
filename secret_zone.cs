using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secret_zone : MonoBehaviour
{
    public float time_here = 5;
    public float cur_time_here = 0.1f;
    public GameObject player;
    public Vector3 sec_pos; // secret position
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            player = other.gameObject;
    }
    private void OnTriggerExit(Collider other) {
        player = null;
        cur_time_here = time_here;
    }
    public void Start() {
        cur_time_here = time_here;
    }
    public void FixedUpdate() {
        if (player) {
            cur_time_here -= Time.deltaTime;
            if (cur_time_here <= 0) {
                player.transform.position = sec_pos;
                cur_time_here = time_here;
            }
        }
    }
}
