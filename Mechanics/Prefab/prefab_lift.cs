using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_lift : MonoBehaviour
{
    [Header("Настройки Лифта")]
    public bool is_active;
    public bool move_y;
    public float count;
    public float speed;
    public float add_y;
    public float max_y;
    public float min_y;
    public Vector3 pos_y;

    public void Start() {
        min_y = transform.position.y;
        max_y = transform.position.y + count;
        pos_y = transform.position;
    }

    public void FixedUpdate() {
        if (is_active) {
            transform.position = Vector3.Lerp(transform.position, pos_y, Time.deltaTime * speed);
            if (move_y) {
                if (transform.position.y > pos_y.y - 0.09) {
                    transform.position = new Vector3(transform.position.x, pos_y.y, transform.position.z);
                    is_active = false;
                }
            }
            else {
                if (transform.position.y < pos_y.y + 0.09) {
                    transform.position = new Vector3(transform.position.x, pos_y.y, transform.position.z);
                    is_active = false;
                }
            }
        }
    }

    public void move_y_up() {
        pos_y.y = Mathf.Round(transform.position.y + add_y);
        move_y = true;
        if (pos_y.y > max_y)
            pos_y.y = max_y;
    }
    public void move_y_down() {
        pos_y.y = Mathf.Round(transform.position.y - add_y);
        move_y = false;
        if (pos_y.y < min_y)
            pos_y.y = min_y;
    }
}
