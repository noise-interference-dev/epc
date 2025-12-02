using UnityEngine;

public class Melee : Weapon, IWeapon
{
    private float lastAttackTime = 0f;

    public void Shoot()
    {
        if (Time.time - lastAttackTime < fireRate) return;
        Debug.Log("Attack");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        foreach (var hitCollider in hitColliders)
        {
            /*if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }*/
        }

        lastAttackTime = Time.time;

    }

    public void Reload() { return; }
}