using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_window : MonoBehaviour
{
    [Header("Настройки Двери")]
    public bool is_active;
    public float speed;
    public float add_y;
    public float max_y;
    public float min_y;
    public Vector3 rot_y;
    public float y_rot;

    public void Start() {
        min_y = transform.rotation.y - add_y;
        max_y = transform.rotation.y + add_y;
        rot_y = transform.localEulerAngles;
    }
    public void FixedUpdate() {
        if (is_active) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot_y), Time.deltaTime * speed);
            y_rot = transform.localEulerAngles.y;
            if (Mathf.Round(transform.localEulerAngles.y - 0.3f) == rot_y.y) {
                is_active = false;
                transform.rotation = Quaternion.Euler(rot_y);
            }
            if (Mathf.Round(transform.localEulerAngles.y + 0.3f) == 270f) {
                is_active = false;
                transform.rotation = Quaternion.Euler(rot_y);
            }
        }
    }
    public void move_z_plus() {
        rot_y.y += add_y;
        if (rot_y.y > max_y)
            rot_y.y = max_y;
    }
    public void move_z_minus() {
        rot_y.y -= add_y;
        if (rot_y.y < min_y)
            rot_y.y = min_y;
    }
}
