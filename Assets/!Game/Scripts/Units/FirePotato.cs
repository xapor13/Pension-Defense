using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePotato : Unit
{
   [Header("Fire Potato Settings")]
    public int fireDamage = 100;
    public float burnDuration = 3f;
    
    protected override void PerformAction()
    {
        Enemy target = FindClosestEnemy();
        if (target != null)
        {
            ShootFireAt(target);
        }
    }
    
    private void ShootFireAt(Enemy target)
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            FireProjectile fireProj = projectile.GetComponent<FireProjectile>();
            if (fireProj != null)
            {
                fireProj.Initialize(target.transform, fireDamage, burnDuration);
            }
        }
    }
}
