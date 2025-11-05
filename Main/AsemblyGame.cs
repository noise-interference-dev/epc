using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsemblyGame : MonoBehaviour
{
    [Header("Настройки")]
    public float dist;
    public Button yourButton;
    public GameObject use_butt;

    [Header("Настройки Строительства")] 
    [Header("Настройки Гравитации")] 
    [Header("Настройки Стрельбы")] 
    public float Damage;
    public float BulletRange;

    [Header("Настройки Двери")] 
    public GameObject door_obj;

    [Header("Настройки Окна")] 
    public GameObject wind_obj;
    [Header("Настройки Лифта")] 
    public GameObject lift_pack;
    public GameObject lift_obj;
    
    [Header("Скрипты")] 
    public Asembler asembler;


    public void start() {
        asembler = Transform.FindAnyObjectByType<Asembler>();
    }

    public void fixedUpdate() { // not fixed
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * dist, Color.black);
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * dist, out hit, dist)) {
            GameObject prop = hit.collider.gameObject;
            // Строительство
            // Гравитация
            // Стрельба
            if(prop.TryGetComponent(out Entity enemy)) {
                enemy.Health -= Damage;
            }
            // Дверь
            else if(prop.GetComponent<prefab_door>()) {
                door_obj = prop;
                use_butt.SetActive(true);
                Button btn = yourButton.GetComponent<Button>();
                btn.onClick.AddListener(door_move);
            }
            // Окно
            else if(prop.GetComponent<prefab_window>()) {
                wind_obj = prop;
                use_butt.SetActive(true);
                Button btn = yourButton.GetComponent<Button>();
                btn.onClick.AddListener(wind_move);
            }
            else {
                use_butt.SetActive(false);
            }
            // Лифт
            if(prop.GetComponent<prefab_lift>()) {
                lift_obj = hit.collider.gameObject;
                lift_pack.SetActive(true);
            }
            else {
                lift_pack.SetActive(false);
            }
        }
        else {
            use_butt.SetActive(false);
            lift_pack.SetActive(false);
        }
    }
    public void prop_delete() {
        RaycastHit hit;
        Ray ray = this.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

        if(Physics.Raycast(ray, out hit, 25f)) {
            GameObject prop = hit.transform.gameObject;
            if(prop.TryGetComponent<prefab_prop>(out prefab_prop pp)) {
                if (pp.p_del) asembler.prop_delete(prop);
            }
        }
    }
    public void wind_move() {
        if (!wind_obj.GetComponent<prefab_window>().is_active) {
            float rote_y = Mathf.Abs(Mathf.Round(transform.parent.localEulerAngles.y - 0.48f));
            if (rote_y < 90f || rote_y > 270f) {
                wind_obj.GetComponent<prefab_window>().move_z_minus();
                wind_obj.GetComponent<prefab_window>().is_active = true;
                Debug.Log("sdf");
            }
            else {
                wind_obj.GetComponent<prefab_window>().move_z_plus();
                wind_obj.GetComponent<prefab_window>().is_active = true;
                Debug.Log("fds");
            }
        }
    }
    public void door_move() {
        if (!door_obj.GetComponent<prefab_door>().is_active) {
            float rote_y = Mathf.Abs(Mathf.Round(transform.parent.localEulerAngles.y - 0.48f));
            if (rote_y < 90f || rote_y > 270f) {
                door_obj.GetComponent<prefab_door>().move_z_minus();
                door_obj.GetComponent<prefab_door>().is_active = true;
                Debug.Log("sdf");
            }
            else {
                door_obj.GetComponent<prefab_door>().move_z_plus();
                door_obj.GetComponent<prefab_door>().is_active = true;
                Debug.Log("fds");
            }
        }
    }

    public void lift_move_up() {
        if (!lift_obj.GetComponent<prefab_lift>().is_active) {
            lift_obj.GetComponent<prefab_lift>().move_y_up();
            lift_obj.GetComponent<prefab_lift>().is_active = true;
        }
    }

    public void lift_move_down() {
        if (!lift_obj.GetComponent<prefab_lift>().is_active) {
            lift_obj.GetComponent<prefab_lift>().move_y_down();
            lift_obj.GetComponent<prefab_lift>().is_active = true;
        }
    }
}
