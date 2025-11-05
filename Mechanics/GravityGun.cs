using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityGun : MonoBehaviour
{
    public float distance;

    public Vector3 rot;
    public Quaternion trot;

    public Vector3 pos;

    public float maxGrabDist = 15f;
    public float throwForce = 20f;
    public Transform sphere;

    public GameObject grabbedObj;
    public Vector3 asd;
    [SerializeField] private Camera mainCamera;
    Rigidbody g_rb;
    
    [Header("Скрипты")] 
    public Inventory Inventory;
    public Asembler Asembler;

    public void Aweke() {
        Asembler = GetComponent<Asembler>();
        Inventory = GetComponent<Inventory>();
    }
    public void setRotateXPlus() {
        if(rot.x > 350f)
            rot.x = 0f;
        rot.x += 10f;
    }
    public void setRotateXMin() {
        if(rot.x < -350f)
            rot.x = 0f;
        rot.x -= 10f;
    }
    public void setRotateYPlus() {
        if(rot.y > 350f)
            rot.y = 0f;
        rot.y += 10f;
    }
    public void setRotateYMin() {
        if(rot.y < -350f)
            rot.y = 0f;
        rot.y -= 10f;
    }
    public void setRotateZPlus() {
        if(rot.z > 350f)
            rot.z = 0f;
        rot.z += 10f;
    }
    public void setRotateZMin() {
        if(rot.z < -350f)
            rot.z = 0f;
        rot.z -= 10f;
    }

    public void add_distance(float dist) {
        if (distance > 30f) distance = 30f; 
        else if (distance < 3f) distance = 3f;
        distance += dist;
        sphere.transform.localPosition = new Vector3(0, 0, distance);
    }

    public void grabObject() {
        if (g_rb)
        {
            g_rb.isKinematic = false;
            GunReset();
        }
        else
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            // Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
            if (Physics.Raycast(ray, out hit, maxGrabDist))
            {
                if (hit.transform.gameObject.GetComponent<prefab_prop>())
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    grabbedObj = hit.transform.gameObject;
                    if (grabbedObj.GetComponent<prefab_prop>().p_grv)
                    {
                        g_rb = hit.rigidbody;
                        // sphere.position = hit.point;
                        asd = hit.point;
                        // distance = grabbedObj.transform.localPosition.z;

                        rot = new Vector3(grabbedObj.transform.rotation.x, grabbedObj.transform.rotation.y, grabbedObj.transform.rotation.z);
                        if (g_rb)
                            g_rb.isKinematic = true;
                    }
                }
            }
        }
    }

    public void freezeGrabbedObject()
    {
        if (g_rb == null) return;
        g_rb.isKinematic = true;
        GunReset();
    }

    public void throwGrabbedObject()
    {
        if (g_rb == null) return;
        g_rb.isKinematic = false;
        g_rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        GunReset();
    }

    public void GunReset()
    {
        g_rb = null;
        grabbedObj = null;
        rot = new Vector3(0,0,0);
        distance = 3f;
    }

    public void LateUpdate()
    {
        if (grabbedObj != null)
            trot = Quaternion.Lerp(grabbedObj.transform.rotation, Quaternion.Euler(rot), 0.6f);

        pos = new Vector3(sphere.transform.position.x,
                          sphere.transform.position.y,
                          sphere.transform.position.z);
        // var rot = new Vector3(sphere.transform.rotation.x, sphere.transform.rotation.y, sphere.transform.rotation.z) + new Vector3(mainCamera.rotation.x, mainCamera.rotation.y, mainCamera.rotation.z);
        if (g_rb)
        {
            g_rb.position = pos;
            g_rb.rotation = trot;
        }
    }
}
