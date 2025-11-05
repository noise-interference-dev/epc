using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float StartingHealth;
    public float health;
    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            if(health <= 0f)
                Destroy(gameObject);
        }
    }
    public void Start() {
        Health = StartingHealth;
    }
}