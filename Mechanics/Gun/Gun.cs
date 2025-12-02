using UnityEngine.Events;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Настройки Стрельбы")] 
    public UnityEvent OnGunShoot;
    public float FireCooldown;
    public float CurrentCooldown;
    public GameObject Gun_flash;
    // public GameObject ShootButton;
    public float Damage;
    public float bullet_dist;
    public Transform PlayerCam;

    [Header("Скрипты")] 
    public UnitAsembler Asembler;
    public Inventory Inventory;

    public void Aweke() {
        Asembler = GetComponent<UnitAsembler>();
        Inventory = GetComponent<Inventory>();
    }

    public void Start() {
        CurrentCooldown = FireCooldown;
    }

    public void FixedUpdate() {
        if(Inventory.gun_used == 3) {
            if(CurrentCooldown <= 0.5f)
                Gun_flash.SetActive(false);
            if(CurrentCooldown >= 0f)
                CurrentCooldown -= Time.deltaTime;
        }
    } 
 
    public void Shoot() {
        Ray ray_gun = new Ray(PlayerCam.position, PlayerCam.forward);
        if(Physics.Raycast(ray_gun, out RaycastHit hitInfo, bullet_dist)) {
            if(hitInfo.collider.gameObject.TryGetComponent(out Entity enemy))
                enemy.Health -= Damage;
        }
    }
    public void GunShoot() {
        if(CurrentCooldown <= 0f) {
            OnGunShoot?.Invoke();
            CurrentCooldown = FireCooldown;
            Gun_flash.SetActive(true);
        }
        if(CurrentCooldown <= 0f) {
            OnGunShoot?.Invoke();
            CurrentCooldown = FireCooldown;
            Gun_flash.SetActive(true);
        }
    }
}
