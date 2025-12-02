using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public float distance;
    public Transform GunTransform { get; private set;  }

    public float maxGrabDist = 15f;
    public float throwForce = 20f;
    public Transform sphere;

    public GameObject grabbedObj;
    Rigidbody g_rb;
    
    // [Header("Скрипты")] 
    // public Inventory Inventory;
    // public Asembler Asembler;

    // public void Aweke() {
    //     Asembler = GetComponent<Asembler>();
    //     Inventory = GetComponent<Inventory>();
    // }
}
