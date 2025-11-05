using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Gun : MonoBehaviour
{
    public float Damage;
    public float BulletRange;
    public Transform PlayerCam;
 
    public void Shoot()
    {
        Ray gunRay = new Ray(PlayerCam.position, PlayerCam.forward);
        if(Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            if(hitInfo.collider.gameObject.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= Damage;
            }
        }
    }
}
