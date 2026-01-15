// using UnityEngine;

// public class Rifle : Weapon, IWeapon
// {
//     private float lastFireTime = 0f;
//     private float reloadTime = 2f;
//     private bool isReloading = false;

//     private void Start()
//     {
//         currentAmmo = maxAmmo;
//         WeaponController.UpdateAmmoCurrentDisplay();
//     }

//     public void Shoot()
//     {
//         if (isReloading || Time.time - lastFireTime < fireRate) return;
//         if (currentAmmo <= 0)
//         {
//             Reload();
//             return;
//         }

//         currentAmmo--;
//         WeaponController.UpdateAmmoCurrentDisplay();
//         Debug.Log("Shoot");

//         Ray ray = WeaponController.Instance.cameraPlayer.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
//         RaycastHit hit;

//         if (Physics.Raycast(ray, out hit, distance))
//         {
//             /*if (hit.collider.CompareTag("Enemy"))
//             {
//                 hit.collider.GetComponent<Enemy>().TakeDamage(damage);
//             }*/
//         }

//         lastFireTime = Time.time;
//     }

//     public void Reload()
//     {
//         if (isReloading) return;

//         if (currentAmmo >= maxAmmo) return;

//         int ammoCount = WeaponController.Instance.GetAmmoCount(ammoType);

//         if (ammoCount <= 0)
//         {
//             return;
//         }

//         Debug.Log("Reload");
//         isReloading = true;

//         Invoke("FinishReload", reloadTime);
//     }

//     private void FinishReload()
//     {
//         int ammoNeeded = maxAmmo - currentAmmo;
//         int availableAmmo = WeaponController.Instance.GetAmmoCount(ammoType);
//         int ammoToReload = Mathf.Min(ammoNeeded, availableAmmo);

//         if (ammoToReload <= 0)
//         {
//             isReloading = false;
//             return;
//         }

//         currentAmmo += ammoToReload;

//         WeaponController.Instance.DecreaseAmmo(ammoType, ammoToReload);

//         Debug.Log("Reload Finish");
//         isReloading = false;

//         WeaponController.Instance.UpdateAmmoCurrentDisplay();
//     }

// }