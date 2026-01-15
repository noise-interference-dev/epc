using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_perl : MonoBehaviour
{
    public bool is_active;
    public Vector3 pos = new Vector3(0, 0, 0.85f);
    public Collider coll;
    public Renderer rend;
    public GameObject player;

    public void Start() {
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.GetComponent<PlayerController>()) {
            GetComponent<Rigidbody>().isKinematic = true;
            // rend.enabled = false;
            // coll.enabled = false;
            player.transform.position = transform.position + new Vector3(0, 0.75f, 0);
            // transform.localPosition = pos;
            Destroy(this.gameObject);
        }
    }
}
