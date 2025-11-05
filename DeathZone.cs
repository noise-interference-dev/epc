using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float time_here = 30;
    public float cur_time_here = 0.1f;
    public GameObject player_here;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            player_here = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other) {
        player_here = null;
        cur_time_here = time_here;
    }
    public void Start() {
        cur_time_here = time_here;
    }
    public void FixedUpdate() {
        if (player_here) {
            cur_time_here = - Time.deltaTime;
            if (cur_time_here <= 0) {
                player_here.GetComponent<Fpc>().OnClickRespawn();
                cur_time_here = time_here;
            }
        }
    }
}
